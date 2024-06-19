namespace FSharp.Core.Extended

[<AutoOpen>]
module Locks =

    [<AutoOpen; AbstractClass; Sealed; NoComparison; NoEquality>]
    type Lock() =
        /// <summary>Execute the function as a mutual-exclusion region using the input value as a lock. </summary>
        ///
        /// <param name="lockObject">The object to be locked.</param>
        /// <param name="action">The action to perform during the lock.</param>
        ///
        /// <returns>The resulting value.</returns>
        ///
        /// <example id="lock-example">
        /// <code lang="fsharp">
        /// open System.Linq
        ///
        /// /// A counter object, supporting unlocked and locked increment
        /// type TestCounter () =
        ///     let mutable count = 0
        ///
        ///     /// Increment the counter, unlocked
        ///     member this.IncrementWithoutLock() =
        ///         count &lt;- count + 1
        ///
        ///     /// Increment the counter, locked
        ///     member this.IncrementWithLock() =
        ///         lock this (fun () -> count &lt;- count + 1)
        ///
        ///     /// Get the count
        ///     member this.Count = count
        ///
        /// let counter = TestCounter()
        ///
        /// // Create a parallel sequence to that uses all our CPUs
        /// (seq {1..100000}).AsParallel()
        ///     .ForAll(fun _ -> counter.IncrementWithoutLock())
        ///
        /// // Evaluates to a number between 1-100000, non-deterministically because there is no locking
        /// counter.Count
        ///
        /// let counter2 = TestCounter()
        ///
        /// //  Create a parallel sequence to that uses all our CPUs
        /// (seq {1..100000}).AsParallel()
        ///     .ForAll(fun _ -> counter2.IncrementWithLock())
        ///
        /// //  Evaluates to 100000 deterministically because the increment to the counter object is locked
        /// counter2.Count
        /// </code>
        /// </example>
        ///
        [<CompiledName("Lock")>]
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

        /// <summary>Execute the function as a mutual-exclusion region using the input value as a lock. </summary>
        ///
        /// <param name="lockObject">The object to be locked.</param>
        /// <param name="action">The action to perform during the lock.</param>
        ///
        /// <returns>The resulting value.</returns>
        ///
        /// <example id="lock-example">
        /// <code lang="fsharp">
        /// open System.Linq
        ///
        /// /// A counter object, supporting unlocked and locked increment
        /// type TestCounter () =
        ///     let mutable count = 0
        ///
        ///     /// Increment the counter, unlocked
        ///     member this.IncrementWithoutLock() =
        ///         count &lt;- count + 1
        ///
        ///     /// Increment the counter, locked
        ///     member this.IncrementWithLock() =
        ///         lock this (fun () -> count &lt;- count + 1)
        ///
        ///     /// Get the count
        ///     member this.Count = count
        ///
        /// let counter = TestCounter()
        ///
        /// // Create a parallel sequence to that uses all our CPUs
        /// (seq {1..100000}).AsParallel()
        ///     .ForAll(fun _ -> counter.IncrementWithoutLock())
        ///
        /// // Evaluates to a number between 1-100000, non-deterministically because there is no locking
        /// counter.Count
        ///
        /// let counter2 = TestCounter()
        ///
        /// //  Create a parallel sequence to that uses all our CPUs
        /// (seq {1..100000}).AsParallel()
        ///     .ForAll(fun _ -> counter2.IncrementWithLock())
        ///
        /// //  Evaluates to 100000 deterministically because the increment to the counter object is locked
        /// counter2.Count
        /// </code>
        /// </example>
        ///
        [<CompiledName("Lock")>]
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