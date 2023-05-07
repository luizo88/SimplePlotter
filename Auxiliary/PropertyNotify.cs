using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Auxiliary
{
    /// <summary>
    /// An object to implement the INotifyPropertyChanged and IDataErrorInfo interfaces.
    /// </summary>
    public class PropertyNotify : INotifyPropertyChanged, IDataErrorInfo
    {

        #region INTERFACE INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        /// <summary>
        /// Occurs when some property has changed.
        /// </summary>
        /// <param name="name"></param>
        public void OnPropertyChanged([CallerMemberName] string name = "")
        {
            if (PropertyChanged == null)
                return;
            PropertyChanged.Invoke(this, new PropertyChangedEventArgs(name));

        }

        /// <summary>
        /// Notifies the property change.
        /// </summary>
        /// <param name="property">The property name.</param>
        protected void NotifyPropertyChanged([System.Runtime.CompilerServices.CallerMemberName] string property = "")
        {
            if (this.PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(property));
        }

        #endregion

        #region INTERFACE IDataErrorInfo

        private Dictionary<string, string> errors = new Dictionary<string, string>();

        /// <summary>
        /// Generates an error to be used with "ValidatesOnDataErrors=True" in WPF.
        /// </summary>
        /// <param name="key">The name of the property (associated to the binding).</param>
        /// <param name="message">The error message.</param>
        public void AddError(string key, string message)
        {
            if (!errors.ContainsKey(key))
            {
                errors.Add(key, message);
            }
        }

        /// <summary>
        /// Removes an error when it's not more useful.
        /// </summary>
        /// <param name="key">The name of the property (associated to the binding).</param>
        public void RemoveError(string key)
        {
            if (errors.ContainsKey(key))
            {
                errors.Remove(key);
            }
        }

        public string Error
        {
            get { return String.Empty; }
        }

        public string this[string columnName]
        {
            get
            {
                string errorMessage = String.Empty;
                if (errors.ContainsKey(columnName))
                {
                    errorMessage = errors[columnName];
                }
                return errorMessage;
            }
        }

        #endregion

    }
}