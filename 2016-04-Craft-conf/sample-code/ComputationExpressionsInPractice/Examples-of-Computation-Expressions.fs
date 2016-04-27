namespace ComputationExpressionsInPractice


module Samples =
  open System
  open System.Threading
  open System.IO
  open System.Net

  let getHtml(url:string) =               //slow and blocks
        let req = WebRequest.Create url
        let response = req.GetResponse()
        use streatm = response.GetResponseStream()
        use reader = new StreamReader(streatm)
        printfn "Page length is %A." <| reader.ReadToEnd().Length

  let url = "http://www.eff.org"

  getHtml url
  let getHtmlAsync (url:string) = //can be slow and not block
    async{
      printfn "Starting to do stuff"  
      let req = WebRequest.Create url
      let! response = req.AsyncGetResponse()
      use streatm = response.GetResponseStream()
      use reader = new StreamReader(streatm)
      let result = reader.ReadToEndAsync().Result
      Thread.Sleep 2000
      printfn "lenght %A " result.Length
      return result.Length
    }

  let sites = [
    "http://www.eff.org/"  
    "http://www.ted.com/talks/edward_snowden_here_s_how_we_take_back_the_internet#t-704593"  
    "http://freedom.press/"
    "http://imgur.com"
    "http://theguardian.com"
    "http://rte.ie"
    ]
  sites
  |> List.map (getHtmlAsync)
  |> Async.Parallel
  |> Async.RunSynchronously

  let getHtmlAsyncTry (url:string) = 
    async{
      printfn "Starting to do stuff" 
      try 
        let req = WebRequest.Create url
        let! response = req.AsyncGetResponse()
        use streatm = response.GetResponseStream()
        use reader = new StreamReader(streatm)
        let r = reader.ReadToEndAsync().Result
        System.Threading.Thread.Sleep 1000
        printfn "lenght %A " r.Length
        return r.Length
      with
      | :? IOException as io -> 
          printfn "IO exc %A" io.Message 
          return 0
      | :? ArgumentException as ae -> 
          printfn "IO exc %A" ae.Message
          return 0
    }
  
  let myFunction url = 
  
    getHtmlAsync url  
    |> Async.Catch  
    |> Async.RunSynchronously     
    |> function 
        | Choice1Of2 result -> printfn "all good %A" result
        | Choice2Of2 (ex:exn) -> printfn "Errors :( %A" ex.Message

  let s = List.map(myFunction) sites 

  // Cancellation 

  let cancelableTask =
    async {
      printfn "Waiting"
      for i = 1 to 42 do
      printfn "Counting %d" i
      do! Async.Sleep(1000)
      printfn "Done"
    }
  let notCancelableTask =
    async {    
      for i = 1 to 42 do
      printfn "Don't cancel Counting %d" i
      do! Async.Sleep(1000)
    } |> Async.RunSynchronously

  let handler (ex) = printfn "Cancelled %A" ex

  Async.TryCancelled( cancelableTask, handler)
  |> Async.Start

  Async.CancelDefaultToken() // if there are other tasks running it blocks :(


  let comp = Async.TryCancelled(cancelableTask, handler)
  let cancelToken = new CancellationTokenSource()

  Async.Start(comp, cancelToken.Token)

  cancelToken.Cancel()



module AsycExamples =

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
  Async.Start sleepWorkflow

module LoggingExamples =
  type LoggingBuilder() =
      let log p = printfn "expression is %A" p

      member this.Bind(x, f) = 
          log x
          f x

      member this.Return(x) = 
          x
  let logger = new LoggingBuilder()
  let loggedWorkflow = 
      logger
         {
          let! x = 42
          let! y = 43
          let! z = x + y
          return z
          }
// Example from Programming F# 3
module InventoryExample = 
    open System.Collections.Generic
    
    type ProductId = string
    type Price = float
    
    type Inventory() = 
        let inv = new Dictionary<ProductId, Price>()
        member this.Stock id price = inv.Add(id, price)
        member this.Price id = 
            try 
                Some(inv.[id])
            with :? KeyNotFoundException -> None

    let maybe = MaybeBuilder()

    let (|@|) p1 p2 = maybe { 
        let! v1 = p1
        let! v2 = p2
        return v1 + v2 }
    
    let reporter priceSum  = 
        match priceSum with
        | Some(p) -> printfn "Total price: %g." p
        | None -> printfn "One or more id not found."
    
    let inventory = new Inventory()
    
    inventory.Stock "MyWidget" 10.3
    inventory.Stock "Gizmos" 4.34
    inventory.Stock "Foo1000" 8.12
    
    //Sum prices
    inventory.Price("MyWidget") |@| inventory.Price("Gizmos") |> reporter
    
    //A further step, price sum pipelining
    inventory.Price("MyWidget")
    |> (|@|) (inventory.Price("Gizmos"))
    |> (|@|) (inventory.Price("Foo1000"))
    |> reporter
    
    //A failing computation
    inventory.Price("MyWidget")
    |> (|@|) (inventory.Price("Gizmos"))
    |> (|@|) (inventory.Price("DoesNotExist"))
    |> reporter    
    
    let sumAndReport (inventory : Inventory) ids = 
        let basket = List.map (fun pid -> inventory.Price(pid)) ids
        List.reduce (fun p1 p2 -> p1 |@| p2) basket |> reporter



module CalculateResistance = 
    type Result = 
        | Success of float
        | DivByZero
    
    let divide x y = 
        match y with
        | 0.0 -> DivByZero
        | _ -> Success(x / y)
    
    type DefineBuilder() = 
        
        member this.Bind(x : Result, rest : float -> Result) = 
            match x with
            | Success(x) -> rest x
            | DivByZero -> DivByZero
        
        member this.Return(x : 'a) = x
    
    let definer = DefineBuilder()
    let totalResistance r1 r2 r3 = definer { let! x = divide 1.0 r1
                                             let! y = divide 1.0 r2
                                             let! z = divide 1.0 r3
                                             return divide 1.0 (x + y + z) }


// Example from https://alfredodinapoli.wordpress.com/2012/04/02/humbly-simple-f-maybe-monad-application-scenario/
// but using local Maybe definition 
module StringChecker = 

  type StringCheckerBuilder() =
          member __.Bind(value, func) = 
              match value with
              | Some i  -> func i
              | None -> 0
        
          member __.Return value = value
        
  let strToInt (s: string) = 
      match System.Int32.TryParse s with
      | true, i -> Some i
      | false, _ -> None
                        
  let stringCheck = StringCheckerBuilder()
  let stringAddWorkflow x y z = 
      stringCheck 
          {
          let! a = strToInt x
          let! b = strToInt y
          let! c = strToInt z
          return a + b + c
          }

 
  let good = stringAddWorkflow "12" "3" "2"
  let bad = stringAddWorkflow "12" "xyz" "2"
