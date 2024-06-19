namespace FSharp.Core.Extended.Tests.Core.Lock

module Min =
    open Expecto
    open FSharp.Core.Extended.Locks
    open System


    let private config =
        { Expecto.FsCheckConfig.defaultConfig with
            maxTest = 200 }


    [<Tests>]
    let tests =
        testList "Lock" [
            // These two will throw in DEBUG if it goes to the wrong lock implementation (via assert)
            testCase "System.Threading.Lock casted to System.Object passed to lock 'overload' which accepts System.Object causes an assert" <| fun () ->
                Expect.throwsT<ArgumentException> (fun () -> lock (System.Threading.Lock() :> obj) (fun () -> ())) "System.Threading.Lock should not be passed to this function"
            testCase "System.Object is accepted in the lock function" <| fun () -> lock (obj()) (fun () -> ())
            testCase "System.Threading.Lock is accepted in the lock function" <| fun () -> lock (System.Threading.Lock()) (fun () -> ())
            testPropertyWithConfig config "System.Object lock works as expected" <| fun (maxvalue: int) ->
                let syncRoot = System.Object()
                let mutable k = 0
                let comp _ =
                    async {
                        return lock syncRoot <| fun () ->
                            k <- k + 1
                            System.Threading.Thread.Sleep(1)
                            k
                    }
                let arr = Async.RunSynchronously (Async.Parallel(Seq.map comp [1 .. maxvalue]))

                let expected = [| 1 .. maxvalue |]
                let actual = Array.sort arr

                Expect.equal expected actual $"Expected the array to be {expected} but was {actual}"
            testPropertyWithConfig config "System.Threading.Lock lock works as expected" <| fun (maxvalue: int) ->
                let syncRoot = System.Threading.Lock()
                let mutable k = 0
                let comp _ =
                    async {
                        return lock syncRoot <| fun () ->
                            k <- k + 1
                            System.Threading.Thread.Sleep(1)
                            k
                    }
                let arr = Async.RunSynchronously (Async.Parallel(Seq.map comp [1 .. maxvalue]))

                let expected = [| 1 .. maxvalue |]
                let actual = Array.sort arr

                Expect.equal expected actual $"Expected the array to be {expected} but was {actual}"
        ]
