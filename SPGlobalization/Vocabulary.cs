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
