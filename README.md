# NConfig
A simple, lightweight file serialization facade for reading and writing XML, Json, and ini files to and from plain-old csharp objects to easily use strong-typed results.

This project shares similarities to the [Microsoft ASP.NET Core project](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/configuration/). Unlike that project, this one is geared towards using POCOs and doesn't jump through extra hoops to make everthing behave like appconfig style settings.

## NConfig.Abstractions
Separates the common interfaces in order to minimize dependencies.

## NConfig.Xml
Xml is built into certain .NET frameworks. This is a very thin facade.

## NConfig.Ini
Uses the [ini-parser library](https://github.com/rickyah/ini-parser) to do the basic ini file legwork. This project adds and wraps serialization and deserialization.

[MadMilkman.Ini](https://github.com/MarioZ/MadMilkman.Ini) also seems like a decent project, but ini-parser was the most popular.

## NConfig.Json
Uses the [Newtonsoft.Json library](https://github.com/JamesNK/Newtonsoft.Json) to the do Json file serialization.

# Examples
### INI
Working on implicit sections. Currently, sections must be specified with the IniSectionAttribute. It can be applied to either the Property or the Class definiton. For the love of all that is good, pick one and stick with it!
```chsarp
public class ExampleConfig
{
    public int ExampleGlobalInt { get; set; }
    public Section1 Section1 { get; set; }
    [IniSection("Section Two")]
    public Section2 Section2 { get; set; }
}
[IniSection("Section One")]
public class Section1
{
    public int Integer { get; set; }
    public bool Boolean { get; set; }
}
  
public class Section2
{
    public double Double { get; set; }
    public string String { get; set; }
}
```
