using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimplePlotterMisc
{
    //Done by chatGPT... need to be corrected/improved... still, it works.
    public static class FTAlgorithm
    {
        /// <summary>
        /// Calcula o espectro de amplitude |X(f)| em 'terms' frequências, de 0 até Nyquist,
        /// a partir de amostras (X,Y) ordenadas em X. Suporta espaçamento não-uniforme em X
        /// via pesos do trapézio.
        ///
        /// Escala:
        /// - Para f=0 (DC): Amplitude = |X(0)| / T  (média ponderada no intervalo)
        /// - Para f>0:      Amplitude = 2*|X(f)| / T (aprox. amplitude do seno/cosseno)
        /// onde T = X_last - X_first.
        /// </summary>
        public static List<PointObj> RunFT1(
            List<PointObj> samples,
            int terms,
            bool ensureSortedByX = true,
            bool removeWeightedMean = true)
        {
            if (samples == null) throw new ArgumentNullException(nameof(samples));
            if (samples.Count < 2) throw new ArgumentException("É necessário pelo menos 2 pontos.", nameof(samples));
            if (terms < 1) throw new ArgumentOutOfRangeException(nameof(terms), "terms deve ser >= 1.");

            // Copia/ordena (se necessário)
            PointObj[] s = ensureSortedByX
                ? samples.OrderBy(p => p.X).ToArray()
                : samples.ToArray();

            int n = s.Length;

            // Verifica monotonicidade e calcula deltas
            double[] dt = new double[n - 1];
            for (int i = 0; i < n - 1; i++)
            {
                double d = s[i + 1].X - s[i].X;
                if (d <= 0)
                    throw new ArgumentException("Os pontos devem estar estritamente crescentes em X (sem repetição).");
                dt[i] = d;
            }

            double t0 = s[0].X;
            double t1 = s[n - 1].X;
            double T = t1 - t0;
            if (T <= 0) throw new ArgumentException("Intervalo total em X inválido.");

            // Estima fs a partir do intervalo total (mesmo com dt não-uniforme)
            // fs ~ (n-1)/T  => Nyquist ~ fs/2
            double fs = (n - 1) / T;
            double fMax = fs / 2.0;

            // Frequências de saída: 0 ... fMax (terms pontos)
            double df = (terms == 1) ? 0.0 : fMax / (terms - 1);

            // Pesos do trapézio para integral: w0=dt0/2, wi=(dt(i-1)+dt(i))/2, wN-1=dtN-2/2
            double[] w = new double[n];
            w[0] = dt[0] / 2.0;
            for (int i = 1; i < n - 1; i++)
                w[i] = (dt[i - 1] + dt[i]) / 2.0;
            w[n - 1] = dt[n - 2] / 2.0;

            // Opcional: remover média (DC) ponderada
            double[] y = new double[n];
            for (int i = 0; i < n; i++) y[i] = s[i].Y;

            if (removeWeightedMean)
            {
                double num = 0.0, den = 0.0;
                for (int i = 0; i < n; i++) { num += y[i] * w[i]; den += w[i]; }
                double mean = (den > 0) ? (num / den) : 0.0;
                for (int i = 0; i < n; i++) y[i] -= mean;
            }

            var result = new List<PointObj>(terms);
            const double TwoPi = 2.0 * Math.PI;

            // Cálculo direto: X(f) ≈ ∫ y(t) e^{-j2πft} dt ≈ Σ w_i y_i e^{-j2πf x_i}
            // Complexidade: O(n * terms)
            for (int k = 0; k < terms; k++)
            {
                double f = k * df;
                double re = 0.0;
                double im = 0.0;

                for (int i = 0; i < n; i++)
                {
                    double angle = -TwoPi * f * s[i].X; // rad
                    double c = Math.Cos(angle);
                    double si = Math.Sin(angle);
                    double a = y[i] * w[i];

                    re += a * c;
                    im += a * si;
                }

                double mag = Math.Sqrt(re * re + im * im);

                // Normalização para amplitude (aprox.):
                // - DC: média no intervalo (sem dobrar)
                // - demais: dobra para amplitude de seno/cosseno
                double amp = (k == 0) ? (Math.Abs(re) / T) : (2.0 * mag / T);

                result.Add(new PointObj(f, amp));
            }

            return result;
        }

        public static List<PointObj> RunFT2(
            IReadOnlyList<PointObj> samples,
            int terms,
            bool ensureSortedByX = true,
            bool removeWeightedMean = true,
            bool removeLinearTrend = true,
            bool applyHannWindow = true)
        {
            if (samples == null) throw new ArgumentNullException(nameof(samples));
            if (samples.Count < 2) throw new ArgumentException("É necessário pelo menos 2 pontos.", nameof(samples));
            if (terms < 1) throw new ArgumentOutOfRangeException(nameof(terms), "terms deve ser >= 1.");

            var s = ensureSortedByX ? samples.OrderBy(p => p.X).ToArray() : samples.ToArray();
            int n = s.Length;

            // deltas e checagem de monotonicidade
            double[] dt = new double[n - 1];
            for (int i = 0; i < n - 1; i++)
            {
                double d = s[i + 1].X - s[i].X;
                if (d <= 0) throw new ArgumentException("X deve ser estritamente crescente (sem repetição).");
                dt[i] = d;
            }

            double t0 = s[0].X;
            double t1 = s[n - 1].X;
            double T = t1 - t0;
            if (T <= 0) throw new ArgumentException("Intervalo total em X inválido.");

            // fs aproximada (para limitar Nyquist). Para uniformes, isso é consistente.
            double fs = (n - 1) / T;
            double fNyq = fs / 2.0;

            // Frequências alinhadas em k/T (k inteiro), mais estável para separar DC do resto
            double df = 1.0 / T;
            int maxTermsByNyq = (int)Math.Floor(fNyq / df) + 1;  // inclui k=0
            int m = Math.Min(terms, Math.Max(1, maxTermsByNyq));

            // pesos do trapézio
            double[] w = new double[n];
            w[0] = dt[0] / 2.0;
            for (int i = 1; i < n - 1; i++) w[i] = (dt[i - 1] + dt[i]) / 2.0;
            w[n - 1] = dt[n - 2] / 2.0;

            // vetor y
            double[] y = new double[n];
            for (int i = 0; i < n; i++) y[i] = s[i].Y;

            // remove média ponderada (DC)
            if (removeWeightedMean)
            {
                double num = 0.0, den = 0.0;
                for (int i = 0; i < n; i++) { num += y[i] * w[i]; den += w[i]; }
                double mean = den > 0 ? num / den : 0.0;
                for (int i = 0; i < n; i++) y[i] -= mean;
            }

            // remove tendência linear ponderada (opcional)
            if (removeLinearTrend)
            {
                // ajuste y ≈ a + b*x via mínimos quadrados ponderados
                double Sw = 0, Sx = 0, Sy = 0, Sxx = 0, Sxy = 0;
                for (int i = 0; i < n; i++)
                {
                    double wi = w[i];
                    double xi = s[i].X;
                    double yi = y[i];
                    Sw += wi;
                    Sx += wi * xi;
                    Sy += wi * yi;
                    Sxx += wi * xi * xi;
                    Sxy += wi * xi * yi;
                }
                double denom = (Sw * Sxx - Sx * Sx);
                if (Math.Abs(denom) > 0)
                {
                    double b = (Sw * Sxy - Sx * Sy) / denom;
                    double a = (Sy - b * Sx) / Sw;
                    for (int i = 0; i < n; i++) y[i] -= (a + b * s[i].X);
                }
            }

            // janela Hann (opcional) com correção de ganho coerente
            double coherentGain = 1.0;
            if (applyHannWindow)
            {
                // Hann: 0.5*(1 - cos(2πi/(n-1)))
                // ganho coerente = média dos coeficientes (para corrigir amplitude de seno alinhado ao bin)
                double sumWin = 0.0;
                for (int i = 0; i < n; i++)
                {
                    double win = 0.5 * (1.0 - Math.Cos(2.0 * Math.PI * i / (n - 1)));
                    y[i] *= win;
                    sumWin += win;
                }
                coherentGain = sumWin / n;
                if (coherentGain <= 0) coherentGain = 1.0;
            }

            var result = new List<PointObj>(m);
            const double TwoPi = 2.0 * Math.PI;

            for (int k = 0; k < m; k++)
            {
                double f = k * df;
                double re = 0.0, im = 0.0;

                for (int i = 0; i < n; i++)
                {
                    double angle = -TwoPi * f * s[i].X;
                    double c = Math.Cos(angle);
                    double si = Math.Sin(angle);
                    double a = y[i] * w[i];
                    re += a * c;
                    im += a * si;
                }

                double mag = Math.Sqrt(re * re + im * im);

                // Normalização:
                // X(f) ≈ ∫ y(t) e^{-j2πft} dt; amplitude de seno alinhado ao bin ≈ 2|X|/T
                // Não dobrar DC (k=0). Dobra apenas k>0. Corrigir janela (coherentGain).
                double amp = (k == 0) ? (mag / T) : (2.0 * mag / T);
                amp /= coherentGain;

                result.Add(new PointObj(f, amp));
            }

            return result;
        }

    }
}
