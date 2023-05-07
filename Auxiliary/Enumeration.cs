using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auxiliary
{
    /// <summary>
    /// A class to help with some enumeration issues.
    /// </summary>
    public static class Enumeration
    {
        /// <summary>
        /// Returns the component model attribute Description of a given enumeration.
        /// </summary>
        /// <param name="enumValue">The value of the enumeration.</param>
        /// <returns></returns>
        public static string GetEnumDescription(object enumValue)
        {
            var type = enumValue.GetType();
            var memInfo = type.GetMember(enumValue.ToString());
            var attributes = memInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false);
            return ((DescriptionAttribute)attributes[0]).Description;
        }

        /// <summary>
        /// Returns the enum name of a given description.
        /// </summary>
        /// <param name="description">The description.</param>
        /// <param name="enumerationType">The type of the enum.</param>
        /// <returns></returns>
        public static string GetEnumByDescription(string description, Type enumerationType)
        {
            string result = "";
            foreach (var item in Enum.GetNames(enumerationType))
            {
                var memInfo = enumerationType.GetMember(item);
                var attributes = memInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false);
                if (((DescriptionAttribute)attributes[0]).Description == description) result = item;
            }
            return result;
        }

    }
}
