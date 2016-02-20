- title : F# Vienna
- description : Intro to f# and trees 
- author : Andrea Magnorsky
- theme : night
- transition : default

***
- data-background : images/fsharpT.png
- data-background-size : 900px

## F# Workshop 

### Vienna |> F#

***

## Andrea Magnorsky 

Digital Furnace Games  ▀  BatCat Games  ▀  GameCraft Foundation

- @SilverSpoon 
- [roundcrisis.com](roundcrisis.com)

---

#### OniKira: Demon Killer 


<iframe width="853" height="480" src="//www.youtube.com/embed/8OH31zfRlDs?rel=0" frameborder="0" allowfullscreen></iframe>

***
- data-background: images/onikira-poster.png
 

***
## Core concepts

* Declarative language
* Functional First
* Inmutable by default
* Structural equility 

---

* white space sensitive
* install F# Power tools now if in VS (or show spaces on your editor)

---

## Let's write some code

---

## F# -> other cool things 

    open System

    let sleepWorkflow  = async{
        printfn "Starting sleep workflow at %O" DateTime.Now.TimeOfDay
        do! Async.Sleep 2000
        printfn "Finished sleep workflow at %O" DateTime.Now.TimeOfDay
        }

    Async.RunSynchronously sleepWorkflow      


---

## Cloud with MBrace

    let job =
        cloud { 
            return sprintf 
                "run in the cloud on worker '%s' " Environment.MachineName }
        |> runtime.CreateProcess

---

## Type Providers

    type data = 
    ...

***

### Today

![meh](images/tree.png)

---

## How?

    git clone https://github.com/c4fsharp/Dojo-Fractal-Forrest.git

*** 

## Tomorrow!

Want to make a game? YES
Get there at 9am
in Microsoft Vienna

![gc](images/gamecraft-logo.png)


---

### Resources 

* Practice:
    * F# Koans
    * F# tutorial
    * OSS projects
* Read:
    * F# for fun and profit
    * F# Weekly
    * Programming F#
    * Expert F# 
* Social:
    * #fsharp
    * F# Channel on functionalprogramming.slack.com

***

### Thanks :D

![onikira](images/onikira.jpg)

- @SilverSpoon 
- [roundcrisis.com](roundcrisis.com)


***