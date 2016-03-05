[<AutoOpen>]
module Utils
open System.Diagnostics
open System


let ShowGif (uri: Uri) =
  let sInfo = new ProcessStartInfo(uri.AbsoluteUri )
  Process.Start(sInfo)
  ()

let ShowGifUrl (url: string) = ShowGif <| Uri(url)

let ShowCelebration() = ShowGifUrl "http://i.giphy.com/fHiz7HAUlSaIg.gif"
let ShowFailure() = ShowGifUrl "http://giphy.com/gifs/panda-num-nums-ZeB4HcMpsyDo4/fullscreen"

ShowFailure()