namespace FSharp.Extended.Benchmarks.Collections.Array.Max

open BenchmarkDotNet.Attributes
open FSharp.Extended.Benchmarks.Collections.Array.Utils

#nowarn "20"

[<GenericTypeArguments(typeof<int>)>]
[<GenericTypeArguments(typeof<int64>)>]
[<GenericTypeArguments(typeof<double>)>]
[<BenchmarkCategory("Array", "Array.Sum")>]
type ArraySum<'T when 'T : comparison and 'T: (static member (+) : 'T * 'T -> 'T) and 'T: (static member Zero: 'T) and 'T: (new: unit -> 'T) and 'T: struct and 'T :> System.ValueType>() =
    inherit ArrayBenchmarkBase<'T>()
    [<Benchmark(Description="Array.sum - FSharp.Core.Extended")>]
    member inline this.SumFaster() =
        FSharp.Core.Extended.Collections.Array.sum this.Array
        ()

    [<Benchmark(Description="Array.sum - FSharp.Core")>]
    member inline this.SumDefault() =
        Microsoft.FSharp.Collections.Array.sum this.Array
        ()
