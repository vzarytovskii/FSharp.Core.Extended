[![NuGet version (FSharp.Core.Extended)](https://img.shields.io/nuget/v/FSharp.Core.Extended.svg?style=flat-square)](https://www.nuget.org/packages/FSharp.Core.Extended/)

# What

> [!WARNING]
> This library is very much work in progress, expect issues.

This library tries to be a drop-in replacement for the `FSharp.Core` with functions, which are generally faster and more flexible than built-in ones, but may be backwards-incompatible at runtime/compile-time.

## Examples of current (and future planned) backwards-incompatibilities:
- Some collections functions (`min`, `max`, `sum`, etc) can handle things like `NaN` differently than `FSharp.Core`.
- All `try*` functions return `ValueOption<'T>`.
- `Option<'T>` is aliasing `ValueOption<'T>`, all `Option` module functions shadowing the ones from `FSharp.Core`, several helper functions/methods provided to convert back and from the `FSharp.Core.Option<'T>`.
- `...`

## Hot to use:
This library being a `drop-in` replacement doesn't mean that just referencing its NuGet package is enough. Shadowing is achieved by opening this library's namespace (works on any granularity), for example:

```fsharp
open FSharp.Core.Extended
```

will shadow every module and type defined in this library (e.g. `Option` type, `Option` module, `Array` module, `List` module, etc)


```fsharp
open FSharp.Core.Extended.Collections
```

will shadow every module and type defined for collections


```fsharp
open FSharp.Core.Extended.Collections.Array
```

will shadow every type and module defined for arrays


And so on.
