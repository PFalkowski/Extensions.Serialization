using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Xml.Linq;
using Xunit;

namespace Extensions.Serialization.Test
{
    ///TODO: The Turkey Test http://www.moserware.com/2008/02/does-your-code-pass-turkey-test.html
    public class UtilitiesTest
    {
        [Fact]
        public void SerializeToXDocTest()
        {
            var tested = PersonList.SerializeToXDoc();

            var firstNames = new HashSet<string>(from e in tested.Descendants("FirstName") select e.Value);
            var ages = new HashSet<int>(from e in tested.Descendants("Age") select int.Parse(e.Value));

            Assert.Equal(4, firstNames.Count);
            Assert.Equal(4, ages.Count);
            Assert.True(firstNames.Contains("Alex"));
            Assert.True(firstNames.Contains("Cloe"));
            Assert.True(firstNames.Contains("Jack"));
            Assert.True(firstNames.Contains("John"));
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
        public void ToCsvNoCustomization()
        {
            var values = new[] { 1, 2, 3, 4, 5 };

            var csv = values.ToCsv();

            Assert.Equal("1,2,3,4,5", csv);
        }
        [Fact]
        public void ToCsvWithCustomSeparator()
        {
            var values = new[] { 1, 2, 3, 4, 5 };

            var csv = values.ToCsv(';', '\"');

            Assert.Equal("\"1\";\"2\";\"3\";\"4\";\"5\"", csv);
        }
        [Fact]
        public void ToCsvWithQutation()
        {
            var values = new[] { 1, 2, 3, 4, 5 };

            var csv = values.ToCsv(',', '\"');

            Assert.Equal("\"1\",\"2\",\"3\",\"4\",\"5\"", csv);
        }

        [Fact]
        public void ToCsvWithSeparatorAndQuotation()
        {
            var values = new[] { 1, 2, 3, 4, 5 };

            var csv = values.ToCsv(';', '\"');

            Assert.Equal("\"1\";\"2\";\"3\";\"4\";\"5\"", csv);
        }
        [Fact]
        public void ToCsvTestInvariantCulture()
        {
            var values = new[] { 1.123, 2.123, 3.234, 4.532, 5.723 };

            var csv = values.ToCsv(CultureInfo.InvariantCulture);
            var csv2 = values.ToCsv(CultureInfo.InvariantCulture, "G", ';', '\"');

            var expected1 = "1.123,2.123,3.234,4.532,5.723";
            var expected2 = @"""1.123"";""2.123"";""3.234"";""4.532"";""5.723""";


            Assert.Equal(expected1, csv);
            Assert.Equal(expected2, csv2);
        }

        [Fact]
        public void ToCsvTest2()
        {
            var values = new[] { 1.123, 2.123, 3.234, 4.532, 5.723 };
            var plCulture = new CultureInfo("pl-PL");
            var csv = values.ToCsv(plCulture);
            var csv2 = values.ToCsv(plCulture, "G", ';', '\"');
            var csv3 = values.ToCsv(plCulture, "F1", ';', '\"');
            var expected1 = "1,123,2,123,3,234,4,532,5,723";
            var expected2 = @"""1,123"";""2,123"";""3,234"";""4,532"";""5,723""";
            var expected3 = @"""1,1"";""2,1"";""3,2"";""4,5"";""5,7""";
            Assert.Equal(expected1, csv);
            Assert.Equal(expected2, csv2);
            Assert.Equal(expected3, csv3);
        }
        #region Mocks

        public sealed class Person
        {
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public int Age { get; set; }
        }


        private List<Person> PersonList => new List<Person>
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
            }
        };

        #endregion
    }
}
