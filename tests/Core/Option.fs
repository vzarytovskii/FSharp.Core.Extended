namespace FSharp.Core.Extended.Tests.Core.Option

module Min =
  open Expecto
  [<Tests>]
  let tests =
    testList "Option" [
      testCase "Option can be created" <| fun _ ->
        let opt = FSharp.Core.Extended.Option.Some(1)
        Expect.equal opt.Value 1 $"Expected opt.Value to be 1 (but was {opt.Value})"
      ]
