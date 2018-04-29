# Extensions.Serialization

[![NuGet version (Extensions.Serialization)](https://img.shields.io/nuget/v/Extensions.Serialization.svg)](https://www.nuget.org/packages/Extensions.Serialization/)
[![Licence (Extensions.Serialization)](https://img.shields.io/github/license/mashape/apistatus.svg)](https://choosealicense.com/licenses/mit/)

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
