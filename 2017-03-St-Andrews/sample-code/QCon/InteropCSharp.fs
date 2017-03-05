module InteropCSharp

open System

let (parsed, number) = Int32.TryParse("3")


// using interfaces from .net

type ICanBeBoring =
  abstract member Boringify: int -> int -> int

type BoringDisposableThing() = 
  interface ICanBeBoring with
    member this.Boringify a b = a * b * a
  interface IDisposable with 
    member this.Dispose() =
      printfn "I'll be disposed"

let aBoringThing = new BoringDisposableThing()
// can't do this -> aBoringThing.Boringify()
let boring = aBoringThing :> ICanBeBoring 
boring.Boringify 3 5 |> printfn "%i"