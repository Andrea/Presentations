namespace ComputationExpressionsInPractice

open NUnit.Framework

module Simplest =
    
    type OnlyContinueBuilder () =
        // all Bind does is make the binding happen, i.e. apply x to f
        member this.Bind(x,f) = 
          printfn "from Bind   %A %s " (System.DateTime.Now.TimeOfDay)  (x.ToString()) 
          let y = f x
          printfn "from Bind   %A %s " (System.DateTime.Now.TimeOfDay)  (x.ToString()) 
          y
        member this.Return(x) = 
            printfn "from return %A %s"  (System.DateTime.Now.TimeOfDay) (x.ToString())
            x

    let onlyContinue = new OnlyContinueBuilder() 

    let x =onlyContinue 
            {
                let! foo = "One"
                let! bar = "Two"
                let! fooBar = foo + bar
                return fooBar
            }

    