namespace FSharp.Core.Extended

#nowarn "1204" // Compiler-only usage warnings
#nowarn "3391" // op_Implicit conversions warning
#nowarn "9"

[<AutoOpen>]
module Locks =
    [<AbstractClass; Sealed; NoComparison; NoEquality>]
    type Lock =

        static member inline Lock((lockObject: obj, _: Lock), action: unit -> 'a) =
            assert (lockObject.GetType() <> typeof<System.Threading.Lock>)

            let mutable lockTaken = false
            try
                System.Threading.Monitor.Enter(lockObject, &lockTaken)
                action ()
            finally
                if lockTaken then
                    System.Threading.Monitor.Exit(lockObject)

        static member inline Lock((lockObject: System.Threading.Lock, _: Lock), action: unit -> 'a) =
            let scope = lockObject.EnterScope()
            try
                action ()
            finally
                scope.Dispose()

        static member inline Invoke ([<InlineIfLambda>] action: unit -> 'U) (lockObject: 'T) : 'U =
            let inline call (mthd: ^M, lockObject: ^I, _output: ^R) = ((^M or ^I or ^R) : (static member Lock : (_ * _) * _ -> _) (lockObject, mthd), action)
            call (Unchecked.defaultof<Lock>, lockObject, Unchecked.defaultof<'U>)

    let inline lock (lockObject: _) ([<InlineIfLambda>] action: unit -> 'a) = Lock.Invoke action lockObject
