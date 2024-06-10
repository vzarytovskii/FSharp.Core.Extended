namespace FSharp.Core.Extended

#nowarn "1204" // Compiler-only usage warnings
#nowarn "3391" // op_Implicit conversions warning
#nowarn "9"

[<AutoOpen>]
module Locks =
    [<AbstractClass; Sealed; NoComparison; NoEquality>]
    type Lock =
#if NET9_OR_HIGHER
        static member inline Lock(lockObject: System.Threading.Lock, [<InlineIfLambda>] action: unit -> 'a, _: Lock) =
            use _ = lockObject.EnterLockScope()
            action ()
#endif
        static member inline Lock(lockObject: _, [<InlineIfLambda>] action: unit -> 'a, _: Lock) =
            let mutable lockTaken = false
            try
                System.Threading.Monitor.Enter(lockObject, &lockTaken);
                action()
            finally
                if lockTaken then
                    System.Threading.Monitor.Exit(lockObject)
