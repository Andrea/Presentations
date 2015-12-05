namespace ComputationExpressionsInPractice

module Simplest =
    
    type OnlyContinueBuilder () =

        member this.Bind(x,f) = 
          printfn "Bind begin  %A %s " (System.DateTime.Now.TimeOfDay)  (x.ToString()) 
          let y = f x
          printfn "Bind end  %A %s " (System.DateTime.Now.TimeOfDay)  (x.ToString()) 
          y
        member this.Return(x) = 
            printfn "Return %A %s"  (System.DateTime.Now.TimeOfDay) (x.ToString())
            x

    let onlyContinue = new OnlyContinueBuilder() 

    let x =onlyContinue 
            {
                let! foo = "One"
                let! bar = "Two"
                let! fooBar = foo + bar
                return fooBar
            }

    
