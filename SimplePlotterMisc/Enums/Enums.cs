using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimplePlotterMisc.Enums
{

    [TypeConverter(typeof(EnumDescriptionTypeConverter))]
    public enum Colors
    {
        [LocalizedDescription("Red")]
        Red = 0,
        [LocalizedDescription("Green")]
        Green = 1,
        [LocalizedDescription("Blue")]
        Blue = 2,
        [LocalizedDescription("Black")]
        Black = 3,
        [LocalizedDescription("Gray")]
        Gray = 4
    }


}