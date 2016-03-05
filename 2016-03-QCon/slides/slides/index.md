- title : Unfrying your brain with F\#
- description : A talk about some more advanced features of F\#
- author : Andrea Magnorsky
- theme : solarized
- transition : default

***
- data-background : images/roundcrisis.jpg
- data-background-size : 800px


' thanks for coming to my talk :D
' thanks to Amanda and QCon for inviting me

***
- data-background : images/egg.jpg
- data-background-size : 1500px

## Unfrying your brain with F\#

### QCon London - March 2016

***
- data-background : images/onikira-poster.png


## Killing demons for fun and profit

***
- data-background : images/oni1.png

***
- data-background : images/oni2.jpg


***

## Obvious code is good code.

' don't make it about familiarity

' it is easy to consume F# code from C# ( even F# specific features)
' there are very simple ways to deal with the


***
## F# is a general purpose language.

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


'    open System.IO
'    let openPictures path = sprintf "gimp.exe %s" path
'    let openText path = sprintf "emacs.exe %s" path

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
## Neat trick

    let (|Int|) v =   
      match Int32.TryParse v with
      | true, r ->  r
      | false,_ ->  0

    let contrivedAdd (Int c) = c + 5


' Active patterns without pattern matching, because every let binding and parameter
' is a pattern match


---
## (| Bannana Operator |)

![banana](https://s-media-cache-ak0.pinimg.com/564x/b9/5f/3f/b95f3f0446635cb37f6022ee3b6bddaf.jpg)


***
- data-background
- data-background-size : 800px
![1](http://i.imgur.com/7yyj1r2.jpg)


***

## Computation expressions

* problems it solves
* async | mbrace

***
## Monoids?


' understand monoids, the reason for that is that they made me think about program flow  
' why bother?

* Convert pairwise operations into work in collections
* Parallelization and Incrementalism

---

## Monoids

* Closures  $  a' \rightarrow  a' \rightarrow  a'  $ (example  int -> int -> int   )
* Identity   $ x + I  = x $
* Associativity  $ x + (y + z) = (x + y ) + z $

---

    type Colour = { r: byte; g: byte; b: byte; a: byte }

    let addTwo c1 c2 = {
        r = c1.r + c2.r
        g = c1.g + c2.g
        b = c1.b + c2.b
        a = c1.a + c2.a
    }
---

    type Monoid<'a> =
        { neutral : 'a
          op : 'a -> 'a -> 'a }

---


* understanding monoids. why?


***
## Type providers

what they do

***

sample xml

***

sample R

***

how they work

***
## Lessons learned

* Language features are great, the true power of F# as a developer best friend becomes evident in the space between
absolute purity and OO, closer to FP
* F#

***

### Thanks :D

![thanks](images/otter-laughing.jpg)

- @SilverSpoon
- [roundcrisis.com](roundcrisis.com)

***

## Events and User Groups

![fk](images/fk.jpeg)

* [Functional Kats](http://www.meetup.com/nyc-fsharp/)
* [F#unctional Londoners meetup group](http://www.meetup.com/FSharpLondon/)
* Other user groups about programming languages that have no cats with capes on their logos :D

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
