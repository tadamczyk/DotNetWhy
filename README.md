| ![](https://raw.githubusercontent.com/tadamczyk/DotNetWhy/master/assets/logo/256/logo.png) |
|:--:|
| `dotnet why` - a .NET global tool to show information about why a NuGet package is installed. |
| ![GitHub](https://img.shields.io/github/license/tadamczyk/DotNetWhy) ![GitHub Workflow Status](https://img.shields.io/github/actions/workflow/status/tadamczyk/DotNetWhy/continuous-integration.yml?branch=master) ![GitHub Workflow Status](https://img.shields.io/github/actions/workflow/status/tadamczyk/DotNetWhy/release.yml?label=release) ![Nuget](https://img.shields.io/nuget/v/DotNetWhy?label=version) ![Nuget](https://img.shields.io/nuget/dt/DotNetWhy) ![GitHub issues](https://img.shields.io/github/issues/tadamczyk/DotNetWhy) ![GitHub pull requests](https://img.shields.io/github/issues-pr/tadamczyk/DotNetWhy) |

## Installation

Download and install the [.NET 6/8 SDK](https://www.microsoft.com/net/download).

Once installed, run the following command:

```bash
> dotnet tool install -g DotNetWhy
```

## Example

```bash
> dotnet why Newtonsoft.Json

Analyzing project(s) from C:\DotNetWhy directory...

ᐉ DotNetWhy                             [6]
ᐅ DotNet.CommandExecutor.Tests        [2/6]
ᐳ net7.0                              [2/2]
1.  Microsoft.NET.Test.Sdk (17.5.0) ▶ Microsoft.TestPlatform.TestHost (17.5.0) ▶
      Newtonsoft.Json (13.0.2)
2.  Newtonsoft.Json (13.0.2)

ᐅ DotNetWhy.Domain                    [2/6]
ᐳ net6.0                              [1/2]
1.  NuGet.ProjectModel (6.5.0) ▶ NuGet.DependencyResolver.Core (6.5.0) ▶
      NuGet.Protocol (6.5.0) ▶ NuGet.Packaging (6.5.0) ▶ Newtonsoft.Json (13.0.1)
ᐳ net7.0                              [1/2]
1.  NuGet.ProjectModel (6.5.0) ▶ NuGet.DependencyResolver.Core (6.5.0) ▶
      NuGet.Protocol (6.5.0) ▶ NuGet.Packaging (6.5.0) ▶ Newtonsoft.Json (13.0.1)

ᐅ DotNetWhy.Domain.Tests              [2/6]
ᐳ net7.0                              [2/2]
1.  Microsoft.NET.Test.Sdk (17.5.0) ▶ Microsoft.TestPlatform.TestHost (17.5.0) ▶
      Newtonsoft.Json (13.0.2)
2.  Newtonsoft.Json (13.0.2)

Time elapsed: 00:00:03.45
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