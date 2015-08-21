- title : F# in Onikira
- description : How we use F# in Onikira
- author : Andrea Magnorsky
- theme : solarized
- transition : default

***


## F# in Onikira

### Andrea Magnorsky

Digital Furnace Games  ▀  BatCat Games  ▀  GameCraft Foundation

- @SilverSpoon 
- [roundcrisis.com](roundcrisis.com)


---

#### Working on OniKira: Demon Killer 

Release on 27th August 2015 on Steam




<iframe width="853" height="480" src="//www.youtube.com/embed/Tt-SwXISipY?rel=0" frameborder="0" allowfullscreen></iframe>

---

#### Working on OniKira: Demon Killer 

Release on 27th August 2015 on Steam

![](images/onikira.jpg)

---


***
### Ecosystem: What we use now

* FsCheck
* Fake
* Compiler Services

***
### Property Testing with FsCheck

>Why write tests, when you can generate them

---

### What is a property?

> x + x = 2 * x 

or

> List.rev(List.rev list) = list

---
### FsCheck

* Can be used from C# 
* Small library
* Can run stand alone or integrates with NUnit and xUnit
* QuickCheck [paper](http://www.cs.tufts.edu/~nr/cs257/archive/john-hughes/quick.pdf) by Koen Claessen and John Hughes 
* Superb article by Scott Wlaschin on [Property based testing ](http://fsharpforfunandprofit.com/posts/property-based-testing/).

---


    [<Property>]
    let ``When adding x to x then result is double x``(x:int)=
        x + x = 2*x

---

    let preconditionMaxHealth maxHealth = maxHealth > 0

    [<Property(Verbose = true)>]    
    let ``Health should never be higher than max`` (x:int)(maxHealth:int)=        
        let healthComponent = initedHealth
        healthComponent.MaxHealth <- maxHealth
        healthComponent.IncreaseHealth x
        preconditionMaxHealth maxHealth ==>  (healthComponent.MaxHealth >= healthComponent.Health)

---

<img src="images/fscheck.png" alt="fs" style="width: 950px;"/>

---
***
### Fake

* Use from any .net language
* It's mature.
* Builds for .net and mono, it's cross platform.
* Integrates with CI Server.

---
### Hello F# make world!

    // include Fake lib
    #r @"tools\FAKE\tools\FakeLib.dll"
    open Fake 
    Target "Foo" (fun _ ->
        trace "Hello World from Foo"
    )
    
    Target "Bar" (fun _ -> 
        trace "Hello World from Bar"
    )
    "Bar"
      ==> "Foo"
    
    RunTargetOrDefault "Foo"

***
### Scripting games with F#
##

##Compiler.Services
    
---
## Duality.Scripting

* File watcher
* Code in any editor :)
* Compiler Services call to fsc
* Runs on Android (probably iOS too with the correct Xamarin license)
* Future -> live coding


[repo](https://github.com/BraveSirAndrew/DualityScripting)

---

<img src="images/fcs.png" alt="fs" style="width: 950px;"/>


***

## Resources
* [Fsharp.org](http://Fsharp.org)
* Fsharp Koans
* [Tryfsharp.org](http://Tryfsharp.org)
* F# for fun and profit
* Community for F# c4f# 

``Books``
* Expert F# 
* Real World Functional programming
* [More Books](http://fsharp.org/about/learning.html)

---

## Events

- Kats Conf - Dublin - 12th September 
 ![FK](images/fk.jpeg)   
- Progressive F#
- CodeMesh - 3rd 4th November ( London)


***

### Thanks :D

![onikira](images/onikira.jpg)

- @SilverSpoon 
- [roundcrisis.com](roundcrisis.com)


***
### Links

* [Pipe and function composition](http://theburningmonk.com/2011/09/fsharp-pipe-forward-and-pipe-backward/)
* [Why F#](http://fsharpforfunandprofit.com/why-use-fsharp/)
* [Why F# and FP in general](http://www.roundcrisis.com/2014/05/10/why-fsharp/)
* [F# Pattern matching for beginners](http://hestia.typepad.com/flatlander/2010/07/f-pattern-matching-for-beginners-part-6-active-patterns.html)
* [F# Component design](http://fsharp.org/specs/component-design-guidelines/fsharp-design-guidelines-v14.pdf)
* [Duality Scripting](http://www.roundcrisis.com/2014/04/21/Fsharp-scripting-for-the-game-engine/) 

