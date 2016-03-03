[<AutoOpen>]
module Utils
open System.Diagnostics
open System


let ShowGif (uri: Uri) =
  let sInfo = new ProcessStartInfo(uri.AbsoluteUri )
  Process.Start(sInfo) 

let ShowGifUrl (url: string) = ShowGif <| Uri(url)