namespace FSharp.Core.Extended.Collections

#nowarn "1204" // Compiler-only usage warnings
#nowarn "3391" // op_Implicit conversions warning
#nowarn "9"

[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
[<RequireQualifiedAccess>]
module Array =
    open System.Linq
    open System
    open System.Numerics
    open System.Runtime.InteropServices

    open FSharp.Core.Extended

    let inline invalidArgFmt (arg:string) (format:string) paramArray =
        let msg = String.Format (format, paramArray)
        raise (new ArgumentException (msg, arg))

    let inline invalidArgInputMustBeNonNegative (arg:string) (count:int) =
        invalidArgFmt arg "{0}\n{1} = {2}" [| LanguagePrimitives.ErrorStrings.InputMustBeNonNegativeString ; arg; count |]

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

    [<CompiledName("Create")>]
    let inline create (count: int) (value: 'T) =
        // This implementation is the fastest by far when we operate on < 1M elements.
        // Benchmarked: InitBlock, Normal Fill, custom Vector-based fill.
        if count < 0 then
            invalidArgInputMustBeNonNegative "count" count

        let array: 'T[] = Array.zeroCreate count

        // TODO: Shall this be somehow more flexible?
        if count < 1_000_000 then
            array.AsSpan().Fill(value)
        else
            if Vector.IsHardwareAccelerated then
                let vector = Vector<'T>(value);
                let mutable idx = 0
                let c = Vector<'T>.Count

                while idx <= array.Length - c do
                    vector.CopyTo(array, idx)
                    idx <- idx + c

                idx <- array.Length - array.Length % c

                while idx < array.Length && idx >= 0 do
                    array[idx] <- value
                    idx <- idx + 1
            else
                //  Fallback to simple loop
                for i in 0 .. array.Length - 1 do
                    array.[i] <- value

        array

    [<CompiledName("TryHead")>]
    let inline tryHead (array: 'T[]) =
        checkNonNull "array" array

        if array.Length = 0 then
            None
        else
            Some array.[0]

    [<CompiledName("Choose")>]
    let inline choose (chooser: 'T -> Option<'U>) (array: 'T[]) : Option<'U>[] =
        checkNonNull "array" array
        [| for x in array do
            match chooser x with
            | Some y -> yield y
            | None -> () |]
