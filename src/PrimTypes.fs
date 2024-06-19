namespace FSharp.Core.Extended

[<AutoOpen>]
module Locks =

    [<AutoOpen; AbstractClass; Sealed; NoComparison; NoEquality>]
    type Lock() =
        static member inline lock (lockObject: System.Threading.Lock) =
            fun ([<InlineIfLambda>] action: unit -> 'R) ->
#if DEBUG
                if (lockObject.GetType() <> typeof<System.Threading.Lock>) then
                    raise (new System.ArgumentException("System.Threading.Lock should be passed to this function", nameof(lockObject)))
#endif
                let scope = lockObject.EnterScope()
                try
                    action ()
                finally
                    scope.Dispose()

        static member inline lock (lockObject: 'T when 'T : not struct) =
            fun ([<InlineIfLambda>] action: unit -> 'R) ->
#if DEBUG
                if (lockObject.GetType() = typeof<System.Threading.Lock>) then
                    raise (new System.ArgumentException("System.Threading.Lock should not be passed to this function", nameof(lockObject)))
#endif
                System.Threading.Monitor.Enter(lockObject)
                try
                    action ()
                finally
                    System.Threading.Monitor.Exit(lockObject)