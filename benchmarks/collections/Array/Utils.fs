namespace FSharp.Extended.Benchmarks.Collections.Array.Utils

open BenchmarkDotNet.Attributes

[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
[<RequireQualifiedAccess>]
module ArrayBenchmark =
    open FSharp.Core.Extended.Benchmarks.Utils
    let random = System.Random ()
    // This is a huge footgun, might be wise to use fscheck or something.
    let inline generateArray<'T> (size: int) :'T array =
        Array.init size (fun _ -> retype (random.Next()))

type ArrayBenchmarkBase<'T>() =
    [<Params(1, 10, 100, 1_000, 10_000, 100_000, 1_000_000, 2_000_000)>]
    member val ArraySize = 0 with get, set

    member val Array : 'T array = Unchecked.defaultof<_> with get, set

    [<GlobalSetup>]
    member this.Setup() =
        this.Array <- ArrayBenchmark.generateArray<'T> this.ArraySize