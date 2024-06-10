namespace FSharp.Extended.Benchmarks.Collections.Array.Create

open BenchmarkDotNet.Attributes
open FSharp.Core.Extended.Benchmarks.Utils

open System

[<GenericTypeArguments(typeof<int>)>]
[<GenericTypeArguments(typeof<int64>)>]
[<GenericTypeArguments(typeof<double>)>]
[<BenchmarkCategory("Array", "Array.create")>]
type ArrayCreate<'T when 'T : comparison>() =

    [<Params(1, 10, 100, 1_000, 10_000, 100_000, 1_000_000)>]
    member val Count = 0 with get, set
    member val Value = retype<int, 'T> 1_000

    [<Benchmark(Description="Array.create - FSharp.Core.Extended")>]
    member inline this.CreateFaster() =
        FSharp.Core.Extended.Collections.Array.create this.Count this.Value

    [<Benchmark(Description="Array.create - FSharp.Core")>]
    member inline this.CreateDefault() =
        Microsoft.FSharp.Collections.Array.create this.Count this.Value
