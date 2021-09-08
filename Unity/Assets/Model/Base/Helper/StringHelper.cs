using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

namespace ETModel
{
	public static class StringHelper
	{
		public static IEnumerable<byte> ToBytes(this string str)
		{
			byte[] byteArray = Encoding.Default.GetBytes(str);
			return byteArray;
		}

		public static byte[] ToByteArray(this string str)
		{
			byte[] byteArray = Encoding.Default.GetBytes(str);
			return byteArray;
		}

	    public static byte[] ToUtf8(this string str)
	    {
            byte[] byteArray = Encoding.UTF8.GetBytes(str);
            return byteArray;
        }

		public static byte[] HexToBytes(this string hexString)
		{
			if (hexString.Length % 2 != 0)
			{
				throw new ArgumentException(String.Format(CultureInfo.InvariantCulture, "The binary key cannot have an odd number of digits: {0}", hexString));
			}

			var hexAsBytes = new byte[hexString.Length / 2];
			for (int index = 0; index < hexAsBytes.Length; index++)
			{
				string byteValue = "";
				byteValue += hexString[index * 2];
				byteValue += hexString[index * 2 + 1];
				hexAsBytes[index] = byte.Parse(byteValue, NumberStyles.HexNumber, CultureInfo.InvariantCulture);
			}
			return hexAsBytes;
		}

		public static string Fmt(this string text, params object[] args)
		{
			return string.Format(text, args);
		}

		public static string ListToString<T>(this List<T> list)
		{
			StringBuilder sb = new StringBuilder();
			foreach (T t in list)
			{
				sb.Append(t);
				sb.Append(",");
			}
			return sb.ToString();
		}
		
		public static string MessageToStr(object message)
		{
#if SERVER
			return MongoHelper.ToJson(message);
#else
			return Dumper.DumpAsString(message);
#endif
		}

        public static string EvaluationString(object instance, string data)
        {
            StringBuilder builder = new StringBuilder();
            foreach (var item in data.Split('}'))
            {
                string placeHolder = GetMiddleString(item + "}", "{", "}");
                var normalstring = item.Split('{');

                if (normalstring.Length > 1 && !string.IsNullOrEmpty(normalstring[0]))
                {
                    builder.Append(normalstring[0]);
                }

                if (!string.IsNullOrEmpty(placeHolder))
                {
                    builder.Append(GetValueFormAssmbly(instance, placeHolder));
                }
                else
                {
                    if (!string.IsNullOrEmpty(normalstring[0]))
                        builder.Append(normalstring[0]);
                }
            }
            return builder.ToString();
        }

        private static Dictionary<string, string> s_CachedValues = new Dictionary<string, string>();

        public static string GetValueFormAssmbly(object instance, string name)
        {
            if (string.IsNullOrEmpty(name))
                return string.Empty;

            string cachedValue;
            if (s_CachedValues.TryGetValue(name, out cachedValue))
                return cachedValue;

            int i = name.LastIndexOf('.');
            if (i < 0)
            {
                Type t = instance.GetType();
                if (t != null)
                {
                    var propertyValue = t.GetProperty(name)?.GetValue(instance);
                    if (propertyValue != null)
                    {
                        return propertyValue.ToString();
                    }
                    var fieldValue = t.GetField(name)?.GetValue(instance);
                    if (fieldValue != null)
                    {
                        return fieldValue.ToString();
                    }
                }
                return name;
            }
            var className = name.Substring(0, i);
            var propName = name.Substring(i + 1);
            foreach (var a in AppDomain.CurrentDomain.GetAssemblies())
            {
                Type t = a.GetType(className, false, false);
                if (t == null)
                {
                    continue;
                }
                try
                {
                    var pi = t.GetProperty(propName, BindingFlags.Static | BindingFlags.FlattenHierarchy | BindingFlags.Public);
                    if (pi != null)
                    {
                        var v = pi.GetValue(null, null);
                        if (v != null)
                        {
                            s_CachedValues.Add(name, v.ToString());
                            return v.ToString();
                        }
                    }
                    var fi = t.GetField(propName, BindingFlags.Static | BindingFlags.FlattenHierarchy | BindingFlags.Public);
                    if (fi != null)
                    {
                        var v = fi.GetValue(null);
                        if (v != null)
                        {
                            s_CachedValues.Add(name, v.ToString());
                            return v.ToString();
                        }
                    }
                }
                catch (Exception)
                {
                    // ignored
                }
            }
            return name;
        }

        public static string GetMiddleString(string sourse, string startstr, string endstr)
        {
            Regex rg = new Regex("(?<=(" + startstr + "))[.\\s\\S]*?(?=(" + endstr + "))", RegexOptions.Multiline | RegexOptions.Singleline);
            return rg.Match(sourse).Value;
        }
    }
}