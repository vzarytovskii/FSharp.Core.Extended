namespace FSharp.Extended.Benchmarks.Collections.Array.Max

open BenchmarkDotNet.Attributes
open FSharp.Extended.Benchmarks.Collections.Array.Utils


[<GenericTypeArguments(typeof<int>)>]
[<GenericTypeArguments(typeof<int64>)>]
[<GenericTypeArguments(typeof<double>)>]
[<BenchmarkCategory("Array", "Array.Max")>]
type ArrayMax<'T when 'T : comparison>() =
    inherit ArrayBenchmarkBase<'T>()

    [<Benchmark(Description="Array.max - FSharp.Core.Extended")>]
    member inline this.MaxExtended() =
        FSharp.Core.Extended.Collections.Array.max this.Array

    [<Benchmark(Description="Array.max - FSharp.Core", Baseline=true)>]
    member inline this.MaxDefault() =
        Microsoft.FSharp.Collections.Array.max this.Array
