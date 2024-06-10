namespace FSharp.Extended.Benchmarks.Collections.Array.Choose

#nowarn "64"

open BenchmarkDotNet.Attributes
open FSharp.Extended.Benchmarks.Collections.Array.Utils
open FSharp.Core.Extended

[<GenericTypeArguments(typeof<int>)>]
[<GenericTypeArguments(typeof<int64>)>]
[<GenericTypeArguments(typeof<double>)>]
[<BenchmarkCategory("Array", "Array.Choose")>]
type ArrayChoose<'T when 'T : comparison>() =
    inherit ArrayBenchmarkBase<'T>()
        member val ChooserFaster : 'T -> Option<'U> = fun x -> Some x with get, set
        member val Chooser : 'T -> Microsoft.FSharp.Core.Option<'U> = fun x -> Microsoft.FSharp.Core.Option.Some x with get, set


    [<Benchmark(Description="Array.choose - FSharp.Core.Extended")>]
    member inline this.ChooseFaster() =
        FSharp.Core.Extended.Collections.Array.choose this.ChooserFaster this.Array

    [<Benchmark(Description="Array.choose - FSharp.Core")>]
    member inline this.ChooseDefault() =
            Microsoft.FSharp.Collections.Array.choose this.Chooser this.Array
