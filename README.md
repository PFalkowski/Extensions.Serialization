Package for boilerplate serialization code, containing extensions like ToCsv, SerializeToXDoc, Deserialize. 

Usage:

```c#
var csv = someList.ToCsv();
var xdc = someList.SerializeToXDoc();
var obj = someXDoc.Deserialize<Persons>()
```
