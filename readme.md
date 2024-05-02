# SyncAPIConnect - Xtb xApi client  

![GitHub repo size](https://img.shields.io/github/repo-size/jirikostiha/xtb-xApi)
![GitHub code size](https://img.shields.io/github/languages/code-size/jirikostiha/xtb-xApi)
![Nuget](https://img.shields.io/nuget/dt/SyncAPIConnect)  
[![Build](https://github.com/jirikostiha/xtb-xApi/actions/workflows/build.yml/badge.svg)](https://github.com/jirikostiha/xtb-xApi/actions/workflows/build.yml)
[![Code Lint](https://github.com/jirikostiha/xtb-xApi/actions/workflows/lint-code.yml/badge.svg)](https://github.com/jirikostiha/xtb-xApi/actions/workflows/lint-code.yml)

This project is fork of [.Net xApi wrapper](http://developers.xstore.pro/api/wrappers.html) with some improvements to make life easier.  
The changes made are mostly additive except for a few necessary exceptions. It is based on xApi version 2.5.0.  

## Changelog

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
    <PackageReference Include="SyncAPIConnect" Version="2.5.5" />
  </ItemGroup>
</Project>
```

## Usage

For usage see [example code](./src/xAPITest/Program.cs), [official page](http://developers.xstore.pro/) and [official documentation](http://developers.xstore.pro/documentation/)
