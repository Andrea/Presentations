module ComputationExpression

let downloader url = 
    async { 
        let! html = downloadAsync(url)
        return html.Length 
    }