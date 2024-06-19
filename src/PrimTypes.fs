namespace FSharp.Core.Extended

[<AutoOpen>]
module Locks =

    [<AutoOpen; AbstractClass; Sealed; NoComparison; NoEquality>]
    type Lock() =
        static member inline lock (lockObject: System.Threading.Lock) =
            fun ([<InlineIfLambda>] action: unit -> 'R) ->
                assert (lockObject.GetType() = typeof<System.Threading.Lock>)
                let scope = lockObject.EnterScope()
                try
                    action ()
                finally
                    scope.Dispose()

        static member inline lock (lockObject: 'T when 'T : not struct) =
            fun ([<InlineIfLambda>] action: unit -> 'R) ->
                assert (lockObject.GetType() <> typeof<System.Threading.Lock>)
                System.Threading.Monitor.Enter(lockObject)
                try
                    action ()
                finally
                    System.Threading.Monitor.Exit(lockObject)