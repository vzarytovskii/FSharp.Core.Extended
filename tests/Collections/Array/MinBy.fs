namespace FSharp.Core.Extended.Tests.Collections.Array

module MinBy =
  open Expecto
  open System
  open System.Linq
  open FSharp.Core.Extended.Tests

  [<Tests>]
  let tests =
    testList "Array.MinBy" [
      testCase "MinBy works as expected with negative infinity" <| fun _ ->
        let xs = [| -infinity; 1.0; 2.0 |]
        let min = FSharp.Core.Extended.Collections.Array.minBy id xs
        Expect.isTrue (Double.IsNegativeInfinity min) $"Expected min to be -infinity (but was {min})"

      testCase "MinBy works as expected with infitiy" <| fun _ ->
        let xs = [| infinity; 1.0; 2.0 |]
        let min = FSharp.Core.Extended.Collections.Array.minBy id xs
        Expect.equal min 1.0 $"Expected min to be 1.0 (but was {min})"

      testCase "MinBy works as expected with negative numbers" <| fun _ ->
        let xs = [| -1; -2; -3 |]
        let min = FSharp.Core.Extended.Collections.Array.minBy id xs
        Expect.equal min -3 $"Expected min to be -3 (but was {min})"

      testCase "MinBy returns NaN when it is the first element" <| fun _ ->
        let xs = [| nan; 1.0; 2.0 |]
        let min = FSharp.Core.Extended.Collections.Array.minBy id xs
        Expect.isTrue (Double.IsNaN min) $"Expected min to be NaN (but was {min})"

      testCase "MinBy returns NaN when it is NOT the first element" <| fun _ ->
        let xs = [| 1.0; nan; 2.0 |]
        let min = FSharp.Core.Extended.Collections.Array.minBy id xs
        Expect.isTrue (Double.IsNaN min) $"Expected min to be NaN (but was {min})"

      testPropertyWithConfig Common.config "FSharp.Core, Enumerable.MinBy and FSharp.Core.Extended.Array.MinBy produce the same results" <| fun (xs: int[]) ->
          // Empty array handling is one of the differences between those 3
          // Different exceptions are thrown
          if xs.Length = 0 then
            true
          else
            let minDefault = Microsoft.FSharp.Collections.Array.minBy id xs
            let minExtended = FSharp.Core.Extended.Collections.Array.minBy id xs
            let minLinq = Enumerable.MinBy(xs, id)
            minDefault = minExtended && minExtended = minLinq
      ]
