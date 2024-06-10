namespace FSharp.Core.Extended.Tests

module Program =
    [<EntryPoint>]
    let main argv =
        Common.init()
        Expecto.Tests.runTestsInAssemblyWithCLIArgs [] argv