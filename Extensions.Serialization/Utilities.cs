using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using CsvHelper;
using CsvHelper.Configuration;

namespace Extensions.Serialization
{
    public static partial class Utilities
    {
        #region Xml
        [Obsolete("Use Extensions.Serialization.Xml version instead of this")]
        public static XDocument SerializeToXDoc<T>(this T source)
        {
            return Xml.Utilities.SerializeToXDoc(source);
        }
        [Obsolete("Use Extensions.Serialization.Xml version instead of this")]
        public static XmlDocument SerializeToXmlDoc<T>(this T source)
            where T : new()
        {
            return Xml.Utilities.SerializeToXmlDoc(source);
        }
        [Obsolete("Use Extensions.Serialization.Xml version instead of this")]
        public static T Deserialize<T>(this XDocument serialized)
        {
            return Xml.Utilities.Deserialize<T>(serialized);
        }
        [Obsolete("Use Extensions.Serialization.Xml version instead of this")]
        public static T Deserialize<T>(this XmlDocument serialized)
        {
            return Xml.Utilities.Deserialize<T>(serialized);
        }
        [Obsolete("Use Extensions.Serialization.Xml version instead of this")]
        public static XmlDocument ToXmlDocument(this XDocument xDocument)
        {
            return Xml.Utilities.ToXmlDocument(xDocument);
        }
        [Obsolete("Use Extensions.Serialization.Xml version instead of this")]
        public static XDocument ToXDocument(this XmlDocument xmlDocument)
        {
            return Xml.Utilities.ToXDocument(xmlDocument);
        }
        #endregion

         #region  Csv
        [Obsolete("Use Extensions.Serialization.Csv version instead of this")]
        public static string SerializeToCsv<T>(this IEnumerable<T> input, string separator = ",", char? quotation = null, CultureInfo info = null)
        {
            return Csv.Utilities.SerializeToCsv(input, separator, quotation, info);
        }
        [Obsolete("Use Extensions.Serialization.Csv version instead of this")]
        public static string SerializeToCsv<T>(this IEnumerable<T> input, ClassMap<T> propertiesMap, string separator = ",",
            char? quotation = null, CultureInfo info = null)
        {
            return Csv.Utilities.SerializeToCsv(input, propertiesMap, separator, quotation, info);
        }
        [Obsolete("Use Extensions.Serialization.Csv version instead of this")]
        public static IEnumerable<T> DeserializeFromCsv<T>(this string csvContents, ClassMap<T> propertiesMap = null, CultureInfo info = null)
        {
            return Csv.Utilities.DeserializeFromCsv(csvContents, propertiesMap, info);
        } 
        #endregion

        private static string ToCsvLine<T>(this IEnumerable<T> input, char separator = ',', char? quotation = null)
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

        private static string ToCsvLine<T>(this IEnumerable<T> input, CultureInfo info, string format = "G",
            char separator = ',', char? quotation = null) where T : IFormattable
        {
            if (input == null) return null;
            var csv = new StringBuilder();
            foreach (var value in input)
            {
                csv.AppendFormat("{0}{1}{2}{3}", quotation, value.ToString(format, info), quotation, separator);
            }
            if (csv.Length != 0)
                return csv.ToString(0, csv.Length - 1);
            return string.Empty;
        }

        /// <summary>
        /// Use in code generation to turn <paramref name="input"/> into declaration of array of <typeparam>T</typeparam>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string WriteToNewArray<T>(this IEnumerable<T> input)
        {
            var typeName = typeof(T).Name;
            var begining = $"{typeName}[] array = new {typeName}[] {{";
            const string end = "};";

            switch (typeName)
            {
                case "Char":
                case "char":
                    return $"{begining}{input.ToCsvLine(',', '\'')}{end}";
                case "String":
                case "string":
                    return $"{begining}{input.ToCsvLine(',', '\"')}{end}";
                default:
                    return $"{begining}{input.ToCsvLine()}{end}";
            }
        }

        /// <summary>
        /// Use in code generation to turn <paramref name="input"/> into declaration of array of <typeparam>T</typeparam>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="input"></param>
        /// <param name="info"></param>
        /// <param name="format"></param>
        /// <returns></returns>
        public static string WriteToNewArray<T>(this IEnumerable<T> input, CultureInfo info, string format = "G")
            where T : IFormattable
        {
            if (info == null) info = CultureInfo.CurrentCulture;

            var typeName = typeof(T).Name;
            var begining = $"{typeName}[] array = new {typeName}[] {{";
            const string end = "};";

            switch (typeName)
            {
                case "Char":
                case "char":
                    return $"{begining}{input.ToCsvLine(info, format, ',', '\'')}{end}";
                case "String":
                case "string":
                    return $"{begining}{input.ToCsvLine(info, format, ',', '\"')}{end}";
                default:
                    return $"{begining}{input.ToCsvLine(info, format)}{end}";
            }
        }
    }
}
