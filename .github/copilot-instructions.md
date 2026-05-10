# Argus Framework – Copilot Instructions

## Build & Test

Build the entire solution:
```
dotnet build ArgusFramework.sln
```

Run all unit tests:
```
dotnet test tests/Argus.UnitTests/Argus.UnitTests.csproj
```

Run a single test by name:
```
dotnet test tests/Argus.UnitTests/Argus.UnitTests.csproj --filter "FullyQualifiedName~StringTests.Left"
```

Run benchmarks (separate project, not part of the test suite):
```
dotnet run --project tests/BenchmarkConsoleProject/BenchmarkConsoleProject/BenchmarkConsoleProject.csproj -c Release
```

## Architecture

This is a multi-project .NET framework library suite. **`Argus.Core`** is the foundation that all other projects depend on. Each satellite project wraps a specific technology domain:

| Project | Domain |
|---|---|
| `Argus.Core` | Foundation library; multi-targeted (netstandard2.0/2.1, net8/9/10) |
| `Argus.AspNetCore` | ASP.NET Core shared components |
| `Argus.Audio.NAudio` | Extensions on top of NAudio |
| `Argus.Graphics` | Imaging via System.Drawing.Common |
| `Argus.Office` | Office file formats via OpenXml SDK |
| `Argus.Windows` | Windows-only OS utilities |
| `Argus.Windows.Forms` | WinForms controls and utilities |

Satellite projects embed `Argus.Core` into their NuGet output (`PrivateAssets="all"` + `CopyProjectReferencesToPackage` target), so each package ships as self-contained.

The `tests/` folder contains: `Argus.UnitTests` (xunit), `BenchmarkConsoleProject` (BenchmarkDotNet), and throwaway test apps (`WinFormsTestApp`, `WpfAppTestApp`).

## Key Conventions

### Namespace for all extension methods
All extension methods, regardless of which project or subfolder they live in, are placed in the **`Argus.Extensions`** namespace. Never use a sub-namespace like `Argus.Extensions.String`.

### File header
Every `.cs` source file begins with this block comment:
```csharp
/*
 * @author            : Blake Pell
 * @website           : http://www.blakepell.com
 * @initial date      : YYYY-MM-DD
 * @last updated      : YYYY-MM-DD
 * @copyright         : Copyright (c) 2003-YYYY, All rights reserved.
 * @license           : MIT
 */
```

### Version format
Package versions follow `YYYY.MM.D.N` (e.g., `2025.12.7.1`). All three version properties (`Version`, `AssemblyVersion`, `FileVersion`) are set in the `.csproj`.

### XML documentation
`Argus.Core` generates XML docs (`<GenerateDocumentationFile>true</GenerateDocumentationFile>`). Every public type and member must have `<summary>` XML doc comments. Param tags are also expected.

### Nullable
`<Nullable>enable</Nullable>` is set in `Argus.Core` and `Argus.AspNetCore`. Use nullable reference types and annotate accordingly.

### Global usings
`Argus.Core` declares common BCL namespaces in `GlobalUsings.cs` (System, System.Collections.Generic, System.IO, System.Linq, System.Text, etc.). Do not re-add these in individual files.

### Zero-allocation strings
`Cysharp.Text.ZString` (imported as `using Cysharp.Text;`) is the preferred approach for high-performance string building. Use `ZString` instead of `StringBuilder` in hot paths.

### DI / Service locator
`Argus.Memory.AppServices` is the framework's static DI wrapper. It is used in non-ASP.NET contexts where `IServiceProvider` cannot be injected. Use `AppServices.GetRequiredService<T>()` / `AppServices.GetService<T>()` rather than constructing instances manually.

### Test structure
Unit tests in `Argus.UnitTests` mirror the source layout: one `*Tests.cs` file per source `*Extensions.cs` or utility class. Tests use **xunit** (`[Fact]` / `Assert.*`). Both xunit and MSTest packages are referenced, but xunit is the primary framework in use.

### Package generation
All library projects set `<GeneratePackageOnBuild>True</GeneratePackageOnBuild>`, so NuGet packages are produced on every build. Expect `.nupkg` files in `bin/` after building.
