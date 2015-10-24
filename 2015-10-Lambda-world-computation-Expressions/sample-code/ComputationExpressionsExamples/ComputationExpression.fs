module AsyncWorkflow

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

module Worker =
    type Job =
        Description : string
        Tittle : string

    type JobApplication = 
        Id : int
        Applicant  : Applicant
        Job : Job

    let apply applicant job =
        application
        
