namespace FSharp.Core.Faster.Tests.Collections.Array

module Sum =
  open Expecto
  open System.Linq

  let config: FsCheckConfig = { FsCheckConfig.defaultConfig with maxTest = 100_000 }
  [<Tests>]
  let tests =
    testList "Array.Sum" [
      testPropertyWithConfig config "FSharp.Core, System.Linq.Enumerable and FSharp.Core.Faster produce the same results" <| fun (xs: int[]) ->
            let sumDefault = Microsoft.FSharp.Collections.Array.sum xs
            let sumFaster = FSharp.Core.Faster.Collections.Array.Array.sum xs
            let sumLinq = Enumerable.Sum xs
            sumDefault = sumFaster && sumFaster = sumLinq
      ]
