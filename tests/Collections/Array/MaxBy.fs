namespace FSharp.Core.Extended.Tests.Collections.Array

module maxBy =
  open Expecto
  open System
  open System.Linq
  open FSharp.Core.Extended.Tests

  [<Tests>]
  let tests =
    testList "Array.MaxBy" [
      testCase "MaxBy works as expected with negative infinity" <| fun _ ->
        let xs = [| -infinity; 1.0; 2.0 |]
        let max = FSharp.Core.Extended.Collections.Array.maxBy (fun x -> x) xs
        Expect.equal max 2.0 $"Expected max to be 2.0 (but was {max})"

      testCase "MaxBy works as expected with infinity" <| fun _ ->
        let xs = [| infinity; 1.0; 2.0 |]
        let max = FSharp.Core.Extended.Collections.Array.maxBy (fun x -> x) xs
        Expect.equal max infinity $"Expected max to be infinity (but was {max})"

      testCase "MaxBy works as expected with negative numbers" <| fun _ ->
        let xs = [| -1; -2; -3 |]
        let max = FSharp.Core.Extended.Collections.Array.maxBy (fun x -> x) xs
        Expect.equal max -1 $"Expected max to be -1 (but was {max})"

      testCase "MaxBy works as expected with NaN as the first element" <| fun _ ->
        let xs = [| nan; 1.0; 2.0 |]
        let max = FSharp.Core.Extended.Collections.Array.maxBy (fun x -> x) xs
        Expect.equal max 2 $"Expected max to be 2 (but was {max})"

      testCase "MaxBy returns max when it is NOT the first element" <| fun _ ->
        let xs = [| 1.0; nan; 2.0 |]
        let max = FSharp.Core.Extended.Collections.Array.maxBy (fun x -> x) xs
        Expect.equal max 2 $"Expected max to be 2 (but was {max})"

      testCase "MaxBy works as expected with positive numbers" <| fun _ ->
        let xs = [| 1.0; 2.0; 3.0 |]
        let max = FSharp.Core.Extended.Collections.Array.maxBy (fun x -> x) xs
        Expect.equal max 3.0 $"Expected max to be 3.0 (but was {max})"

      testCase "MaxBy works as expected with mixed numbers" <| fun _ ->
        let xs = [| -1.0; 0.0; 1.0 |]
        let max = FSharp.Core.Extended.Collections.Array.maxBy (fun x -> x) xs
        Expect.equal max 1.0 $"Expected max to be 1.0 (but was {max})"

      testCase "MaxBy throws an exception as expected with an empty array" <| fun _ ->
        let xs = [||]
        Expect.throwsT<ArgumentException> (fun () -> FSharp.Core.Extended.Collections.Array.maxBy (fun x -> x) xs) "Expected maxBy to throw an exception for empty array"

      testPropertyWithConfig Common.config "FSharp.Core, Enumerable.Max and FSharp.Core.Extended Array.Max produce the same results" <| fun (xs: int[]) ->
          // Empty array handling is one of the differences between those 3
          // Different exceptions are thrown
          if xs.Length = 0 then
            true
          else
            let maxDefault = Microsoft.FSharp.Collections.Array.maxBy (fun x -> x) xs
            let maxExtended = FSharp.Core.Extended.Collections.Array.maxBy (fun x -> x) xs
            let maxLinq = Enumerable.MaxBy(xs, (fun x -> x))
            maxDefault = maxExtended && maxExtended = maxLinq
      ]
