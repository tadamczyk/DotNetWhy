> **This project is no longer maintained.**
>
> The functionality provided by this tool has been superseded by the native `dotnet nuget why` command introduced in the .NET CLI.
> We recommend using the official command for future development.
> For more information, please refer to the [official documentation](https://learn.microsoft.com/en-us/dotnet/core/tools/dotnet-nuget-why).

----

| ![](https://raw.githubusercontent.com/tadamczyk/DotNetWhy/master/assets/logo/256/logo.png) |
|:--:|
| `dotnet why` - a .NET global tool to show information about why a NuGet package is installed |
| ![Nuget](https://img.shields.io/nuget/v/DotNetWhy?label=version) ![GitHub](https://img.shields.io/github/license/tadamczyk/DotNetWhy) ![Nuget](https://img.shields.io/nuget/dt/DotNetWhy) ![GitHub Workflow Status](https://img.shields.io/github/actions/workflow/status/tadamczyk/DotNetWhy/build.yml?branch=master) ![GitHub Workflow Status](https://img.shields.io/github/actions/workflow/status/tadamczyk/DotNetWhy/release.yml?label=release) ![GitHub issues](https://img.shields.io/github/issues/tadamczyk/DotNetWhy) ![GitHub pull requests](https://img.shields.io/github/issues-pr/tadamczyk/DotNetWhy) |

## Installation

Download and install the [.NET 6/8 SDK](https://www.microsoft.com/net/download).

Once installed, run the following command:

```bash
> dotnet tool install -g DotNetWhy
```

## Example

```bash
> dotnet why Newtonsoft.Json

Why...?
* is the *Newtonsoft.Json* package
* in the C:\DotNetWhy directory

Answer:
*   DotNetWhy                  [2]
**  DotNetWhy.Core           [2/2]
*** net6.0                   [1/2]
1.  NuGet.ProjectModel (6.7.0) > NuGet.DependencyResolver.Core (6.7.0) > NuGet.Protocol (6.7.0) >
      NuGet.Packaging (6.7.0) > Newtonsoft.Json (13.0.1)
*** net8.0                   [1/2]
1.  NuGet.ProjectModel (6.7.0) > NuGet.DependencyResolver.Core (6.7.0) > NuGet.Protocol (6.7.0) >
      NuGet.Packaging (6.7.0) > Newtonsoft.Json (13.0.1)

Time elapsed: 00:00:03.01
```

## Usage

The mandatory query argument for `dotnet why` command is package name:

```bash
> dotnet why Newtonsoft.Json
```

At this moment, the only additional optional query argument for `dotnet why` command is `--version|-v` option:
```bash
> dotnet why Newtonsoft.Json --version 13.0.1
> dotnet why Newtonsoft.Json -v 13.0.2
```