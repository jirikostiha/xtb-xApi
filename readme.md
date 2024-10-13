<p align="center">
  <img src="\src\SyncAPIConnector\package_icon.png " alt="SyncAPIConnect" width="50"/>
</p>

# SyncAPIConnect - Xtb xApi client  

![GitHub repo size](https://img.shields.io/github/repo-size/jirikostiha/xtb-xApi)
![GitHub code size](https://img.shields.io/github/languages/code-size/jirikostiha/xtb-xApi)
![Nuget](https://img.shields.io/nuget/dt/SyncAPIConnect)  
[![Build](https://github.com/jirikostiha/xtb-xApi/actions/workflows/build.yml/badge.svg)](https://github.com/jirikostiha/xtb-xApi/actions/workflows/build.yml)
[![Code Lint](https://github.com/jirikostiha/xtb-xApi/actions/workflows/lint-code.yml/badge.svg)](https://github.com/jirikostiha/xtb-xApi/actions/workflows/lint-code.yml)

This project is fork of [.Net xApi wrapper](http://developers.xstore.pro/api/wrappers.html) with some improvements to make life easier.
It is based on xApi version 2.5.0.  
In the beginning there were mostly additive changes with some necessary exceptions and now there are many improvements and changes in original code.  

## Changelog

2.5.18 XApiClient as main api providing class  
2.5.17 time arguments instead of long, reduced memory footprint (long->int)  
2.5.16 async cancelation, time members, various small changes  
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

## Setup

Add [nuget package](https://www.nuget.org/packages/SyncAPIConnect) to the project.  
For example like this:
```xml
<Project Sdk="Microsoft.NET.Sdk">
  <PropertyGroup>
    <TargetFramework>net8</TargetFramework>
  </PropertyGroup>
  <ItemGroup>
    <PackageReference Include="SyncAPIConnect" Version="2.5.X" />
  </ItemGroup>
</Project>
```

## Usage

For usage see [example code](./src/SystemTests/Program.cs ), [official page](http://developers.xstore.pro/) and [official documentation](http://developers.xstore.pro/documentation/)
