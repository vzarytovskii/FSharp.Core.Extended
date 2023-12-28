namespace FSharp.Core.Faster.Tests.Collections.Array

module Sum =
  open Expecto
  open System.Linq
  open FSharp.Core.Faster.Tests

  [<Tests>]
  let tests =
    testList "Array.Sum" [
      testPropertyWithConfig Common.config "FSharp.Core, System.Linq.Enumerable and FSharp.Core.Faster produce the same results" <| fun (xs: int64[]) ->
            let sumDefault = Microsoft.FSharp.Collections.Array.sum xs
            let sumFaster = FSharp.Core.Faster.Collections.Array.sum xs
            let sumLinq = Enumerable.Sum xs
            sumDefault = sumFaster && sumFaster = sumLinq
      ]
