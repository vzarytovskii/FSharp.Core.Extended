namespace FSharp.Faster.Benchmarks.Collections.Array.Max

open BenchmarkDotNet.Attributes
open FSharp.Faster.Benchmarks.Collections.Array.Utils


[<GenericTypeArguments(typeof<int>)>]
[<GenericTypeArguments(typeof<int64>)>]
[<GenericTypeArguments(typeof<double>)>]
[<BenchmarkCategory("Array", "Array.Max")>]
type ArrayMax<'T when 'T : comparison>() =
    inherit ArrayBenchmarkBase<'T>()

    [<Benchmark(Description="Array.max - FSharp.Core.Faster")>]
    member inline this.MaxFaster() =
        FSharp.Core.Faster.Collections.Array.Array.max this.Array

    [<Benchmark(Description="Array.max - FSharp.Core")>]
    member inline this.MaxDefault() =
        Microsoft.FSharp.Collections.Array.max this.Array
