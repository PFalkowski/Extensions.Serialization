﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
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

        public static string SerializeToCsv<T>(this IEnumerable<T> input, string separator = ",", char? quotation = null, CultureInfo info = null)
        {
            return SerializeToCsv(input, null, separator, quotation, info);
        }

        public static string SerializeToCsv<T>(this IEnumerable<T> input, ClassMap<T> propertiesMap, string separator = ",",
            char? quotation = null, CultureInfo info = null)
        {
            if (input == null) return null;
            var stb = new StringBuilder();
            using (var writer = new CsvWriter(new StringWriter(stb), false))
            {
                writer.Configuration.Delimiter = separator;
                writer.Configuration.SanitizeForInjection = true;
                if (quotation.HasValue)
                {
                    writer.Configuration.QuoteAllFields = true;
                    writer.Configuration.Quote = quotation.Value;
                }
                else
                {
                    writer.Configuration.QuoteAllFields = false;
                }
                if (info != null)
                {
                    writer.Configuration.CultureInfo = info;
                }

                if (propertiesMap != null)
                {
                    writer.Configuration.RegisterClassMap(propertiesMap);
                }
                writer.WriteRecords(input);
                writer.Flush();
            }
            return stb.ToString();
        }

        public static IEnumerable<T> DeserializeFromCsv<T>(this string csvContents, ClassMap<T> propertiesMap = null, CultureInfo info = null)
        {
            IEnumerable<T> result;
            using (var csv = new CsvReader(new StringReader(csvContents), false))
            {
                if (propertiesMap != null) { csv.Configuration.RegisterClassMap(propertiesMap); }
                if (info != null) { csv.Configuration.CultureInfo = info; }
                result = csv.GetRecords<T>().ToList();
            }
            return result;
        }


        public static XDocument SerializeToXDoc<T>(this T source)
        {
            var result = new XDocument();
            using (var writer = result.CreateWriter())
            {
                var serializer = new XmlSerializer(source.GetType());
                serializer.Serialize(writer, source);
            }
            return result;
        }

        public static XmlDocument SerializeToXmlDoc<T>(this T source)
            where T : new()
        {
            var result = new XmlDocument();
            using (var ms = new MemoryStream())
            {
                var serializer = new XmlSerializer(source.GetType());
                serializer.Serialize(ms, source);
                ms.Flush();
                ms.Position = 0;
                result.Load(ms);
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

        public static T Deserialize<T>(this XmlDocument serialized)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));
            using (MemoryStream xmlStream = new MemoryStream())
            {
                serialized.Save(xmlStream);
                xmlStream.Flush();
                xmlStream.Position = 0;
                using (TextReader reader = new StreamReader(xmlStream))
                {
                    return (T)xmlSerializer.Deserialize(reader);
                }
            }
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
