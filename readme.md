# SyncAPIConnect - Xtb xApi client  

[![Build](https://github.com/jirikostiha/xtb-xApi/actions/workflows/build.yml/badge.svg)](https://github.com/jirikostiha/xtb-xApi/actions/workflows/build.yml)
[![Code Lint](https://github.com/jirikostiha/xtb-xApi/actions/workflows/lint-code.yml/badge.svg)](https://github.com/jirikostiha/xtb-xApi/actions/workflows/lint-code.yml)

This project is fork of [.Net xApi wrapper](http://developers.xstore.pro/api/wrappers.html) with some improvements to make life easier.  

## Setup

Add [nuget package](https://www.nuget.org/packages/SyncAPIConnect) to the project.  
For example like this:
```xml
<Project Sdk="Microsoft.NET.Sdk">
  <PropertyGroup>
    <TargetFramework>net8</TargetFramework>
  </PropertyGroup>
  <ItemGroup>
    <PackageReference Include="SyncAPIConnect" Version="2.5.2" />
  </ItemGroup>
</Project>
```

## Usage

For usage see [example code](./src/xAPITest/Program.cs), [official page](http://developers.xstore.pro/) and [official documentation](http://developers.xstore.pro/documentation/)
