# StringExtractLib
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)

A .NET library to extract strings from different data sources.

### Features

+ Supports UTF8 and UTF16 string extraction.
+ Supports multiple data sources:
	* From file
	* From bytes
	* From memory (StringExtractLib.Memory - Windows only)

Easy to set up:
```csharp
var strings = new FileStringReader("C:/DummyFile.exe").ReadAll();
```

With custom extraction options:
```csharp
var options = new FileStringReaderOptions(
    minimumSize: 4,
    maximumSize: 12,
    stringType: StringType.Utf8,
    chunkSize: 8192);

var reader = new FileStringReader("C:/DummyFile.exe", options);
var strings = reader.ReadAll();
```
