using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Common
{
    public class ConvertHelper
    {
        public static bool ToBoolean(object obj, bool defVal = false)
        {
            bool result = defVal;
            try
            {
                result = Convert.ToBoolean(obj);
                return result;
            }
            catch
            {
                return result;
            }
        }

        public static bool? ToBooleanComplete(object obj)
        {
            bool? result = null;
            try
            {
                if (obj == null)
                {
                    return result;
                }
                result = Convert.ToBoolean(obj);
                return result;
            }
            catch
            {
                return result;
            }
        }

        public static short ToInt16(object obj, short defaultVal = 0)
        {
            short result = defaultVal;
            try
            {
                result = Convert.ToInt16(obj);
                return result;
            }
            catch
            {
                return result;
            }
        }
        public static string ToBase64String(Bitmap bitmap)
        {
            try
            {
                MemoryStream ms = new MemoryStream();
                bitmap.Save(ms, ImageFormat.Jpeg);

                byte[] arr = new byte[ms.Length];
                ms.Position = 0;
                ms.Read(arr, 0, (int)ms.Length);
                ms.Close();

                return Convert.ToBase64String(arr);
            }
            catch
            {
                return string.Empty;
            }
        }
        public static short? ToInt16Complete(object obj)
        {
            short? result = null;
            try
            {
                if (obj == null)
                {
                    return result;
                }
                result = Convert.ToInt16(obj);
                return result;
            }
            catch
            {
                return result;
            }
        }

        public static int ToInt32(object obj, int defaultVal = 0)
        {
            int result = defaultVal;
            try
            {
                result = Convert.ToInt32(obj);
                return result;
            }
            catch
            {
                return result;
            }
        }

        public static int? ToInt32Complete(object obj)
        {
            int? result = null;
            try
            {
                if (obj == null)
                {
                    return result;
                }
                result = Convert.ToInt32(obj);
                return result;
            }
            catch
            {
                return result;
            }
        }

        public static long ToInt64(object obj, long defaultVal = 0L)
        {
            long result = defaultVal;
            try
            {
                result = Convert.ToInt64(obj);
                return result;
            }
            catch
            {
                return result;
            }
        }

        public static long? ToInt64Complete(object obj)
        {
            long? result = null;
            try
            {
                if (obj == null)
                {
                    return result;
                }
                result = Convert.ToInt64(obj);
                return result;
            }
            catch
            {
                return result;
            }
        }

        public static float ToSingle(object obj, float defaultVal = 0f)
        {
            float result = defaultVal;
            try
            {
                result = Convert.ToSingle(obj);
                return result;
            }
            catch
            {
                return result;
            }
        }

        public static float? ToSingleComplete(object obj)
        {
            float? result = null;
            try
            {
                if (obj == null)
                {
                    return result;
                }
                result = Convert.ToSingle(obj);
                return result;
            }
            catch
            {
                return result;
            }
        }

        public static double ToDouble(object obj, double defaultVal = 0.0)
        {
            double result = defaultVal;
            try
            {
                result = Convert.ToDouble(obj);
                return result;
            }
            catch
            {
                return result;
            }
        }

        public static double? ToDoubleComplete(object obj)
        {
            double? result = null;
            try
            {
                if (obj == null)
                {
                    return result;
                }
                result = Convert.ToDouble(obj);
                return result;
            }
            catch
            {
                return result;
            }
        }

        public static decimal ToDecimal(object obj, decimal defaultVal = 0m)
        {
            decimal result = defaultVal;
            try
            {
                result = Convert.ToDecimal(obj);
                return result;
            }
            catch
            {
                return result;
            }
        }

        public static decimal? ToDecimalComplete(object obj)
        {
            decimal? result = null;
            try
            {
                if (obj == null)
                {
                    return result;
                }
                result = Convert.ToDecimal(obj);
                return result;
            }
            catch
            {
                return result;
            }
        }

        public static DateTime ToDateTime(object obj)
        {
            return ToDateTime(obj, DateTime.MinValue);
        }

        public static DateTime ToDateTime(object obj, DateTime defaultVal)
        {
            DateTime result = defaultVal;
            try
            {
                result = Convert.ToDateTime(obj);
                return result;
            }
            catch
            {
                return result;
            }
        }

        public static DateTime? ToDateTimeComplete(object obj)
        {
            DateTime? result = null;
            try
            {
                if (obj == null)
                {
                    return result;
                }
                result = Convert.ToDateTime(obj);
                return result;
            }
            catch
            {
                return result;
            }
        }

        public static Guid ToGuid(object obj)
        {
            return ToGuid(obj, Guid.Empty);
        }

        public static Guid ToGuid(object obj, Guid defaultVal)
        {
            Guid result = defaultVal;
            try
            {
                result = Guid.Parse(obj.ToString());
                return result;
            }
            catch
            {
                return result;
            }
        }

        public static Guid? ToGuidComplete(object obj)
        {
            Guid? result = null;
            try
            {
                if (obj == null)
                {
                    return result;
                }
                result = Guid.Parse(obj.ToString());
                return result;
            }
            catch
            {
                return result;
            }
        }

        public static long? DateTime2TicksUTCComplete(DateTime? time, bool isUTC = false)
        {
            long? result = null;
            if (time.HasValue)
            {
                DateTime dateTime = new DateTime(1970, 1, 1);
                if (!isUTC)
                {
                    int num = -TimeZoneInfo.Local.BaseUtcOffset.Hours;
                    time = time.Value.AddHours(num);
                }
                result = (time.Value.Ticks - dateTime.Ticks) / 10000000;
            }
            return result;
        }

        public static long DateTime2TicksUTC(DateTime? time, bool isUTC = false)
        {
            long result = 0L;
            if (time.HasValue)
            {
                DateTime dateTime = new DateTime(1970, 1, 1);
                if (!isUTC)
                {
                    int num = -TimeZoneInfo.Local.BaseUtcOffset.Hours;
                    time = time.Value.AddHours(num);
                }
                result = (time.Value.Ticks - dateTime.Ticks) / 10000000;
            }
            return result;
        }

        public static long? DateTime2TicksComplete(DateTime? time, bool isUTC = false)
        {
            long? result = null;
            if (time.HasValue)
            {
                DateTime dateTime = new DateTime(1970, 1, 1);
                if (isUTC)
                {
                    int hours = TimeZoneInfo.Local.BaseUtcOffset.Hours;
                    time = time.Value.AddHours(hours);
                }
                result = (time.Value.Ticks - dateTime.Ticks) / 10000000;
            }
            return result;
        }

        public static long DateTime2Ticks(DateTime? time, bool isUTC = false)
        {
            long result = 0L;
            if (time.HasValue)
            {
                DateTime dateTime = new DateTime(1970, 1, 1);
                if (isUTC)
                {
                    int hours = TimeZoneInfo.Local.BaseUtcOffset.Hours;
                    time = time.Value.AddHours(hours);
                }
                result = (time.Value.Ticks - dateTime.Ticks) / 10000000;
            }
            return result;
        }

        public static DateTime? Ticks2DateTimeComplete(long? ticks, bool isUTC = false)
        {
            DateTime? result = null;
            if (ticks > 0)
            {
                TimeSpan value = new TimeSpan(ticks.Value * (long)Math.Pow(10.0, 7.0));
                result = new DateTime(1970, 1, 1).Add(value);
                if (isUTC)
                {
                    int hours = TimeZoneInfo.Local.BaseUtcOffset.Hours;
                    result = result.Value.AddHours(hours);
                }
            }
            return result;
        }

        public static DateTime Ticks2DateTime(long? ticks, bool isUTC = false)
        {
            DateTime result = DateTime.MinValue;
            if (ticks > 0)
            {
                TimeSpan value = new TimeSpan(ticks.Value * (long)Math.Pow(10.0, 7.0));
                result = new DateTime(1970, 1, 1).Add(value);
                if (isUTC)
                {
                    int hours = TimeZoneInfo.Local.BaseUtcOffset.Hours;
                    result = result.AddHours(hours);
                }
            }
            return result;
        }

        public static DateTime? Ticks2DateTimeUTCComplete(long? ticks, bool isUTC = false)
        {
            DateTime? result = null;
            if (ticks > 0)
            {
                TimeSpan value = new TimeSpan(ticks.Value * (long)Math.Pow(10.0, 7.0));
                result = new DateTime(1970, 1, 1).Add(value);
                if (!isUTC)
                {
                    int num = -TimeZoneInfo.Local.BaseUtcOffset.Hours;
                    result = result.Value.AddHours(num);
                }
            }
            return result;
        }

        public static DateTime Ticks2DateTimeUTC(long? ticks, bool isUTC = false)
        {
            DateTime result = DateTime.MinValue;
            if (ticks > 0)
            {
                TimeSpan value = new TimeSpan(ticks.Value * (long)Math.Pow(10.0, 7.0));
                result = new DateTime(1970, 1, 1).Add(value);
                if (!isUTC)
                {
                    int num = -TimeZoneInfo.Local.BaseUtcOffset.Hours;
                    result = result.AddHours(num);
                }
            }
            return result;
        }

        public static string[] String2ArrayString(string text, StringSplitOptions option = StringSplitOptions.RemoveEmptyEntries, params string[] separator)
        {
            string[] result = null;
            try
            {
                if (separator == null || separator.Length == 0)
                {
                    separator = new string[1]
                    {
                        ","
                    };
                }
                if (text == null)
                {
                    return result;
                }
                result = text.Split(separator, option);
                return result;
            }
            catch
            {
                return result;
            }
        }

        public static int[] String2ArrayInt32(string text, params string[] separator)
        {
            List<int> list = new List<int>();
            try
            {
                if (separator == null || separator.Length == 0)
                {
                    separator = new string[1]
                    {
                        ","
                    };
                }
                if (text != null)
                {
                    string[] array = text.Split(separator, StringSplitOptions.RemoveEmptyEntries);
                    for (int i = 0; i < array.Length; i++)
                    {
                        int item = ToInt32(array[i]);
                        list.Add(item);
                    }
                }
            }
            catch
            {
            }
            return list.ToArray();
        }

        public static Dictionary<string, string> String2StringMap(string text, string rowSeparator, string cellSeparator)
        {
            Dictionary<string, string> dictionary = new Dictionary<string, string>();
            try
            {
                string[] array = String2ArrayString(text, StringSplitOptions.RemoveEmptyEntries, rowSeparator);
                for (int i = 0; i < array.Length; i++)
                {
                    string[] array2 = String2ArrayString(array[i], StringSplitOptions.None, cellSeparator);
                    if (array2.Length != 0 && !string.IsNullOrEmpty(array2[0]))
                    {
                        string key = array2[0];
                        string value = string.Empty;
                        if (array2.Length > 1)
                        {
                            value = array2[1];
                        }
                        dictionary[key] = value;
                    }
                }
                return dictionary;
            }
            catch
            {
                return dictionary;
            }
        }

        public static string Map2String(Dictionary<string, string> map, string rowSeparator, string cellSeparator)
        {
            string text = string.Empty;
            foreach (KeyValuePair<string, string> item in map)
            {
                text += $"{rowSeparator}{item.Key}{cellSeparator}{item.Value}";
            }
            if (!string.IsNullOrEmpty(text) && !string.IsNullOrEmpty(cellSeparator))
            {
                text = text.Substring(cellSeparator.Length);
            }
            return text;
        }

        public static int[] StringArray2Int32(string[] array)
        {
            int[] array2 = null;
            if (array != null && array.Length != 0)
            {
                array2 = new int[array.Length];
                for (int i = 0; i < array.Length; i++)
                {
                    array2[i] = ToInt32(array[i]);
                }
            }
            return array2;
        }

        public static List<T> Array2List<T>(T[] array)
        {
            List<T> list = new List<T>();
            if (array != null && array.Length >= 0)
            {
                list.AddRange(array);
            }
            return list;
        }

        public static T[] List2Array<T>(T[] list)
        {
            T[] result = new T[0];
            if (list != null && list.Length >= 0)
            {
                result = list.ToArray();
            }
            return result;
        }

        public static string String2Unicode(string source)
        {
            byte[] bytes = Encoding.Unicode.GetBytes(source);
            StringBuilder stringBuilder = new StringBuilder();
            for (int i = 0; i < bytes.Length; i += 2)
            {
                stringBuilder.AppendFormat("\\u{0}{1}", bytes[i + 1].ToString("x").PadLeft(2, '0'), bytes[i].ToString("x").PadLeft(2, '0'));
            }
            return stringBuilder.ToString();
        }

        public static string Unicode2String(string source)
        {
            return new Regex("\\\\u([0-9A-F]{4})", RegexOptions.IgnoreCase | RegexOptions.Compiled).Replace(source, (Match x) => string.Empty + Convert.ToChar(Convert.ToUInt16(x.Result("$1"), 16)).ToString());
        }

        public static string EncodeBase64(string source)
        {
            return EncodeBase64(source, Encoding.UTF8);
        }

        public static string EncodeBase64(string source, Encoding encode)
        {
            string result = string.Empty;
            byte[] bytes = encode.GetBytes(source);
            try
            {
                result = Convert.ToBase64String(bytes);
                return result;
            }
            catch
            {
                return result;
            }
        }

        public static string DecodeBase64(string cipher)
        {
            return DecodeBase64(cipher, Encoding.UTF8);
        }

        public static string DecodeBase64(string cipher, Encoding encode)
        {
            string result = "";
            byte[] bytes = Convert.FromBase64String(cipher);
            try
            {
                result = encode.GetString(bytes);
                return result;
            }
            catch
            {
                return result;
            }
        }

        public static string UnTrimComma(string idString)
        {
            if (!string.IsNullOrEmpty(idString))
            {
                idString = idString.Trim(',');
                if (!string.IsNullOrEmpty(idString))
                {
                    idString = $",{idString},";
                }
            }
            return idString;
        }

        public static string TrimComma(string idString)
        {
            if (!string.IsNullOrEmpty(idString))
            {
                idString = idString.Trim(',');
            }
            return idString;
        }

        public static string AddIDToIDString(int id, string idString, bool withComma)
        {
            string empty = string.Empty;
            List<int> list = new List<int>();
            if (string.IsNullOrEmpty(idString))
            {
                list.Add(id);
            }
            else
            {
                list = new List<int>(String2ArrayInt32(idString));
                if (!list.Contains(id))
                {
                    list.Add(id);
                }
            }
            empty = string.Join(",", list);
            if (withComma)
            {
                empty = $",{empty},";
            }
            return empty;
        }

        public static bool IsIdyInString(string idy, string idString)
        {
            bool result = false;
            if (idy == null && idString == null)
            {
                result = true;
            }
            else if (idy != null && idString != null)
            {
                result = (idString.IndexOf(idy) > -1);
            }
            return result;
        }

        public static bool IsIDInString(int? id, string idString)
        {
            bool result = false;
            if (id.HasValue)
            {
                result = new List<int>(String2ArrayInt32(idString)).Contains(id.Value);
            }
            return result;
        }

        public static bool IsStringInArray(string item, params string[] array)
        {
            if (item == null || array == null)
            {
                return false;
            }
            return new List<string>(array).Contains(item);
        }

        public static string Trim(string text)
        {
            string text2 = text;
            if (text2 != null)
            {
                text2 = text2.Trim();
            }
            return text2;
        }

        public static string ToString(object obj)
        {
            string text = null;
            if (obj != null)
            {
                text = obj.ToString();
                if (text != null)
                {
                    text = text.Trim();
                }
            }
            return text;
        }

        public static string ToString(decimal? number, string format)
        {
            string empty = string.Empty;
            if (number.HasValue)
            {
                try
                {
                    return number.Value.ToString(format);
                }
                catch
                {
                    return number.ToString();
                }
            }
            return empty;
        }

        public static string ToString(double? number, string format)
        {
            string empty = string.Empty;
            if (number.HasValue)
            {
                try
                {
                    return number.Value.ToString(format);
                }
                catch
                {
                    return number.ToString();
                }
            }
            return empty;
        }

        public static string ToString(DateTime? time, string format)
        {
            string result = string.Empty;
            try
            {
                if (!time.HasValue)
                {
                    return result;
                }
                result = time.Value.ToString(format);
                return result;
            }
            catch
            {
                return result;
            }
        }

        public static bool StrIn(string sourceStr, string code)
        {
            bool result = false;

            if (("|" + sourceStr + "|").IndexOf("|" + code + "|") > -1)
            {
                result = true;
            }

            return result;
        }

        public static string ToJsonString(Dictionary<string, string> keyValues)
        {
            List<string> result = new List<string>();

            foreach(var item in keyValues)
            {
                result.Add($"\"{item.Key}\":\"{item.Value}\"");
            }

            return result.Count() > 0 ? $"[{{{string.Join(",", result)}}}]" : "";
        }

        public static string GetDictionary(Dictionary<string, string> map, string key)
        {
            string result = null;

            map.TryGetValue(key, out result);

            return result;
        }

        public static bool GetDictionary(Dictionary<string, bool> map, string key)
        {
            bool result = false;

            map.TryGetValue(key, out result);

            return result;
        }

        public static string ToDateTimeComplate(object paramter, string type = null)
        {
            string result = null;

            if (paramter != null && paramter != DBNull.Value && ToString2(paramter) != "")
            {
                if (type != null)
                {
                    result = Convert.ToDateTime(paramter).ToString(type);
                }
                else
                {
                    result = ConvertHelper.ToDateTime(paramter).ToString();
                }
            }

            return result;
        }

        public static string ToString2(object parameter)
        {
            string result = "";

            if (parameter != null && parameter != DBNull.Value)
            {
                result = Convert.ToString(parameter);
            }

            return result;
        }
    }
}
