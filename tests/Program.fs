namespace FSharp.Core.Faster.Tests

module Program =
    [<EntryPoint>]
    let main argv =
        Common.init()
        Expecto.Tests.runTestsInAssemblyWithCLIArgs [] argv