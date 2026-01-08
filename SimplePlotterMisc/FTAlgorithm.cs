using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
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

        /// <summary>
        /// Calcula a FFT de um sinal real amostrado uniformemente.
        /// Entrada: pontos (X=tempo ou eixo amostral, Y=sinal). Pontos devem estar ordenados e com passo constante em X.
        /// Saída: lista de pontos (X=frequência [Hz], Y=magnitude).
        /// </summary>
        public static List<PointObj> RunFT3(
            IReadOnlyList<PointObj> samples,
            bool oneSided = true,
            bool removeMean = false,
            bool applyHannWindow = false,
            double uniformTolerance = 1e-9)
        {
            if (samples == null) throw new ArgumentNullException(nameof(samples));
            if (samples.Count < 2) throw new ArgumentException("É necessário pelo menos 2 amostras.", nameof(samples));

            // Garante ordenação por X (se já vier ordenado, o custo é pequeno; se não, corrige).
            var s = samples.OrderBy(p => p.X).ToArray();

            // Verifica amostragem uniforme e calcula dt e Fs.
            double dt = s[1].X - s[0].X;
            if (dt <= 0) throw new ArgumentException("X deve ser estritamente crescente.", nameof(samples));

            for (int i = 2; i < s.Length; i++)
            {
                double dti = s[i].X - s[i - 1].X;
                double err = Math.Abs(dti - dt);
                double scale = Math.Max(1.0, Math.Abs(dt));
                if (err > uniformTolerance * scale)
                {
                    throw new ArgumentException(
                        "A FFT requer amostragem uniforme em X. Detectada variação no passo entre pontos.",
                        nameof(samples));
                }
            }

            double fs = 1.0 / dt;

            // Zero padding para próxima potência de 2
            int n = s.Length;
            int nFft = NextPowerOfTwo(n);

            // Monta vetor complexo (real) com opções de pré-processamento
            double mean = 0.0;
            if (removeMean)
            {
                for (int i = 0; i < n; i++) mean += s[i].Y;
                mean /= n;
            }

            var x = new Complex[nFft];
            for (int i = 0; i < n; i++)
            {
                double v = s[i].Y - (removeMean ? mean : 0.0);

                if (applyHannWindow)
                {
                    // Janela de Hann no trecho útil (n amostras)
                    double w = 0.5 * (1.0 - Math.Cos(2.0 * Math.PI * i / (n - 1)));
                    v *= w;
                }

                x[i] = new Complex(v, 0.0);
            }
            for (int i = n; i < nFft; i++) x[i] = Complex.Zero;

            // FFT in-place
            FftInPlace(x);

            // Monta espectro de magnitude com normalização
            // Convenção: magnitude unilateral para sinal real:
            //  - DC (k=0): |X[0]|/N
            //  - Nyquist (k=N/2, se N par): |X[k]|/N
            //  - Demais bins (1..N/2-1): 2*|X[k]|/N
            int maxK = oneSided ? (nFft / 2) : (nFft - 1);
            var result = new List<PointObj>(maxK + 1);

            if (oneSided)
            {
                for (int k = 0; k <= nFft / 2; k++)
                {
                    double freq = k * fs / nFft;
                    double mag = x[k].Magnitude / nFft;

                    bool isDc = (k == 0);
                    bool isNyquist = (k == nFft / 2);

                    if (!isDc && !isNyquist)
                        mag *= 2.0;

                    result.Add(new PointObj(freq, mag));
                }
            }
            else
            {
                for (int k = 0; k <= maxK; k++)
                {
                    double freq = k * fs / nFft;       // espectro “0..Fs”
                    double mag = x[k].Magnitude / nFft; // normalizado por N
                    result.Add(new PointObj(freq, mag));
                }
            }

            return result;
        }

        // FFT iterativa Cooley-Tukey radix-2 (in-place)
        private static void FftInPlace(Complex[] buffer)
        {
            int n = buffer.Length;
            if ((n & (n - 1)) != 0) throw new ArgumentException("O tamanho do FFT deve ser potência de 2.");

            // Bit-reversal permutation
            int j = 0;
            for (int i = 1; i < n; i++)
            {
                int bit = n >> 1;
                while ((j & bit) != 0)
                {
                    j ^= bit;
                    bit >>= 1;
                }
                j ^= bit;

                if (i < j)
                {
                    (buffer[i], buffer[j]) = (buffer[j], buffer[i]);
                }
            }

            // Danielson-Lanczos
            for (int len = 2; len <= n; len <<= 1)
            {
                double ang = -2.0 * Math.PI / len;
                Complex wLen = new Complex(Math.Cos(ang), Math.Sin(ang));

                for (int i = 0; i < n; i += len)
                {
                    Complex w = Complex.One;
                    int half = len >> 1;

                    for (int k = 0; k < half; k++)
                    {
                        Complex u = buffer[i + k];
                        Complex v = w * buffer[i + k + half];
                        buffer[i + k] = u + v;
                        buffer[i + k + half] = u - v;
                        w *= wLen;
                    }
                }
            }
        }

        private static int NextPowerOfTwo(int v)
        {
            if (v < 1) return 1;
            int p = 1;
            while (p < v) p <<= 1;
            return p;
        }

    }
}
