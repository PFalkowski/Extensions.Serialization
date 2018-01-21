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
            Assert.True(firstNames.Contains("Alex"));
            Assert.True(firstNames.Contains("Cloe"));
            Assert.True(firstNames.Contains("Jack"));
            Assert.True(firstNames.Contains("John"));
            Assert.True(firstNames.Contains("Grace"));
            Assert.True(ages.Contains(27));
            Assert.True(ages.Contains(35));
            Assert.True(ages.Contains(45));
            Assert.True(ages.Contains(30));
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
            var plCulture = new CultureInfo("pl-PL");
            var csv = values.SerializeToCsv();
            var expected = "\"1,123\"\r\n\"2,123\"\r\n\"3,234\"\r\n\"4,532\"\r\n\"5,723\"\r\n";
            Assert.Equal(expected, csv.ToString());
        }
        [Fact]
        public void WriteToNewArray0()
        {
            int[] ints = null;
            var emptyDeclaration = ints.WriteToNewArray<int>();
        }

        [Fact]
        public void WriteToNewArray1()
        {
            string s;
            var ints = new int[0];
            s = ((IEnumerable<int>)ints).WriteToNewArray<int>();
            Assert.Equal<string>("Int32[] array = new Int32[] {};", s);
        }

        [Fact]
        public void WriteToNewArray2()
        {
            string s;
            var ints = new int[1];
            s = ((IEnumerable<int>)ints).WriteToNewArray<int>();
            Assert.Equal<string>("Int32[] array = new Int32[] {0};", s);
        }

        [Fact]
        public void WriteToNewArray3()
        {
            string s;
            var ints = new[] { 1, 2, 3 };
            s = ((IEnumerable<int>)ints).WriteToNewArray<int>();
            Assert.Equal<string>("Int32[] array = new Int32[] {1,2,3};", s);
        }

        [Fact]
        public void WriteToNewArray4()
        {
            string s;
            var arr = new[] { "abc", "d", "" };
            s = arr.WriteToNewArray();
            Assert.Equal<string>("String[] array = new String[] {\"abc\",\"d\",\"\"};", s);
        }

        [Fact]
        public void WriteToNewArray5()
        {
            string s;
            var arr = new[] { 'a', 'd', '\\' };
            s = arr.WriteToNewArray();
            Assert.Equal<string>("Char[] array = new Char[] {\'a\',\'d\',\'\\\'};", s);
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

            Assert.Equal(serialized[0].FirstName, "Alex");
            Assert.Equal(serialized[0].LastName, "Friedman");
            Assert.Equal(serialized[0].Age, 27);
            Assert.Equal(serialized[1].FirstName, "Jack");
            Assert.Equal(serialized[1].LastName, "Bauer");
            Assert.Equal(serialized[1].Age, 45);
            Assert.Equal(serialized[2].FirstName, "Cloe");
            Assert.Equal(serialized[2].LastName, "O'Brien");
            Assert.Equal(serialized[2].Age, 35);
            Assert.Equal(serialized[3].FirstName, "John");
            Assert.Equal(serialized[3].LastName, "Doe");
            Assert.Equal(serialized[3].Age, 30);
            Assert.Equal(serialized[4].FirstName, "Grace");
            Assert.Equal(serialized[4].LastName, "Hooper");
            Assert.Equal(serialized[4].Age, 111);

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
