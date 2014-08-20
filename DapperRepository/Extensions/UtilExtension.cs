using System;
using System.Globalization;
using System.Linq;

namespace DapperRepository.Extensions
{
    public static class UtilExtension
    {
        public static string[] AsSplit(this string str, string sperator)
        {
            return !str.IsNotNullOrEmpty()
                ? new string[] { }
                : str.Split(new[] { sperator }, StringSplitOptions.RemoveEmptyEntries);
        }
        public static bool IsString(this string obj, string str)
        {
            return (obj.IsNotNullOrEmpty()) && obj.Equals(str, StringComparison.InvariantCultureIgnoreCase);
        }

        public static bool IsNotNullOrEmpty(this object str)
        {
            return !String.IsNullOrWhiteSpace(str.AsString());
        }

        public static string AsString(this object obj)
        {
            return obj != null
                ? obj.ToString()
                : String.Empty;
        }

        public static int AsInt(this object obj)
        {
            int result;

            return int.TryParse(obj.AsString(), NumberStyles.Integer, null, out result) ? result : 0;
        }

        public static bool AsBoolString(this object str)
        {
            var items = new string[]
            {
                "YES", //YES/NO
                "Y", //Y/N
                "TRUE",//TRUE/FALSE
                "T",//T/F
                "1",//1/0
                "ON", //ON/OFF
            };

            return str.IsNotNullOrEmpty() && items.Any(a => a.IsString(str.AsString()));
        }

        public static bool IsEnum<T>(this string str) where T : struct
        {
            T t;

            return Enum.TryParse(str, true, out t);
        }

        public static T AsEnum<T>(this string str, T defaultValue = default(T)) where T : struct
        {
            T t;

            return Enum.TryParse(str, true, out t)
                ? t
                : defaultValue;
        }
    }
}
