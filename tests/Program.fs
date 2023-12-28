namespace FSharp.Core.Faster.Tests
open Expecto
open Expecto.Logging
open Expecto.Logging.Message
open Logary.Configuration
open Logary.Adapters.Facade
open Logary.Targets
open Hopac

module Program =
    [<EntryPoint>]
    let main argv =
        let logary =
            Config.create "MyProject.Tests" "localhost"
            |> Config.targets [ LiterateConsole.create LiterateConsole.empty "console" ]
            |> Config.processing (Events.events |> Events.sink ["console";])
            |> Config.build
            |> run
        LogaryFacadeAdapter.initialise<Expecto.Logging.Logger> logary

        // Invoke Expecto:
        runTestsInAssemblyWithCLIArgs [] argv