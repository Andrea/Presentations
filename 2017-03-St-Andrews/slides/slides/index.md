- title : Unfrying your brain with F\#
- description : A talk about some more advanced features of F\#
- author : Andrea Magnorsky
- theme : solarized
- transition : default


***
- data-background : images/egg.png
- data-background-size : 1500px

## Unfrying your brain with F\#

###  March 2017

***
- data-background : images/logo.png
- data-background-size: 600px

## Enterprise solutions (HR, Financials and planning)

***
- data-background : images/onikira-poster.png


## Killing demons for fun and profit
***
- data-background : images/oni1.png

' portion of the game in F#
' we started uptaking in build, tests
' and then scripting

***
- data-background : images/oni2.jpg


***

## Obvious code is good code.

' don't make it about familiarity
' easy to consume F# code from C# ( even F# specific features)
' known way to deal with the paradigm mismatch


***
## F# is a general purpose language.

' cross platofrm
' Functional first
' open source
' great libraries created by the comunity 

***

![egg](http://www.publicdomainpictures.net/pictures/10000/nahled/egg-871282749217Q6v0.jpg)

***
## Interop


```fsharp
type Order =
  | GoldOrder
  | PlatinumOrder of string

  member this.OrderInfo =
    match this with
    | GoldOrder -> ""
    | PlatinumOrder(extraInfo ) -> "A foamy latte"

```    

***
## Pattern matching

    let openFile (filePath) =
      match filePath with
      | path when
          Path.GetExtension(path) = ".txt" ||
          Path.GetExtension(path) = ".md" ->
                      openText path
      | path when
          Path.GetExtension(path) = ".jpg" ||
          Path.GetExtension(path) = ".png" ||
          Path.GetExtension(path) = ".gif" -> openText path

      | _ -> "oh noes"

---

## Too many when guards

![](http://cdn.abclocal.go.com/content/creativecontent/images/cms/831727_630x354.jpg)

---

### Active patterns

' pattern matching -> against literal values
' AP -> less when guards, match agains elements of a string | collection

    let (|Extension|) (path: string) =
       Path.GetExtension <| path.ToLower()

    let openFile' path =
     match path with
     | Extension ".png"
     | Extension ".jpg"
     | Extension ".gif" -> openPictures path
     | Extension ".txt"   
     | Extension ".md" -> openText path
     | _ -> "oh noes"


---
## (| Bannana Operator |)

![banana](https://s-media-cache-ak0.pinimg.com/564x/b9/5f/3f/b95f3f0446635cb37f6022ee3b6bddaf.jpg)


---
## Active patterns


* Use them outside of a match expression
* Pass parameters
* Nest them and combine them
* Should not be expensive or cause side effects.

' Active patterns without pattern matching, because every let binding and parameter
' is a pattern match

***

![cat-birthday](images/cat-birthday.jpg)


***

## Type Providers

![babel](https://upload.wikimedia.org/wikipedia/commons/thumb/f/fc/Pieter_Bruegel_the_Elder_-_The_Tower_of_Babel_(Vienna)_-_Google_Art_Project_-_edited.jpg/800px-Pieter_Bruegel_the_Elder_-_The_Tower_of_Babel_(Vienna)_-_Google_Art_Project_-_edited.jpg)

' does not contain any types itself; it is a component for generating
' descriptions of types, methods and their implementations.
' Get data from diverse sources and generate types for them.
' Use type providers to:
'      * Write to databases
'      * Run other languages like R or python
'      * Choose your own adventure
' You might be thinking, well I can codegen that however type providers provider a simpler
'  process, (less potential errors, no extra tools). They also tend to scale better (think of ' something huge like Freebase)

---


    
    type GiphyTP = JsonProvider<"http://api.giphy.com/API/link">
    let query = ["api_key", key; "q", searchTerm]
    let response = Http.RequestString (baseUrl,  query)
    
    let giphy = GiphyTP.Parse(response)



***

## Asynchronous Workflows

' Asynchronous? Is something that started, and will execute on the background and terminate later
' Can use tasks

' No callbacks :D

![die-waiting](images/waiting.gif)

' Inspired async await in C#
' Great for IO bound operations (not cpu bound, use TPL lib)
' Avoid blocking threads
' Cancellation is easier

---

    let getHtml(url:string) =  
      let req = WebRequest.Create url

      let response = req.GetResponse()
      use stream = response.GetResponseStream()
      use reader = new StreamReader(streatm)
      
      reader.ReadToEnd().Length

---

    let getHtmlA(url:string) =  
      async{
          let req = WebRequest.Create url
          let! response = req.AsyncGetResponse() // ding!
          use stream = response.GetResponseStream()
          use reader = new StreamReader(streatm)
          return reader.ReadToEndAsync().Length // ding!
          }

---

    sites
    |> List.map (getHtmlAsync)
    |> Async.Parallel
    |> Async.RunSynchronously


***

## Computation expressions

' Perfect for some heavy lifting behind the scenes

---

##Familiar 

![](https://theowlunderground.files.wordpress.com/2013/04/jean-luc-picard-jean-luc-picard-21977733-694-530.jpg)

---

![](images/locutus.jpg)
' let! do! all operation with bang are implemented using CPS. Declared in a `Builder` (A builder is a type that has certain methods implemented)

---

![shapes](images/shapes.gif)

' Think of the importance of the signature of the functions Bind, Return, etc.
' Given the signature of the functions, we can combine them

---

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

--- 

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


***

## Compiler Serivices

![hot-swap](http://www.storagereview.com/images/QNAP-TS-879-Pro-Hot-Swap.jpg)
' Slow monolith? why not use a plug in architecture and compile code on the fly

***


### Separate how to deal with data, from what the data does

---

### Enjoy dynamic like features with type safety
' Feels like a scripting language 

---

### Ease your way into asynchronous code

---

### When you need to do something dificult show the right patterns with familiar idioms
' Tame difficulty of some code, give it an easy to understand usage.

---


' Attack big projects with compiler services 

## Make easy things easy, and dificult things possible


***

![thanks](images/otter-laughing.jpg)

- @SilverSpoon
- [roundcrisis.com](roundcrisis.com)

### https://github.com/Andrea/UnfryingYourBrain

***

### Resources

* [Extensible Pattern Matching Via a Lightweight Language Extension](http://blogs.msdn.com/b/dsyme/archive/2007/04/07/draft-paper-on-f-active-patterns.aspx)
* [Active Patterns Series: Pattern Matching- Richard Dalton](http://www.devjoy.com/series/active-patterns/)
* [Interesting active patterns - Luke ](http://luketopia.net/2014/09/11/interesting-active-patterns/)
* [Using F# active patterns with Linq](http://langexplr.blogspot.ie/2007/05/using-f-active-patterns-with-linq.html)
* [Denatured proteins rescued by trio of chaperones](http://www.uchospitals.edu/news/1998/19980710-hsp104.html)
* [F# usage survey](https://docs.google.com/forms/d/1Ly_W1ZUH3x_ph4H6I_64uvEib2brDx34j-FoaZkeYLI/viewanalytics)


<script>
  (function(i,s,o,g,r,a,m){i['GoogleAnalyticsObject']=r;i[r]=i[r]||function(){
  (i[r].q=i[r].q||[]).push(arguments)},i[r].l=1*new Date();a=s.createElement(o),
  m=s.getElementsByTagName(o)[0];a.async=1;a.src=g;m.parentNode.insertBefore(a,m)
  })(window,document,'script','//www.google-analytics.com/analytics.js','ga');

  ga('create', 'UA-46761189-1', 'auto');
  ga('send', 'pageview');

</script>
