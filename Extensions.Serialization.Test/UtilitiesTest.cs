using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Xml.Linq;
using CsvHelper.Configuration;
using Xunit;

namespace Extensions.Serialization.Test
{
    ///TODO: The Turkey Test http://www.moserware.com/2008/02/does-your-code-pass-turkey-test.html
    public class UtilitiesTest
    {
        [Fact]
        public void SerializeToXDocTest()
        {
            var tested = PersonsList.SerializeToXDoc();

            var firstNames = new HashSet<string>(from e in tested.Descendants("FirstName") select e.Value);
            var ages = new HashSet<int>(from e in tested.Descendants("Age") select int.Parse(e.Value));

            Assert.Equal(5, firstNames.Count);
            Assert.Equal(5, ages.Count);
            Assert.Contains("Alex", firstNames);
            Assert.Contains("Cloe", firstNames);
            Assert.Contains("Jack", firstNames);
            Assert.Contains("John", firstNames);
            Assert.Contains("Grace", firstNames);
            Assert.Contains(27, ages);
            Assert.Contains(35, ages);
            Assert.Contains(45, ages);
            Assert.Contains(30, ages);
        }

        [Fact]
        public void DeserializeXDoc()
        {
            var input = XDocument.Parse(Properties.Resources.ArrayOfPerson);
            var received = input.Deserialize<List<Person>>();

            Assert.Equal(4, received.Count);
            Assert.Equal(27, received[0].Age);
            Assert.Equal(45, received[1].Age);
            Assert.Equal(35, received[2].Age);
            Assert.Equal(30, received[3].Age);
        }
        
        [Fact]
        public void ToCsvTest2()
        {
            var values = new[] { 1.123, 2.123, 3.234, 4.532, 5.723 };
            var csv = values.SerializeToCsv();
            const string expected = "\"1,123\"\r\n\"2,123\"\r\n\"3,234\"\r\n\"4,532\"\r\n\"5,723\"\r\n";
            Assert.Equal(expected, csv.ToString());
        }
        [Fact]
        public void WriteToNewArray0()
        {
            int[] ints = null;
            var emptyDeclaration = ints.WriteToNewArray();
        }

        [Fact]
        public void WriteToNewArray1()
        {
            string s;
            var ints = new int[0];
            s = ints.WriteToNewArray();
            Assert.Equal("Int32[] array = new Int32[] {};", s);
        }

        [Fact]
        public void WriteToNewArray2()
        {
            string s;
            var ints = new int[1];
            s = ints.WriteToNewArray();
            Assert.Equal("Int32[] array = new Int32[] {0};", s);
        }

        [Fact]
        public void WriteToNewArray3()
        {
            string s;
            var ints = new[] { 1, 2, 3 };
            s = ints.WriteToNewArray();
            Assert.Equal("Int32[] array = new Int32[] {1,2,3};", s);
        }

        [Fact]
        public void WriteToNewArray4()
        {
            string s;
            var arr = new[] { "abc", "d", "" };
            s = arr.WriteToNewArray();
            Assert.Equal("String[] array = new String[] {\"abc\",\"d\",\"\"};", s);
        }

        [Fact]
        public void WriteToNewArray5()
        {
            string s;
            var arr = new[] { 'a', 'd', '\\' };
            s = arr.WriteToNewArray();
            Assert.Equal("Char[] array = new Char[] {\'a\',\'d\',\'\\\'};", s);
        }


        [Fact]
        public void FromCsvDeserializesProperly()
        {
            var tested = @"""FirstName"",""LastName"",""Age""
""Grace"",""Hopper"",""10""";
            //var tested = "\"Grace\",\"Hopper\", \"10\"";
            var deserialized = tested.DeserializeFromCsv<Person>();
        }

        [Fact]
        public void ToCsvDSrializesProperlyWithDefaults()
        {
            var tested = PersonsList;
            var serialized = tested.SerializeToCsv();

            Assert.Equal("FirstName,LastName,Age\r\nAlex,Friedman,27\r\nJack,Bauer,45\r\nCloe,O'Brien,35\r\nJohn,Doe,30\r\nGrace,Hooper,111\r\n", serialized.ToString());
        }
        [Fact]
        public void ToCsvDSrializesProperlyWithCustomSeparator()
        {
            var tested = PersonsList;
            var serialized = tested.SerializeToCsv("*");

            Assert.Equal("FirstName*LastName*Age\r\nAlex*Friedman*27\r\nJack*Bauer*45\r\nCloe*O'Brien*35\r\nJohn*Doe*30\r\nGrace*Hooper*111\r\n", serialized.ToString());
        }
        [Fact]
        public void ToCsvDeSrializesProperlyWithCustomQuotation()
        {
            var tested = PersonsList;
            var serialized = tested.SerializeToCsv(",", '"');

            Assert.Equal("\"FirstName\",\"LastName\",\"Age\"\r\n\"Alex\",\"Friedman\",\"27\"\r\n\"Jack\",\"Bauer\",\"45\"\r\n\"Cloe\",\"O'Brien\",\"35\"\r\n\"John\",\"Doe\",\"30\"\r\n\"Grace\",\"Hooper\",\"111\"\r\n", serialized.ToString());
        }

        private class PersonMaping : ClassMap<Person>
        {
            public PersonMaping()
            {
                Map(m => m.FirstName).Name("forename");
                Map(m => m.LastName).Name("surname"); ;
                Map(m => m.Age).Name("age");
            }
        }

        [Fact]
        public void ToCsvSerializesProperlyWithCustomMapping()
        {
            var tested = PersonsList;
            var serialized = tested.SerializeToCsv(new PersonMaping(), ",", '"');

            Assert.Equal("\"forename\",\"surname\",\"age\"\r\n\"Alex\",\"Friedman\",\"27\"\r\n\"Jack\",\"Bauer\",\"45\"\r\n\"Cloe\",\"O'Brien\",\"35\"\r\n\"John\",\"Doe\",\"30\"\r\n\"Grace\",\"Hooper\",\"111\"\r\n", serialized.ToString());
        }
        [Fact]
        public void ToCsvDeserializesProperlyWithCustomMapping()
        {
            var tested =
                "\"forename\",\"surname\",\"age\"\r\n\"Alex\",\"Friedman\",\"27\"\r\n\"Jack\",\"Bauer\",\"45\"\r\n\"Cloe\",\"O'Brien\",\"35\"\r\n\"John\",\"Doe\",\"30\"\r\n\"Grace\",\"Hooper\",\"111\"\r\n";
            var serialized = tested.DeserializeFromCsv(new PersonMaping()).ToList();

            Assert.Equal("Alex", serialized[0].FirstName);
            Assert.Equal("Friedman", serialized[0].LastName);
            Assert.Equal(27, serialized[0].Age);
            Assert.Equal("Jack", serialized[1].FirstName);
            Assert.Equal("Bauer", serialized[1].LastName);
            Assert.Equal(45, serialized[1].Age);
            Assert.Equal("Cloe", serialized[2].FirstName);
            Assert.Equal("O'Brien", serialized[2].LastName);
            Assert.Equal(35, serialized[2].Age);
            Assert.Equal("John", serialized[3].FirstName);
            Assert.Equal("Doe", serialized[3].LastName);
            Assert.Equal(30, serialized[3].Age);
            Assert.Equal("Grace", serialized[4].FirstName);
            Assert.Equal("Hooper", serialized[4].LastName);
            Assert.Equal(111, serialized[4].Age);

        }
        #region Mocks

        public sealed class Person
        {
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public int Age { get; set; }
        }


        private List<Person> PersonsList { get; } = new List<Person>
        {
            new Person
            {
                FirstName = "Alex",
                LastName = "Friedman",
                Age = 27
            },
            new Person
            {
                FirstName = "Jack",
                LastName = "Bauer",
                Age = 45
            },
            new Person
            {
                FirstName = "Cloe",
                LastName = "O'Brien",
                Age = 35
            },
            new Person
            {
                FirstName = "John",
                LastName = "Doe",
                Age = 30
            },
            new Person
            {
                FirstName = "Grace",
                LastName = "Hooper",
                Age = (int) (DateTime.Now - new DateTime(1906, 12, 9)).TotalDays / 365
            }
        };

        #endregion
    }
}
