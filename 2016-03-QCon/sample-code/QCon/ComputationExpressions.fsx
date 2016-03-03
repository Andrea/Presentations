#I __SOURCE_DIRECTORY__
#r "packages/FSharp.Data.2.2.5/lib/net40/FSharp.Data.dll"
#r "System.Core.dll"
#r "System.dll"
#r "System.Numerics.dll"
#r "System.Xml.Linq.dll"
#load "Utils.fs"

open FSharp.Data
open System
open System.IO
open System.Threading
open Utils

type GiphyTP2 = JsonProvider<"http://api.giphy.com/v1/gifs/search?q=monkey+cat&rating=pg-13&api_key=dc6zaTOxFJmzC"> /// special F# thing from Giphy
let baseUrl = "http://api.giphy.com/v1/gifs/search"
let key = "dc6zaTOxFJmzC"

type GifSize =
  | Downsized
  | FixedHeight
  | FixedWidth
  | Original
type GifData ={
    Url : Uri
    Size: int
    Rating: String
  }

// More on the erased types
//let getBySize images size rating = 
//  let gifUrl =match size with // find by size
//              | Downsized -> images.DownsizedMedium.Url, images.Downsized.Size
//              | FixedHeight -> images.FixedHeight.Url, images.FixedHeight.Size
//              | FixedWidth -> images.FixedWidth.Url, images.FixedWidth.Size
//              | Original -> images.Original.Url,  images.Original.Size
//  {Url =Uri(fst gifUrl); Size= snd gifUrl; Rating= rating}    

let searchGif searchString size =
  let searchTerm = String.map(fun c ->  if (c = ' ' )then '+' else c ) searchString
  let query = ["api_key", key; "q", searchTerm]
  let response = Http.RequestString (baseUrl,  query)  
  
  let giphy = GiphyTP2.Parse(response)
  printfn "Status of the request %A" giphy.Meta.Msg
  giphy.Data
  |> Seq.map( fun x ->      
      let gifUrl = 
        match size with // find by size
        | Downsized -> x.Images.DownsizedMedium.Url, x.Images.Downsized.Size
        | FixedHeight -> x.Images.FixedHeight.Url, x.Images.FixedHeight.Size
        | FixedWidth -> x.Images.FixedWidth.Url, x.Images.FixedWidth.Size
        | Original -> x.Images.Original.Url,  x.Images.Original.Size
      {Url =Uri(fst gifUrl); Size= snd gifUrl; Rating= x.Rating})    


let flyingCat = searchGif "Picard" Downsized |> Seq.head 

if flyingCat.Rating = "g" then  
  ShowGif flyingCat.Url


// async workflows great for IO bound computations

let asyncProcess (path:string) doSomething = 
  async {
    printfn "Processing file %A" (Path.GetFileName path)

    use stream = new FileStream(path, FileMode.Open)
    let toRead = int stream.Length

    let! data = stream.AsyncRead(toRead)
    printfn "Read %i bytes " data.Length

    let data' = doSomething data
    use result = new FileStream (path+".results", FileMode.Create)
    do! result.AsyncWrite (data', 0, data'.Length)

  } |> Async.Start

let path = System.IO.Path.Combine(__SOURCE_DIRECTORY__,"AssemblyInfo.fs")  
let identity bytes =   
  System.Threading.Thread.Sleep 10
  printfn "a"
  bytes

asyncProcess path identity

//let asyncWF = async {    }

open System.IO
open System.Net

let getHtml(url:string) =
      let req = WebRequest.Create url
      let response = req.GetResponse()
      use streatm = response.GetResponseStream()
      use reader = new StreamReader(streatm)
      reader.ReadToEnd()

let getHtmlAsync (url:string) = 
  async{
    printfn "Starting to do stuff"  
    let req = WebRequest.Create url
    let! response = req.AsyncGetResponse()
    use streatm = response.GetResponseStream()
    use reader = new StreamReader(streatm)
    let r = reader.ReadToEndAsync().Result
    System.Threading.Thread.Sleep 1000
    printfn "lenght %A " r.Length
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
|>Async.Parallel
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

let ss url = 
  getHtmlAsyncTry url   
  |> Async.Catch  
  |> Async.RunSynchronously   //TODO: how to run this in parallel?
  |> function 
      | Choice1Of2 result -> printfn "all good %A" result
      | Choice2Of2 (ex:exn) -> printfn "Errors :( %A" ex.Message

let s = List.map(ss) sites 

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


// A builder

type MaybeBuilder() = 
        
    member __.Bind(value, func) = 
        match value with
        | Some value -> func value
        | None -> None
        
    member __.Return value = Some value
    member this.ReturnFrom value = this.Bind(value, this.Return)
   
module DivideExample = 
    
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
  let maybe = MaybeBuilder()

  let divisionM a b c d = maybe { let! x = divide a b
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
            let x = 42
            let! y = Some 2016
            let! z = Some 2
            return x + y + z
        }
    
    let desugared = 
        maybe.Delay(fun () ->
            let x = 42
            maybe.Bind(Some 2016, fun y ->
                maybe.Bind(Some 2, fun z ->
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