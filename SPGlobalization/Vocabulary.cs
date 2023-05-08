using System;
using System.Collections.Generic;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;

namespace SPGlobalization
{
    public sealed class Vocabulary : Auxiliary.PropertyNotify
    {
        private static Vocabulary instance = new Vocabulary();
        private Languages actualLanguage;
        private List<Languages> availableLanguages = new List<Languages>();

        public Vocabulary()
        {
            loadLanguages();
            switch (System.Globalization.CultureInfo.CurrentCulture.CompareInfo.Name)
            {
                case "es":
                case "es-MX":
                    ActualLanguage = Languages.ES; break;
                case "pt-BR":
                    ActualLanguage = Languages.PT; break;
                default:
                    ActualLanguage = Languages.EN; break;
            }
        }

        #region PRIVATE METHODS

        private void loadLanguages()
        {
            foreach (var item in Enum.GetValues(typeof(Languages)))
            {
                availableLanguages.Add((Languages)item);
            }
        }

        private void setLanguageVocabulary(Languages newLanguage)
        {
            switch (newLanguage)
            {
                case Languages.EN:
                    Resources.Resources.Culture = System.Globalization.CultureInfo.GetCultureInfo("");
                    break;
                case Languages.ES:
                    Resources.Resources.Culture = System.Globalization.CultureInfo.GetCultureInfo("es");
                    break;
                case Languages.PT:
                    Resources.Resources.Culture = System.Globalization.CultureInfo.GetCultureInfo("pt-BR");
                    break;
                default:
                    Resources.Resources.Culture = System.Globalization.CultureInfo.GetCultureInfo("");
                    break;
            }
        }

        #endregion

        #region PROPERTIES

        public Languages ActualLanguage
        {
            get { return actualLanguage; }
            set
            {
                if (actualLanguage.Equals(value)) return;
                actualLanguage = value;
                setLanguageVocabulary(actualLanguage);
                NotifyAllChanges();
                NotifyPropertyChanged("ActualLanguage");
            }
        }

        public List<Languages> AvailableLanguages
        {
            get { return availableLanguages; }
        }

        public static Vocabulary Instance
        {
            get
            {
                return instance;
            }
        }

        public string DataSeries { get { return string.Format("{0}", Resources.Resources.DataSeries); } }
        public string AddDataSeries { get { return string.Format("{0}", Resources.Resources.AddDataSeries); } }
        public string Name { get { return string.Format("{0}", Resources.Resources.Name); } }
        public string Scale { get { return string.Format("{0}", Resources.Resources.Scale); } }
        public string RemoveDataSeries { get { return string.Format("{0}", Resources.Resources.RemoveDataSeries); } }
        public string ArrowUp { get { return string.Format("{0}", Resources.Resources.ArrowUp); } }
        public string ArrowDown { get { return string.Format("{0}", Resources.Resources.ArrowDown); } }
        public string AxisConfiguration { get { return string.Format("{0}", Resources.Resources.AxisConfiguration); } }
        public string XScale { get { return string.Format("{0}", Resources.Resources.XScale); } }
        public string YScale { get { return string.Format("{0}", Resources.Resources.YScale); } }
        public string ScaledX { get { return string.Format("{0}", Resources.Resources.ScaledX); } }
        public string ScaledY { get { return string.Format("{0}", Resources.Resources.ScaledY); } }
        public string OriginalX { get { return string.Format("{0}", Resources.Resources.OriginalX); } }
        public string OriginalY { get { return string.Format("{0}", Resources.Resources.OriginalY); } }
        public string XLimits { get { return string.Format("{0}", Resources.Resources.XLimits); } }
        public string YLimits { get { return string.Format("{0}", Resources.Resources.YLimits); } }
        public string ChartBox { get { return string.Format("{0}", Resources.Resources.ChartBox); } }
        public string ChartSize { get { return string.Format("{0}", Resources.Resources.ChartSize); } }
        public string TimesNewRoman { get { return string.Format("{0}", Resources.Resources.TimesNewRoman); } }
        public string Garamond { get { return string.Format("{0}", Resources.Resources.Garamond); } }
        public string Futura { get { return string.Format("{0}", Resources.Resources.Futura); } }
        public string Bodoni { get { return string.Format("{0}", Resources.Resources.Bodoni); } }
        public string Arial { get { return string.Format("{0}", Resources.Resources.Arial); } }
        public string Helvetica { get { return string.Format("{0}", Resources.Resources.Helvetica); } }
        public string Verdana { get { return string.Format("{0}", Resources.Resources.Verdana); } }
        public string Rockwell { get { return string.Format("{0}", Resources.Resources.Rockwell); } }
        public string FranklinGothic { get { return string.Format("{0}", Resources.Resources.FranklinGothic); } }
        public string Univers { get { return string.Format("{0}", Resources.Resources.Univers); } }
        public string Frutiger { get { return string.Format("{0}", Resources.Resources.Frutiger); } }
        public string Avenir { get { return string.Format("{0}", Resources.Resources.Avenir); } }
        public string Consolas { get { return string.Format("{0}", Resources.Resources.Consolas); } }
        public string FontStyle { get { return string.Format("{0}", Resources.Resources.FontStyle); } }
        public string FontName { get { return string.Format("{0}", Resources.Resources.FontName); } }
        public string XAxisTitle { get { return string.Format("{0}", Resources.Resources.XAxisTitle); } }
        public string YAxisTitle { get { return string.Format("{0}", Resources.Resources.YAxisTitle); } }
        



        #endregion

        #region PUBLIC METHODS

        private void NotifyAllChanges()
        {
            var val = new List<string>();
            foreach (var item in this.GetType().GetProperties())
                NotifyPropertyChanged(item.Name);
        }

        #endregion

    }

}
