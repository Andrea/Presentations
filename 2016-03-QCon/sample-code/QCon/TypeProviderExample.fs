module TypeProviderGiphy
open FSharp.Data

let baseUrl = "http://api.giphy.com/v1/gifs/search"
let key = "dc6zaTOxFJmzC"

type GiphyTP = JsonProvider<"http://api.giphy.com/v1/gifs/search?q=monkey+cat&rating=pg-13&api_key=dc6zaTOxFJmzC">
