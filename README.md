# Extensions.Serialization

[![CI](https://github.com/PFalkowski/Extensions.Serialization/actions/workflows/ci.yml/badge.svg)](https://github.com/PFalkowski/Extensions.Serialization/actions/workflows/ci.yml)
[![NuGet version](https://img.shields.io/nuget/v/Extensions.Serialization.svg)](https://www.nuget.org/packages/Extensions.Serialization/)
[![NuGet downloads](https://img.shields.io/nuget/dt/Extensions.Serialization.svg)](https://www.nuget.org/packages/Extensions.Serialization/)
[![Quality Gate Status](https://sonarcloud.io/api/project_badges/measure?project=PFalkowski_Extensions.Serialization&metric=alert_status)](https://sonarcloud.io/summary/new_code?id=PFalkowski_Extensions.Serialization)
[![Coverage](https://sonarcloud.io/api/project_badges/measure?project=PFalkowski_Extensions.Serialization&metric=coverage)](https://sonarcloud.io/summary/new_code?id=PFalkowski_Extensions.Serialization)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://choosealicense.com/licenses/mit/)
[![Buy Me a Coffee](https://img.shields.io/badge/Buy%20Me%20a%20Coffee-support-yellow.svg)](https://www.buymeacoffee.com/piotrfalkowski)

> **Note:** This is a facade package. For new projects prefer installing the focused packages directly:
> - [`Extensions.Serialization.Xml`](https://www.nuget.org/packages/Extensions.Serialization.Xml/) — XML / XDocument / XmlDocument helpers
> - [`Extensions.Serialization.Csv`](https://www.nuget.org/packages/Extensions.Serialization.Csv/) — CSV helpers via CsvHelper

All methods in this package are marked `[Obsolete]` and delegate to the above packages.

## Usage

```csharp
// XML
XDocument xDoc = anything.SerializeToXDoc();
XmlDocument xmlDoc = anything.SerializeToXmlDoc();
List<Person> persons = xDoc.Deserialize<List<Person>>();

// CSV
string csv = list.SerializeToCsv();
IEnumerable<Person> people = csv.DeserializeFromCsv<Person>();

// Code generation helper
string decl = new[] { 1, 2, 3 }.WriteToNewArray();
// → "Int32[] array = new Int32[] {1,2,3};"
```
