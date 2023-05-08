using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimplePlotterVM.Enums
{

    [TypeConverter(typeof(EnumDescriptionTypeConverter))]
    public enum Fonts
    {
        [LocalizedDescription("TimesNewRoman")]
        TimesNewRoman = 0,
        [LocalizedDescription("Garamond")]
        Garamond = 1,
        [LocalizedDescription("Futura")]
        Futura = 2,
        [LocalizedDescription("Bodoni")]
        Bodoni = 3,
        [LocalizedDescription("Arial")]
        Arial = 4,
        [LocalizedDescription("Helvetica")]
        Helvetica = 5,
        [LocalizedDescription("Verdana")]
        Verdana = 6,
        [LocalizedDescription("Rockwell")]
        Rockwell = 7,
        [LocalizedDescription("FranklinGothic")]
        FranklinGothic = 8,
        [LocalizedDescription("Univers")]
        Univers = 9,
        [LocalizedDescription("Frutiger")]
        Frutiger = 10,
        [LocalizedDescription("Avenir")]
        Avenir = 11,
        [LocalizedDescription("Consolas")]
        Consolas = 12
    }


}