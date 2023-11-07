namespace FSharp.Core.Faster.Benchmarks

open System
open BenchmarkDotNet.Running

type Program() =
    [<EntryPoint>]
    static let main args =
        let summary =
            BenchmarkSwitcher
                .FromAssembly(typeof<Program>.Assembly)
                .Run(args);

        if Object.ReferenceEquals (summary, null) then
            failwith "No summary"

        0