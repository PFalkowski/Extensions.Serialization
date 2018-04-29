# Extensions.Serialization

[![NuGet version (Extensions.Serialization)](https://img.shields.io/nuget/v/Extensions.Serialization.svg)](https://www.nuget.org/packages/Extensions.Serialization/)
[![Licence (Extensions.Serialization)](https://img.shields.io/github/license/mashape/apistatus.svg)](https://choosealicense.com/licenses/mit/)
[![Licence (Extensions.Serialization)](https://img.shields.io/badge/licence-Apache--2.0-green.svg)](https://choosealicense.com/licenses/apache-2.0/)
[![Licence (Extensions.Serialization)](https://img.shields.io/badge/licence-MS--PL-blue.svg)](https://opensource.org/licenses/MS-PL)

Package for boilerplate serialization code, containing extensions like ToCsv, SerializeToXDoc, Deserialize. 

Usage:

```c#
string csv = anything.SerializeToCsv();
XDocument xDoc = anything.SerializeToXDoc();
XmlDocument xmlDoc = anything.SerializeToXmlDoc();

Persons persons = csv.DeserializeFromCsv<Persons>();
Persons persons = xDoc.Deserialize<Persons>();
Persons persons = xmlDoc.Deserialize<Persons>();
```
