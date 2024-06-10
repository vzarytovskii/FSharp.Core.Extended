namespace FSharp.Core.Extended.Tests.Collections.Array

module Create =
  open Expecto
  open FSharp.Core.Extended.Tests

  [<Tests>]
  let tests =
    testList "Array.Create" [

      testCase "Array created as expected" <| fun _ ->
        let value = 42
        let length = 1000
        let array = FSharp.Core.Extended.Collections.Array.create length value
        Expect.equal array.Length length $"Expected length to be {length} (but was {array.Length})"
        Expect.allEqual array value $"Expected all elements to be {value} (but was {array})"

      testPropertyWithConfig Common.config "FSharp.Core and FSharp.Core.Extended Array.create produce the same results" <| fun (count: int) (value: int) ->
          // Just skip the test if the count is negative for now
          if count < 0 then
            true
          else
            let createDefault = Microsoft.FSharp.Collections.Array.create count value
            let createFaster = FSharp.Core.Extended.Collections.Array.create count value
            createDefault = createFaster &&
            Array.forall ((=) value) createFaster &&
            Array.forall ((=) value) createDefault
      ]
