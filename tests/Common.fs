namespace FSharp.Core.Faster.Tests

module Common =
    open Logary.Configuration
    open Logary.Targets
    open Hopac
    open Logary.Adapters.Facade

    let init () =
        Expecto.Expect.defaultDiffPrinter <- Expecto.Diff.colourisedDiff
        let logary =
            Config.create "FSharp.Core.Faster.Tests" "localhost"
            |> Config.targets [ LiterateConsole.create LiterateConsole.empty "console" ]
            |> Config.processing (Events.events |> Events.sink ["console";])
            |> Config.build
            |> run
        LogaryFacadeAdapter.initialise<Expecto.Logging.Logger> logary

    let config =
        { Expecto.FsCheckConfig.defaultConfig with
            maxTest = 100_000 }