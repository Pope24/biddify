using System.Text.RegularExpressions;

namespace Common
{
    public class EnumHelper
    {
        public static string ToDisplayString<TEnum>(TEnum value) where TEnum : Enum
        {
            return Regex.Replace(value.ToString(), "([a-z])([A-Z])", "$1 $2");
        }

        public static bool TryParseDisplayString<TEnum>(string display, out TEnum result) where TEnum : struct, Enum
        {
            var noSpace = display.Replace(" ", "");
            return Enum.TryParse(noSpace, ignoreCase: true, out result);
        }

        public static TEnum ParseDisplayString<TEnum>(string display) where TEnum : struct, Enum
        {
            var noSpace = display.Replace(" ", "");
            return Enum.Parse<TEnum>(noSpace, ignoreCase: true);
        }

        public static List<string> GetDisplayStrings<TEnum>() where TEnum : Enum
        {
            return Enum.GetValues(typeof(TEnum))
                       .Cast<TEnum>()
                       .Select(ToDisplayString)
                       .ToList();
        }

        public static Dictionary<TEnum, string> GetDisplayMap<TEnum>() where TEnum : Enum
        {
            return Enum.GetValues(typeof(TEnum))
                       .Cast<TEnum>()
                       .ToDictionary(val => val, val => ToDisplayString(val));
        }
    }
}
