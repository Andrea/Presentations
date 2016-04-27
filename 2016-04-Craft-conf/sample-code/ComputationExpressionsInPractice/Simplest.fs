namespace ComputationExpressionsInPractice
open System

module Simplest =

  type SimplestBuilder () =
      
      // what is f? b -> (b -> c) -> c  
      member this.Bind(x,f) = 
        printfn "Bind begin  %A %s " (DateTime.Now.TimeOfDay)  (x.ToString()) 
        let y = f x
        printfn "Bind end  %A %s " (DateTime.Now.TimeOfDay)  (x.ToString()) 
        y
      member this.Return(x) = 
          printfn "Return %A %s"  (DateTime.Now.TimeOfDay) (x.ToString())
          x

  let simple = new SimplestBuilder() 
// the more things looks different, the more they are the same

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

   