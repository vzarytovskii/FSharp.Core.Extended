namespace FSharp.Core.Extended

#nowarn "1204" // Compiler-only usage warnings
#nowarn "3391" // op_Implicit conversions warning
#nowarn "9"

[<AutoOpen>]
module Locks =
    [<AbstractClass; Sealed; NoComparison; NoEquality>]
    type Lock =
        static member inline Lock ((lockObject: System.Threading.Lock, action: unit -> 'a), _: Lock) : 'a =
            let scope = lockObject.EnterScope()
            try
                action ()
            finally
                scope.Dispose()

        static member inline Lock ((lockObject: obj, action: unit -> 'a), _: Lock) : 'a =
            assert (lockObject.GetType() <> typeof<System.Threading.Lock>)

            let mutable lockTaken = false
            try
                System.Threading.Monitor.Enter(lockObject, &lockTaken)
                action ()
            finally
                if lockTaken then
                    System.Threading.Monitor.Exit(lockObject)

        static member inline Invoke (action: unit -> 'U) (lockObject: 'L) : 'U =
                let inline call (mthd: ^M, lockObject: ^O, _output: ^R) = ((^M or ^O or ^R) : (static member Lock : (_ * _) * _ -> _) (lockObject, action), mthd)
                call (Unchecked.defaultof<Lock>, lockObject, Unchecked.defaultof<'U>)

    let inline lock (lockObject: 'T when 'T : not struct) (action: unit -> 'a) : 'a = Lock.Invoke action lockObject
