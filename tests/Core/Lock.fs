namespace FSharp.Core.Extended.Tests.Core.Lock

module Min =
    open Expecto
    open FSharp.Core.Extended.Locks

    let config = { FsCheckConfig.defaultConfig with maxTest = 100 }

    let lockTest syncRoot (count: int) =
        let mutable k = 0
        let comp syncRoot _ =
            async {
                return lock syncRoot (
                    fun () ->
                        k <- k + 1
                        System.Threading.Thread.Sleep(1)
                        k
                )
            }
        let arr = Async.RunSynchronously (Async.Parallel(Seq.map (comp syncRoot) [1..count]))
        Expect.equal [|1..count|] (Array.sort arr) $"Expected {[1..count]} but was {arr}"

    [<Tests>]
    let tests =
        testList "Lock" [
            testPropertyWithConfig config "System.Object lock works as expected" (lockTest (System.Object()))
            testPropertyWithConfig config "System.Threading.Lock lock works as expected" (lockTest (System.Threading.Lock()))
        ]
