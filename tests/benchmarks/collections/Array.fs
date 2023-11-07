namespace FSharp.Faster.Benchmarks.Collections

open BenchmarkDotNet.Attributes

[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
[<RequireQualifiedAccess>]
module ArrayBenchmark =
     let private random = System.Random ()
     let arrays =
        [| 10 ; 100 ; 1_000 ; 10_000 ; 100_000 ; 1_000_000 ; 2_000_000 |]
        |> Array.map (fun i -> i, Array.zeroCreate i)
        |> Array.map (fun (index, arr) ->
            random.NextBytes arr
            index, arr
        )
        |> Map.ofArray

type ArrayBenchmark() =

    [<Params(10, 100, 1000, 10_000, 100_000, 1_000_000, 2_000_000)>]
    member val ArraySize = 0 with get, set

    [<Benchmark>]
    member this.MaxDefault() =
         Microsoft.FSharp.Collections.Array.max ArrayBenchmark.arrays[this.ArraySize]

    [<Benchmark>]
    member this.MaxFaster() =
        FSharp.Core.Faster.Collections.Array.Array.max ArrayBenchmark.arrays[this.ArraySize]

    [<Benchmark>]
    member this.MinDefault() =
            Microsoft.FSharp.Collections.Array.min ArrayBenchmark.arrays[this.ArraySize]

    [<Benchmark>]
    member this.MinFaster() =
        FSharp.Core.Faster.Collections.Array.Array.min ArrayBenchmark.arrays[this.ArraySize]
