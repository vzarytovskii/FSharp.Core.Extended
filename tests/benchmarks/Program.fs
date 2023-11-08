namespace FSharp.Core.Faster.Benchmarks

open System
open BenchmarkDotNet.Running
open BenchmarkDotNet.Configs
open BenchmarkDotNet.Diagnosers
open BenchmarkDotNet.Loggers
open BenchmarkDotNet.Exporters
open BenchmarkDotNet.Columns
open BenchmarkDotNet.Exporters.Csv
open BenchmarkDotNet.Analysers
open BenchmarkDotNet.Jobs
open BenchmarkDotNet.Environments
open BenchmarkDotNet.Order

type Program() =
    [<EntryPoint>]
    static let main args =
        let config =
            ManualConfig
                .CreateEmpty()
                .StopOnFirstError(true)
                .WithOption(ConfigOptions.JoinSummary, true)
                .AddDiagnoser(
                    MemoryDiagnoser.Default,
                    ThreadingDiagnoser.Default,
                    ExceptionDiagnoser.Default)
                .AddLogger(ConsoleLogger.Default)
                .AddExporter(
                    MarkdownExporter.GitHub,
                    CsvMeasurementsExporter.Default)
                .AddColumnProvider(DefaultColumnProviders.Instance)
                .WithOrderer(
                    DefaultOrderer(
                        SummaryOrderPolicy.Declared,
                        MethodOrderPolicy.Declared)
                )
                .AddAnalyser(
                    EnvironmentAnalyser.Default,
                    MinIterationTimeAnalyser.Default,
                    RuntimeErrorAnalyser.Default,
                    ZeroMeasurementAnalyser.Default)
                .AddJob(
                    Job
                        .Default
                        .WithJit(Jit.RyuJit)
                        .WithGcServer(true)
                        .WithGcConcurrent(true))
        let summary =
            BenchmarkSwitcher
                .FromAssembly(typeof<Program>.Assembly)
                .Run(args, config);

        if Object.ReferenceEquals (summary, null) then
            failwith "No summary"

        0