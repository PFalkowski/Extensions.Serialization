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
