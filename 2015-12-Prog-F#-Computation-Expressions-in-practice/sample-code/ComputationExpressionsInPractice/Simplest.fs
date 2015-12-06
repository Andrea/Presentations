namespace ComputationExpressionsInPractice


module Simplest =
(*
0. Welcome to the computation expression tutorial :D 
*)
  open System

  type SimplestBuilder () =  
      member this.Bind(x,f) = 
        printfn "Bind begin  %A %s " (DateTime.Now.TimeOfDay)  (x.ToString()) 
        let y = f x
        printfn "Bind end  %A %s " (DateTime.Now.TimeOfDay)  (x.ToString()) 
        y
      member this.Return(x) = 
          printfn "Return %A %s"  (DateTime.Now.TimeOfDay) (x.ToString())
          x

  let simple = new SimplestBuilder() 
(*
1. For this step you will need to write the body of the computation expression 
so that it works exactly the same way "y" does
*)
  let x =simple{
    let! one = "One"
    let! two = "Two"
    let! testing = one + two
    return testing
    }

  let y = 
    simple.Bind("One", fun one -> 
      simple.Bind("Two", fun two -> 
        simple.Bind(one + two, fun testing ->
          simple.Return(testing))))


  // An alternative way to write the code in CPS
  let z = 
    simple.Bind("One",     fun one -> 
    simple.Bind("Two",     fun two -> 
    simple.Bind(one + two, fun testing ->
    simple.Return(testing))))

   