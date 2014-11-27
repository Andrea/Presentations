- title : F# and open source
- description : Introduction to FsReveal
- author : Andrea Magnorsky
- theme : solarized
- transition : default

***
## F# and Open Source

### Andrea Magnorsky

Digital Furnace Games ␣ ▀ ␣ BatCat Games ␣ ▀ ␣ GameCraft Foundation

- @SilverSpoon 
- [roundcrisis.com](roundcrisis.com)

---

#### Working on OniKira: Demon Killer 

Available on Steam Early Access




<iframe width="750" height="500" src="//www.youtube.com/embed/sEPPbZdKBzM" frameborder="0" allowfullscreen></iframe>

---
***

### F# and Open Source
####

- Open sourced 4 years ago. [Announced by Don Syme](http://blogs.msdn.com/b/dsyme/archive/2010/11/04/announcing-the-f-compiler-library-source-code-drop.aspx)
- F# Software Foundation at [fsharp.org](http://fsharp.org/)
- User groups growth 
- 100+ pull requests for F# 4.0

---
###
<iframe width="1024" height="768" src="//fsharp.org" frameborder="0" allowfullscreen></iframe>
---
***

### Big wins!

- Great for language adoption
- Visual Studio community with extensions
- CLR in a package FTW

***

### Why you should care?

#### F# (with tooltips)

    let a = 5
    let factorial x = [1..x] |> List.reduce (*)
    let c = factorial a

---

#### C#

    [lang=cs]
    using System;

    class Program
    {
        static void Main()
        {
            Console.WriteLine("Hello, world!");
        }
    }

---

#### JavaScript

    [lang=js]
    function copyWithEvaluation(iElem, elem) {
        return function (obj) {
            var newObj = {};
            for (var p in obj) {
                var v = obj[p];
                if (typeof v === "function") {
                    v = v(iElem, elem);
                }
                newObj[p] = v;
            }
            if (!newObj.exactTiming) {
                newObj.delay += exports._libraryDelay;
            }
            return newObj;
        };
    }


---

#### Haskell
 
    [lang=haskell]
    recur_count k = 1 : 1 : zipWith recurAdd (recur_count k) (tail (recur_count k))
            where recurAdd x y = k * x + y

    main = do
      argv <- getArgs
      inputFile <- openFile (head argv) ReadMode
      line <- hGetLine inputFile
      let [n,k] = map read (words line)
      printf "%d\n" ((recur_count k) !! (n-1))

*code from [NashFP/rosalind](https://github.com/NashFP/rosalind/blob/master/mark_wutka%2Bhaskell/FIB/fib_ziplist.hs)*

---

### SQL

    [lang=sql]
    select *
    from
    (select 1 as Id union all select 2 union all select 3) as X
    where Id in (@Ids1, @Ids2, @Ids3)

*sql from [Dapper](https://code.google.com/p/dapper-dot-net/)*

***

** F is for Fun

$ \Pr(A|B)=\frac{\Pr(B|A)\Pr(A)}{\Pr(B|A)\Pr(A)+\Pr(B|\neg A)\Pr(\neg A)} $

***
** F# is for Fun

$ \Pr(A|B)=\frac{\Pr(B|A)\Pr(A)}{\Pr(B|A)\Pr(A)+\Pr(B|\neg A)\Pr(\neg A)} $

***

### The Reality of a Developer's Life 

**When I show my boss that I've fixed a bug:**
  
![When I show my boss that I've fixed a bug](http://www.topito.com/wp-content/uploads/2013/01/code-07.gif)
  
**When your regular expression returns what you expect:**
  
![When your regular expression returns what you expect](http://www.topito.com/wp-content/uploads/2013/01/code-03.gif)
  
*from [The Reality of a Developer's Life - in GIFs, Of Course](http://server.dzone.com/articles/reality-developers-life-gifs)*

