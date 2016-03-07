#I __SOURCE_DIRECTORY__

#load "Utils.fs"

open System
open System.Threading
open Utils
open System.IO
open System.Net

let getHtml(url:string) =  //slow and blocks
      let req = WebRequest.Create url
      let response = req.GetResponse()
      use streatm = response.GetResponseStream()
      use reader = new StreamReader(streatm)
      reader.ReadToEnd().Length

let url = "http://www.eff.org"
let getHtmlA(url:string) =  
  async{
      let req = WebRequest.Create url
      let! response = req.AsyncGetResponse()
      use streatm = response.GetResponseStream()
      use reader = new StreamReader(streatm)
      return reader.ReadToEnd().Length
      }

let getHtmlAsync (url:string) = //can be slow and not block
  async{
    printfn "Starting to do stuff"  
    let req = WebRequest.Create url
    let! response = req.AsyncGetResponse()
    use streatm = response.GetResponseStream()
    use reader = new StreamReader(streatm)
    let result = reader.ReadToEndAsync().Result
//    Thread.Sleep 2000
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


// Not magic!!


let division a b c d = 
    match b with
    | 0 -> None
    | _ -> 
        match c with
        | 0 -> None
        | _ -> 
            match d with
            | 0 -> None
            | _ -> Some(((a / b) / c) / d)
    
let divide a b = 
    match b with
    | 0 -> None
    | _ -> Some(a / b)

type MaybeBuilder() =         
    member __.Bind(value, func) = 
        match value with
        | Some value -> func value
        | None -> None
        
    member __.Return value = Some value
    member __.ReturnFrom value = __.Bind(value, __.Return)
   
    
let maybe = MaybeBuilder()

let divisionM a b c d = 
  maybe { 
      let! x = divide a b
      let! y = divide x c
      let! z = divide y d
      return z }

   
module Desugared =
    type OtterMaybeBuilder() =
        member this.Bind(x, f) =
            printfn "this.Bind: %A" x
            match x with
            | Some(x) when x >= 0 && x <= 100 -> f(x)
            | _ -> None
        member this.Delay(f) = f()
        member this.Return(x) = Some x

    let maybe = new OtterMaybeBuilder()
    let sugared =
        maybe {
            let x = 10
            let! y = Some 13
            let! z = Some 11
            return x + y + z
        }
    sugared    
    let desugared = 
        maybe.Delay(fun () ->
            let x = 10
            maybe.Bind(Some 13, fun y ->
                maybe.Bind(Some 11, fun z ->
                    maybe.Return(x + y + z)
                    )
                )
            )

ShowGifUrl "http://i.giphy.com/DaeRzNNU4I6lO.gif"


module MonoidComputationExpression =
    
  type Colour = {
    r : byte
    g : byte
    b : byte
    a : byte }

  let addTwo c1 c2 = 
      { r = c1.r + c2.r
        g = c1.g + c2.g
        b = c1.b + c2.b
        a = c1.a + c2.a }

  let black = { 
        r = 0uy
        g = 0uy
        b = 0uy
        a = 0uy 
        }

  type MonoidBuilder ()=
      member __.Combine(a,b) = addTwo a b 
      member __.Zero() = black
      member __.Yield(s) = s
      member __.Delay f  = f() 
  let monoid = MonoidBuilder()

  let result = monoid{
          yield {r = 254uy; g=254uy; b= 33uy; a = 66uy}
          yield {r = 54uy; g=254uy; b= 33uy; a = 100uy}
      }