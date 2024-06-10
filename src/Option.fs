namespace FSharp.Core.Extended

open System
[<DefaultAugmentation(false)>]
[<StructuralEquality; StructuralComparison>]
[<CompilationRepresentation(CompilationRepresentationFlags.UseNullAsTrueValue)>]
[<CompiledName("FSharpOption`1")>]
type Option<'T> =
    | Some of 'T
    | None

    [<CompilationRepresentation(CompilationRepresentationFlags.Instance)>]
    member x.Value =
        match x with
        | Some x -> x
        | None -> raise (new InvalidOperationException("Option.Value"))

    [<CompilationRepresentation(CompilationRepresentationFlags.Instance)>]
    member x.IsNone =
        match x with
        | None -> true
        | _ -> false

    [<CompilationRepresentation(CompilationRepresentationFlags.Instance)>]
    member x.IsSome =
        match x with
        | Some _ -> true
        | _ -> false

    static member inline None : Option<'T> = None
    static member inline Some (value:'T) : Option<'T> = Some(value)
    static member op_Implicit (value: 'T) : Option<'T> = Some(value)
    static member op_Explicit (value: 'T) : Option<'T> = Some(value)

    static member op_Implicit (value: Option<'T>) : Microsoft.FSharp.Core.Option<'T> =
        match value with
        | Some value -> Microsoft.FSharp.Core.Some(value)
        | None -> Microsoft.FSharp.Core.None

    static member op_Explicit (value: Option<'T>) : Microsoft.FSharp.Core.Option<'T> =
        match value with
        | Some value -> Microsoft.FSharp.Core.Some(value)
        | None -> Microsoft.FSharp.Core.None

    static member op_Implicit (value: FSharp.Core.Option<'T>) : Option<'T> =
        match value with
        | Microsoft.FSharp.Core.Some x -> Some(x)
        | Microsoft.FSharp.Core.None -> None

    static member op_Explicit (value: FSharp.Core.Option<'T>) : Option<'T> =
        match value with
        | Microsoft.FSharp.Core.Some x -> Some(x)
        | Microsoft.FSharp.Core.None -> None

    static member op_Implicit (value: Option<'T>) : Microsoft.FSharp.Core.ValueOption<'T> =
        match value with
        | Some value -> Microsoft.FSharp.Core.ValueSome(value)
        | None -> Microsoft.FSharp.Core.ValueNone

    static member op_Explicit (value: Option<'T>) : Microsoft.FSharp.Core.ValueOption<'T> =
        match value with
        | Some value -> Microsoft.FSharp.Core.ValueSome(value)
        | None -> Microsoft.FSharp.Core.ValueNone

    static member op_Implicit (value: Microsoft.FSharp.Core.ValueOption<'T>) : Option<'T> =
        match value with
        | Microsoft.FSharp.Core.ValueSome x -> Some(x)
        | Microsoft.FSharp.Core.ValueNone -> None

    static member op_Explicit (value: Microsoft.FSharp.Core.ValueOption<'T>) : Option<'T> =
        match value with
        | Microsoft.FSharp.Core.ValueSome x -> Some(x)
        | Microsoft.FSharp.Core.ValueNone -> None

and 'T option = Option<'T>

[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module Option =
    [<CompiledName("OfFSharpOption")>]
    let inline ofFSharpOption (value: Microsoft.FSharp.Core.Option<'T>) : Option<'T> =
        match value with
        | Microsoft.FSharp.Core.Some x -> Some(x)
        | Microsoft.FSharp.Core.None -> None

    [<CompiledName("OfValueOption")>]
    let inline ofValueOption (value: Microsoft.FSharp.Core.ValueOption<'T>) : Option<'T> =
        match value with
        | Microsoft.FSharp.Core.ValueSome x -> Some(x)
        | Microsoft.FSharp.Core.ValueNone -> None

    [<CompiledName("ToFSharpOption")>]
    let inline toFSharpOption (value: Option<'T>) : Microsoft.FSharp.Core.Option<'T> =
        match value with
        | Some x -> Microsoft.FSharp.Core.Some(x)
        | None -> Microsoft.FSharp.Core.None

    [<CompiledName("ToValueOption")>]
    let toValueOption (value: Option<'T>) : Microsoft.FSharp.Core.ValueOption<'T> =
        match value with
        | Some x -> Microsoft.FSharp.Core.ValueSome(x)
        | None -> Microsoft.FSharp.Core.ValueNone

    [<CompiledName("GetValue")>]
    let inline get option =
        match option with
        | None -> invalidArg "option" "Option value is None"
        | Some x -> x

    [<CompiledName("IsSome")>]
    let inline isSome option =
        match option with
        | None -> false
        | Some _ -> true

    [<CompiledName("IsNone")>]
    let inline isNone option =
        match option with
        | None -> true
        | Some _ -> false

    [<CompiledName("DefaultValue")>]
    let inline defaultValue value option =
        match option with
        | None -> value
        | Some v -> v

    [<CompiledName("DefaultWith")>]
    let inline defaultWith ([<InlineIfLambda>] defThunk) option =
        match option with
        | None -> defThunk ()
        | Some v -> v

    [<CompiledName("OrElse")>]
    let inline orElse ifNone option =
        match option with
        | None -> ifNone
        | Some _ -> option

    [<CompiledName("OrElseWith")>]
    let inline orElseWith ([<InlineIfLambda>] ifNoneThunk) option =
        match option with
        | None -> ifNoneThunk ()
        | Some _ -> option

    [<CompiledName("Count")>]
    let inline count option =
        match option with
        | None -> 0
        | Some _ -> 1

    [<CompiledName("Fold")>]
    let inline fold<'T, 'State> ([<InlineIfLambda>] folder) (state: 'State) (option: 'T option) =
        match option with
        | None -> state
        | Some x -> folder state x

    [<CompiledName("FoldBack")>]
    let inline foldBack<'T, 'State> ([<InlineIfLambda>] folder) (option: option<'T>) (state: 'State) =
        match option with
        | None -> state
        | Some x -> folder x state

    [<CompiledName("Exists")>]
    let inline exists ([<InlineIfLambda>] predicate) option =
        match option with
        | None -> false
        | Some x -> predicate x

    [<CompiledName("ForAll")>]
    let inline forall ([<InlineIfLambda>] predicate) option =
        match option with
        | None -> true
        | Some x -> predicate x

    [<CompiledName("Contains")>]
    let inline contains value option =
        match option with
        | None -> false
        | Some v -> v = value

    [<CompiledName("Iterate")>]
    let inline iter ([<InlineIfLambda>] action) option =
        match option with
        | None -> ()
        | Some x -> action x

    [<CompiledName("Map")>]
    let inline map ([<InlineIfLambda>] mapping) option =
        match option with
        | None -> None
        | Some x -> Some(mapping x)

    [<CompiledName("Map2")>]
    let inline map2 ([<InlineIfLambda>] mapping) option1 option2 =
        match option1, option2 with
        | Some x, Some y -> Some(mapping x y)
        | _ -> None

    [<CompiledName("Map3")>]
    let inline map3 ([<InlineIfLambda>] mapping) option1 option2 option3 =
        match option1, option2, option3 with
        | Some x, Some y, Some z -> Some(mapping x y z)
        | _ -> None

    [<CompiledName("Bind")>]
    let inline bind ([<InlineIfLambda>] binder) option =
        match option with
        | None -> None
        | Some x -> binder x

    [<CompiledName("Flatten")>]
    let inline flatten option =
        match option with
        | None -> None
        | Some x -> x

    [<CompiledName("Filter")>]
    let inline filter ([<InlineIfLambda>] predicate) option =
        match option with
        | None -> None
        | Some x -> if predicate x then Some x else None

    [<CompiledName("ToArray")>]
    let inline toArray option =
        match option with
        | None -> [||]
        | Some x -> [| x |]

    [<CompiledName("ToList")>]
    let inline toList option =
        match option with
        | None -> []
        | Some x -> [ x ]

    [<CompiledName("ToNullable")>]
    let inline toNullable option =
        match option with
        | None -> System.Nullable()
        | Some v -> System.Nullable(v)

    [<CompiledName("OfNullable")>]
    let inline ofNullable (value: System.Nullable<'T>) =
        if value.HasValue then
            Some value.Value
        else
            None

    [<CompiledName("OfObj")>]
    let inline ofObj value =
        match value with
        | null -> None
        | _ -> Some value

    [<CompiledName("ToObj")>]
    let inline toObj value =
        match value with
        | None -> null
        | Some x -> x
