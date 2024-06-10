namespace FSharp.Core.Extended.Tests.Collections.Array

module Sum =
  open Expecto
  open System.Linq
  open FSharp.Core.Extended.Tests

  [<Tests>]
  let tests =
    testList "Array.Sum" [
      testPropertyWithConfig Common.config "FSharp.Core, System.Linq.Enumerable and FSharp.Core.Extended produce the same results" <| fun (xs: int64[]) ->
            let sumDefault = Microsoft.FSharp.Collections.Array.sum xs
            let sumFaster = FSharp.Core.Extended.Collections.Array.sum xs
            let sumLinq = Enumerable.Sum xs
            sumDefault = sumFaster && sumFaster = sumLinq
      ]
