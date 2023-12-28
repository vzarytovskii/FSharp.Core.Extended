namespace FSharp.Core.Faster.Collections.Array

#nowarn "1204" // Compiler-only usage warnings
#nowarn "3391" // op_Implicit conversions warning

[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
[<RequireQualifiedAccess>]
module Array =
    open System.Linq
    open System
    open System.Numerics
    open System.Runtime.InteropServices
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
            // These are already vectorized
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

    [<CompiledName("Sum")>]
    let inline sum (array: 'T[]) : 'T =
        checkNonNull "array" array
        if array.Length = 0 then
            Unchecked.defaultof<_>
        elif array.Length = 1 then
            array[0]
        else
            let mutable span = ReadOnlySpan<'T>(array)
            let mutable sum : 'T = Unchecked.defaultof<_>
            if Vector.IsHardwareAccelerated && span.Length > Vector<'T>.Count then
                let mutable sumVector = Vector<'T>.Zero
                let vectors = MemoryMarshal.Cast<'T, Vector<'T>>(span)
                for i in 0 .. vectors.Length - 1 do
                    sumVector <- sumVector + vectors[i]
                sum <- Vector.Sum(sumVector)
                let remainder = span.Length % Vector<'T>.Count
                span <- span.Slice(span.Length - remainder)

            for i in 0 .. span.Length - 1 do
                sum <- sum + span[i]

            sum
