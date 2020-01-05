using System;
using System.Globalization;
using System.Linq;
using System.Threading;

namespace MicroS_Common
{
    public abstract class FormatAttribute:Attribute
    {
        public abstract string Format(string value);
    }

    public class ToUpperCaseFormatAttribute : FormatAttribute
    {
        public override string Format(string value)
        {
            return value.Trim().ToUpperInvariant();
        }
    }
    public class ToLowerCaseFormatAttribute : FormatAttribute
    {
        public override string Format(string value)
        {
            return value.Trim().ToUpperInvariant();
        }
    }

    public class ToTileCaseFormatAttribute : FormatAttribute
    {
        public override string Format(string value)
        {
            CultureInfo cultureInfo = Thread.CurrentThread.CurrentCulture;
            TextInfo textInfo = cultureInfo.TextInfo;

            var values = value.Trim().Split(" ");
            return string.Join(" ", values.Select(v => textInfo.ToTitleCase(v)));
        }
    }
}
