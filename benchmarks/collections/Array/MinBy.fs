namespace FSharp.Extended.Benchmarks.Collections.Array.Min

open BenchmarkDotNet.Attributes
open FSharp.Extended.Benchmarks.Collections.Array.Utils

[<GenericTypeArguments(typeof<int>)>]
[<GenericTypeArguments(typeof<int64>)>]
[<GenericTypeArguments(typeof<double>)>]
[<BenchmarkCategory("Array", "Array.MinBy")>]
type ArrayMinBy<'T when 'T : comparison>() =
    inherit ArrayBenchmarkBase<'T>()

    [<Benchmark(Description="Array.minBy - FSharp.Core.Extended")>]
    member inline this.MinExtended() =
        FSharp.Core.Extended.Collections.Array.minBy id this.Array

    [<Benchmark(Description="Array.minBy - FSharp.Core", Baseline=true)>]
    member inline this.MinDefault() =
            Microsoft.FSharp.Collections.Array.minBy id this.Array
