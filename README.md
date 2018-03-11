Package for boilerplate serialization code, containing extensions like ToCsv, SerializeToXDoc, Deserialize. 

Usage:

```c#
string csv = anything.SerializeToCsv();
XDocument xDoc = anything.SerializeToXDoc();
XmlDocument xmlDoc = anything.SerializeToXmlDoc();

Persons person = csv.DeserializeFromCsv<Persons>();
Persons person = xDoc.Deserialize<Persons>();
Persons person = xmlDoc.Deserialize<Persons>();
```
