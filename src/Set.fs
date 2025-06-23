namespace FSharp.Core.Extended.Collections

open System.Linq
open System.Collections.Generic
open System.Collections.Immutable
open System.Diagnostics

#if NET_9_OR_GREATER
open System.Runtime.CompilerServices
#endif


[<CompiledName("FSharpSet`1")>]
[<DebuggerDisplay("Count = {Count}")>]
#if NET_9_OR_GREATER
[<CollectionBuilder(typeof<ImmutableHashSet>, nameof(ImmutableHashSet.Create : ReadOnlySpan<'T> -> ImmutableHashSet<'T>))>]
#endif
type Set<'T> = ImmutableHashSet<'T>

[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
[<RequireQualifiedAccess>]
module Set =

    [<CompiledName("Create")>]
    let inline create (xs: #IEnumerable<'T>) : Set<'T> = ImmutableHashSet.CreateRange(xs)
    
    [<CompiledName("IsEmpty")>]
    let inline isEmpty (set: Set<'T>) : bool = set.IsEmpty

    [<CompiledName("Contains")>]
    let inline contains element (set: Set<'T>) : bool =
        set.Contains element

    [<CompiledName("Add")>]
    let inline add value (set: Set<'T>) : Set<'T> =
        set.Add value

    [<CompiledName("Singleton")>]
    let inline singleton<'T> value : Set<'T> =
        ImmutableHashSet.Create(value : 'T)

    [<CompiledName("Remove")>]
    let inline remove value (set: Set<'T>) : Set<'T> =
        set.Remove value

    [<CompiledName("Union")>]
    let inline union (set1: Set<'T>) (set2: Set<'T>) : Set<'T> =
        set1.Union set2

    [<CompiledName("Intersect")>]
    let inline intersect (set1: Set<'T>) (set2: Set<'T>) =
        set1.Intersect set2

    [<CompiledName("Iterate")>]
    let inline iter ([<InlineIfLambda>] action: 'T -> unit) (set: Set<'T>) =
        for item in set do
            action item

    [<CompiledName("Empty")>]
    let inline empty<'T> : Set<'T> = Set<'T>.Empty

    [<CompiledName("ForAll")>]
    let inline forall ([<InlineIfLambda>] predicate: 'T -> bool) (set: Set<'T>) =
        set.All predicate

    [<CompiledName("Exists")>]
    let inline exists ([<InlineIfLambda>] predicate: 'T -> bool) (set: Set<'T>) =
        set.Any predicate

    [<CompiledName("Filter")>]
    let inline filter ([<InlineIfLambda>] predicate: 'T -> bool) (set: Set<'T>) =
        set.Where predicate

    [<CompiledName("Partition")>]
    let inline partition ([<InlineIfLambda>] predicate: 'T -> bool) (set: Set<'T>) : Set<'T> * Set<'T> =
        let lookup = set.ToLookup(predicate)
        create lookup[true], create lookup[false]

    [<CompiledName("Fold")>]
    let inline fold ([<InlineIfLambda>] folder: 'State -> 'T -> 'State) (state: 'State) (set: Set<'T>) : 'State =
        set.Aggregate(state, folder)
    
    [<CompiledName("Map")>]
    let inline map ([<InlineIfLambda>] mapping: 'T -> 'U) (set: Set<'T>) : Set<'U> =
        create (set.Select(mapping))

    [<CompiledName("Count")>]
    let inline count (set: Set<'T>) : int = set.Count

    [<CompiledName("Difference")>]
    let inline difference (set1: Set<'T>) (set2: Set<'T>) : Set<'T> =
        set1.Except set2

    [<CompiledName("IsSubset")>]
    let inline isSubset (set1: Set<'T>) (set2: Set<'T>) : bool =
        set1.IsSubsetOf set2

    [<CompiledName("IsSuperset")>]
    let inline isSuperset (set1: Set<'T>) (set2: Set<'T>) : bool =
        set1.IsSupersetOf set2

    [<CompiledName("IsProperSubset")>]
    let inline isProperSubset (set1: Set<'T>) (set2: Set<'T>) : bool =
        set1.IsProperSubsetOf set2

    [<CompiledName("IsProperSuperset")>]
    let inline isProperSuperset (set1: Set<'T>) (set2: Set<'T>) : bool =
        set1.IsProperSupersetOf set2

    [<CompiledName("MinElement")>]
    let inline minElement (set: Set<'T>) : 'T =
        Enumerable.Min set

    [<CompiledName("MaxElement")>]
    let inline maxElement (set: Set<'T>) : 'T =
        Enumerable.Max set
    
    [<CompiledName("ToList")>]
    let inline toList (set: Set<'T>) : 'T list =
        List.ofSeq set
    
    [<CompiledName("OfList")>]
    let inline ofList (list: 'T list) : Set<'T> =
        create list

    [<CompiledName("ToArray")>]
    let inline toArray (set: Set<'T>) : 'T array =
        Array.ofSeq set

    [<CompiledName("OfArray")>]
    let inline ofArray (array: 'T array) : Set<'T> =
        create array

    [<CompiledName("ToSeq")>]
    let inline toSeq (set: Set<'T>) : seq<'T> =
        set

    [<CompiledName("OfSeq")>]
    let inline ofSeq (seq: seq<'T>) : Set<'T> =
        create seq

    [<CompiledName("ToFSharpSet")>]
    let inline toFSharpSet (set: Set<'T>) : Microsoft.FSharp.Collections.Set<'T> =
        Microsoft.FSharp.Collections.Set.ofSeq set

    [<CompiledName("OfFSharpSet")>]
    let inline ofFSharpSet (set: Microsoft.FSharp.Collections.Set<'T>) : Set<'T> =
        create set
        
[<AutoOpen>]
module AutoOpens = 
    let inline set xs = Set.create xs