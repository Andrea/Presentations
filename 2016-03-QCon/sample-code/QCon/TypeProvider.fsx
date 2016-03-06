
#I __SOURCE_DIRECTORY__
#r "packages/FSharp.Data.2.2.5/lib/net40/FSharp.Data.dll"
#r "System.Core.dll"
#r "System.dll"
#r "System.Numerics.dll"
#r "System.Xml.Linq.dll"

#load "TypeProviderExample.fs"
#load "Utils.fs"

open FSharp.Data
open System
open TypeProviderGiphy

let searchString = "flying cat"

let searchGif searchString =
  let searchTerm = String.map(fun c ->  if (c = ' ' )then '+' else c ) searchString

  let query = ["api_key", key; "q", searchTerm]

  let response = Http.RequestString (baseUrl,  query)
  let giphy = GiphyTP.Parse(response)
  
  printfn "Status of the request %A" giphy.Meta.Status
  
  giphy.Data
  |> Seq.map( fun x -> Uri(x.Images.DownsizedMedium.Url))    


searchGif "caturday" |> Seq.head |> ShowGif

// More things you can do with results
searchGif "omg otter" |> Seq.map(ShowGif)
