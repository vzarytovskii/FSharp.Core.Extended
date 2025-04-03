namespace FSharp.Extended.Benchmarks.Collections.Array.Concat

open BenchmarkDotNet.Attributes

open FSharp.Extended.Benchmarks.Collections.Array.Utils

[<GenericTypeArguments(typeof<int>)>]
[<GenericTypeArguments(typeof<int64>)>]
[<GenericTypeArguments(typeof<double>)>]
[<BenchmarkCategory("Array", "Array.maxBy")>]
type ArrayMaxBy<'T when 'T : comparison>() =
    inherit ArrayBenchmarkBase<'T>()

    [<Benchmark(Description="Array.maxBy - FSharp.Core.Extended")>]
    member inline this.MaxExtended() =
        FSharp.Core.Extended.Collections.Array.maxBy id this.Array

    [<Benchmark(Description="Array.maxBy - FSharp.Core")>]
    member inline this.MaxDefault() =
        Microsoft.FSharp.Collections.Array.maxBy id this.Array
