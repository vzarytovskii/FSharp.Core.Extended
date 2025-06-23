namespace FSharp.Core.Extended.Tests.Collections.Set

module Create =
  open Expecto
  open FsCheck.FSharp
  open FSharp.Core.Extended.Collections
  open FSharp.Core.Extended.Tests

  
  [<Tests>]
  let tests =
    testList "Set.Create" [

      testCase "Set of same values created as expected from the array" <| fun _ ->
        let value = 42
        let length = 1000
        let array = Gen.arrayOfLength length (Gen.constant value) |> Gen.sample 1 |> Seq.head
        let set = Set.create array
        Expect.equal (Set.count set) 1 $"Expected length to be {length} (but was {Set.count set})"
        Expect.allEqual set value $"Expected all elements to be {value} (but was {set})"

      testPropertyWithConfig Common.config "FSharp.Core and FSharp.Core.Extended Set.create produce the same results" <| fun (values: int array) ->
          let createDefault = Microsoft.FSharp.Collections.Set values
          let createExtended = FSharp.Core.Extended.Collections.Set.create values
          
          Microsoft.FSharp.Collections.Set.count createDefault = Set.count createExtended &&
          Set.forall createDefault.Contains createExtended
      ]
