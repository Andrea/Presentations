module ComputationExpression

open System

let sleepAndPrint x = 
    printfn "sleeping and printing"
    Async.Sleep x
let sleepWorkflow  = async{
    printfn "Starting sleep workflow at %O" DateTime.Now.TimeOfDay
    do! sleepAndPrint 2000
    printfn "Finished sleep workflow at %O" DateTime.Now.TimeOfDay
    }

Async.RunSynchronously sleepWorkflow      

