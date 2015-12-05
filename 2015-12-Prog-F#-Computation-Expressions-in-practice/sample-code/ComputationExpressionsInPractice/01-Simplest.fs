namespace ComputationExpressionsInPractice

open NUnit.Framework

module Simplest =
    
    type OnlyContinueBuilder () =
        // apply x to f
        member this.Bind(x,f) = 
          printfn "from Bind begin  %A %s " (System.DateTime.Now.TimeOfDay)  (x.ToString()) 
          let y = f x
          printfn "from Bind end  %A %s " (System.DateTime.Now.TimeOfDay)  (x.ToString()) 
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

    