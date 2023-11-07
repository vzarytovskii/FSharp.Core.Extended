namespace FSharp.Core.Faster.Collections.Array

#nowarn "1204"

[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
[<RequireQualifiedAccess>]
module Array =
    open System.Linq
    let inline checkNonNull argName arg =
        if isNull arg then
            nullArg argName

    [<CompiledName("Length")>]
    let inline length (array: _[]) =
        checkNonNull "array" array
        array.Length

    [<CompiledName("Max")>]
    let inline max<'T> (array: 'T[]) =
        checkNonNull "array" array

        if array.Length = 0 then
            invalidArg "array" LanguagePrimitives.ErrorStrings.InputArrayEmptyString

        if array.Length = 1 then
            array[0]

        else
            Enumerable.Max array

    [<CompiledName("Min")>]
    let inline min<'T> (array: 'T[]) =
        checkNonNull "array" array

        if array.Length = 0 then
            invalidArg "array" LanguagePrimitives.ErrorStrings.InputArrayEmptyString

        if array.Length = 1 then
            array[0]

        else
            Enumerable.Min array