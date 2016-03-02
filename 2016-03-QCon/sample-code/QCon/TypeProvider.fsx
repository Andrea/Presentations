
#I __SOURCE_DIRECTORY__
#r "packages/FSharp.Data.2.2.5/lib/net40/FSharp.Data.dll"
#r "System.Core.dll"
#r "System.dll"
#r "System.Numerics.dll"
#r "System.Xml.Linq.dll"

open FSharp.Data
open System
open TypeProviderExample

//type GiphyTP = JsonProvider<"http://api.giphy.com/v1/gifs/search?q=monkey+cat&rating=pg-13&api_key=dc6zaTOxFJmzC"> /// special F# thing from Giphy
let baseUrl = "http://api.giphy.com/v1/gifs/search"
let key = "dc6zaTOxFJmzC"

let searchGif searchString size =
  let searchTerm = String.map(fun c ->  if (c = ' ' )then '+' else c ) searchString
  let query = ["api_key", key; "q", searchTerm]
  let response = Http.RequestString (baseUrl,  query)
  let giphy = GiphyTP.Parse(response)
  printfn "Status of the request %A" giphy.Meta.Status
  giphy.Data
  |> Array.map( fun x -> Uri(x.Images.DownsizedMedium.Url))    // find by size


searchGif "lol monkeys"
Array.head (searchGif "otter")

// show image somehow

