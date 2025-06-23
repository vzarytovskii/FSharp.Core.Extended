namespace FSharp.Extended.Benchmarks.Collections.Set.Create

open BenchmarkDotNet.Attributes
open FSharp.Core.Extended.Benchmarks.Utils
open FSharp.Extended.Benchmarks.Collections.Array.Utils


[<GenericTypeArguments(typeof<int>)>]
[<GenericTypeArguments(typeof<int64>)>]
[<GenericTypeArguments(typeof<double>)>]
[<BenchmarkCategory("Set", "Set.create")>]
type SetCreate<'T when 'T : comparison>() =
    inherit ArrayBenchmarkBase<'T>()

    [<Params(1, 10, 100, 1_000, 10_000, 100_000, 1_000_000, 10_000_000)>]
    member val Count = 0 with get, set

    [<Benchmark(Description="Set.create - FSharp.Core.Extended")>]
    member inline this.CreateExtended() =
        FSharp.Core.Extended.Collections.Set.create this.Array

    [<Benchmark(Description="Set.create - FSharp.Core", Baseline=true)>]
    member inline this.CreateDefault() =
        Microsoft.FSharp.Collections.Set this.Array
