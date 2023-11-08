namespace FSharp.Faster.Benchmarks.Collections

open BenchmarkDotNet.Attributes
open FSharp.Core.Faster.Benchmarks.Utils

[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
[<RequireQualifiedAccess>]
module ArrayBenchmark =
    let random = System.Random ()

    // This is a huge footgun, might be wise to use fscheck or something.
    let inline generateArray<'T> (size: int) :'T array =
        Array.init size (fun _ -> retype (random.Next()))

[<GenericTypeArguments(typeof<int>)>]
[<GenericTypeArguments(typeof<int64>)>]
[<GenericTypeArguments(typeof<double>)>]
type ArrayMaxBenchmark<'T when 'T : comparison>() =

    [<Params(1, 10, 100, 1_000, 10_000, 100_000, 1_000_000, 2_000_000)>]
    member val ArraySize = 0 with get, set

    member val Array : 'T array = Unchecked.defaultof<_> with get, set

    [<GlobalSetup>]
    member this.Setup() =
        this.Array <- ArrayBenchmark.generateArray<'T> this.ArraySize

    [<Benchmark(Description="Array.max - FSharp.Core.Faster")>]
    member this.MaxFaster() =
        FSharp.Core.Faster.Collections.Array.Array.max this.Array

    [<Benchmark(Description="Array.max - FSharp.Core")>]
    member this.MaxDefault() =
         Microsoft.FSharp.Collections.Array.max this.Array


[<GenericTypeArguments(typeof<int>)>]
[<GenericTypeArguments(typeof<int64>)>]
[<GenericTypeArguments(typeof<double>)>]
type ArrayMinBenchmark<'T when 'T : comparison>() =
    [<Params(1_000, 10_000, 100_000, 1_000_000, 2_000_000)>]
    member val ArraySize = 0 with get, set

    member val Array : 'T array = Unchecked.defaultof<_> with get, set

    [<GlobalSetup>]
    member this.Setup() =
        this.Array <- ArrayBenchmark.generateArray<'T> this.ArraySize

    [<Benchmark(Description="Array.min - FSharp.Core.Faster")>]
    member this.MinFaster() =
        FSharp.Core.Faster.Collections.Array.Array.min this.Array

    [<Benchmark(Description="Array.min - FSharp.Core")>]
    member this.MinDefault() =
            Microsoft.FSharp.Collections.Array.min this.Array
