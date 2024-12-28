<p align="center">
  <img src="\src\XApiClient\package_icon.png " alt="Xtb.XApiClient" width="50"/>
</p>

# xtb xApiClient  

![GitHub repo size](https://img.shields.io/github/repo-size/jirikostiha/xtb-xApi)
![GitHub code size](https://img.shields.io/github/languages/code-size/jirikostiha/xtb-xApi)
![Nuget](https://img.shields.io/nuget/dt/Xtb.XApiClient)  
[![Build](https://github.com/jirikostiha/xtb-xApi/actions/workflows/build.yml/badge.svg)](https://github.com/jirikostiha/xtb-xApi/actions/workflows/build.yml)
[![Code Lint](https://github.com/jirikostiha/xtb-xApi/actions/workflows/lint-code.yml/badge.svg)](https://github.com/jirikostiha/xtb-xApi/actions/workflows/lint-code.yml)

This project is fork of [.Net xApi wrapper](http://developers.xstore.pro/api/wrappers.html) with some improvements to make life easier.
It is based on xApi version 2.5.0.  
In the beginning there were mostly additive changes with some necessary exceptions and now there are many improvements and changes in original code.  

SyncAPIConnect

## Setup

Add [nuget package](https://www.nuget.org/packages/Xtb.XApiClient) to the project.  
```xml
<Project Sdk="Microsoft.NET.Sdk">
  <PropertyGroup>
    <TargetFramework>net8</TargetFramework>
  </PropertyGroup>
  <ItemGroup>
    <PackageReference Include="Xtb.XApiClient" Version="2.5.X" />
  </ItemGroup>
</Project>
```

## Usage

```csharp
var client = XClient.Create("81.2.190.163", 5112, 5113);
await client.ConnectAsync();
await client.LoginAsync(new Credentials("accountId", "password"));
var openTrades = await client.GetTradesAsync(true);
```

For usage see [example code](./src/SystemTests/Program.cs ), [official page](http://developers.xstore.pro/) and [api documentation](http://developers.xstore.pro/documentation/)


## Changelog

**SyncAPIConnect** package renamed to **Xtb.XApiClient**  
2.5.22 separated records from responses, pretty print fixes, minor changes  
2.5.21 more consistent names, performance improvements  
2.5.20 various refactoring, improvements  
2.5.19 various refactoring, minor fixes and changes, unit tests  
2.5.18 XApiClient as main api providing class  
2.5.17 time arguments instead of long, reduced memory footprint (long->int)  
2.5.16 async cancellation, time members, various small changes  
2.5.15 async streaming subscriptions, async IStreamingListener, various simplifications  
2.5.14 replaced Newtonsoft.Json by System.Text.Json (pkamphuis)  
2.5.13 support custom handling of exceptions in streaming connector, enriched exceptions  
2.5.12 more interfaces of records, names of new members unification, various small changes  
2.5.11 fixed performance warnings  
2.5.10 cfd stock indication, various changes on records and codes  
2.5.9 string constants, fix hours interval evaluation  
2.5.8 async methods  
2.5.7 time conversion to DateTimeOffset, time interval check, long short position extension  
2.5.6 records changed from classes to C# records  
2.5.5 common interfaces for streaming and non-streaming records, code constants  
2.5.4 codes to friendly string extensions  
2.5.3 timeout handling, trading examples  
2.5.2 new csproj format, set netstandard2.0  
2.5.1 cleaned linked binaries and set nuget dependencies


## License

Project is under [MIT](./LICENSE) license.
