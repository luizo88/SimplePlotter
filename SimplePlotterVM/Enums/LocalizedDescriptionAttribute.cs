using System;
using System.ComponentModel;
using System.Resources;
using System.Reflection;

namespace SimplePlotterVM.Enums
{

    public class LocalizedDescriptionAttribute : DescriptionAttribute
    {
        SPGlobalization.Vocabulary vocabulary = SPGlobalization.Vocabulary.Instance;
        PropertyInfo property;

        public LocalizedDescriptionAttribute(string PropertyName)
        {
            property = SPGlobalization.Vocabulary.Instance.GetType().GetProperty(PropertyName);
        }

        public override string Description
        {
            get
            {
                return property.GetValue(vocabulary).ToString();
            }
        }

    }

}
