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
        Gray = 4,
        [LocalizedDescription("White")]
        White = 5
    }

    [TypeConverter(typeof(EnumDescriptionTypeConverter))]
    public enum AxisLabelFormats
    {
        [LocalizedDescription("Default")]
        Default = 0,
        [LocalizedDescription("Scientific")]
        Scientific = 1,
        [LocalizedDescription("Engineering")]
        Engineering = 2
    }

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

    [TypeConverter(typeof(EnumDescriptionTypeConverter))]
    public enum ColorTemplates
    {
        [LocalizedDescription("Rainbow")]
        Rainbow = 0,
        [LocalizedDescription("Cycle")]
        Cycle = 1,
        [LocalizedDescription("GrayScale")]
        GrayScale = 2,
        [LocalizedDescription("RedScale")]
        RedScale = 3,
        [LocalizedDescription("GreenScale")]
        GreenScale = 4,
        [LocalizedDescription("BlueScale")]
        BlueScale = 5,
        [LocalizedDescription("BlueToRed")]
        BlueToRed = 6,
        [LocalizedDescription("RedToBlue")]
        RedToBlue = 7,
        [LocalizedDescription("GreenToRed")]
        GreenToRed = 8,
        [LocalizedDescription("RedToGreen")]
        RedToGreen = 9,
        [LocalizedDescription("SiliconSteel")]
        SiliconSteel = 10
    }

}