using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace Extensions.Serialization
{
    public static partial class Utilities
    {
        /// <summary>
        ///     Conforms to RFC 4180 http://tools.ietf.org/html/rfc4180#page-2
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="input"></param>
        /// <param name="separator"></param>
        /// <param name="quotation"></param>
        /// <returns></returns>
        public static string ToCsv<T>(this IEnumerable<T> input, char separator = ',', char? quotation = null)
        {
            if (input == null) return null;
            var csv = new StringBuilder();
            foreach (var value in input)
            {
                csv.AppendFormat("{0}{1}{2}{3}", quotation, value, quotation, separator);
            }
            if (csv.Length != 0)
                return csv.ToString(0, csv.Length - 1);
            return string.Empty;
        }

        public static string ToCsv<T>(this IEnumerable<T> input, CultureInfo info, string format = "G",
            char separator = ',', char? quotation = null) where T : IFormattable
        {
            if (input == null) return null;
            var csv = new StringBuilder();
            foreach (var value in input)
            {
                csv.AppendFormat("{0}{1}{2}{3}", quotation, value.ToString(format, info), quotation, separator);
            }
            return csv.Length != 0 ? csv.ToString(0, csv.Length - 1) : string.Empty;
        }


        public static XDocument SerializeToXDoc<T>(this T source)
            where T : new()
        {
            var result = new XDocument();
            var serializer = new XmlSerializer(source.GetType()); // use .GetType() instead of typeof(T) http://stackoverflow.com/a/2434558/3922292
            using (var writer = result.CreateWriter())
            {
                serializer.Serialize(writer, source);
            }
            return result;
        }

        public static T Deserialize<T>(this XDocument serialized)
        {
            using (var reader = serialized.CreateReader())
            {
                var deserializer = new XmlSerializer(typeof(T));
                return (T)deserializer.Deserialize(reader);
            }
        }
    }
}
