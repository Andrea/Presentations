# Lessons learned from consuming OO frameworks with a functional language 

Consuming OO from within a Functional language - how to relieve the indigestion!

Scenario: you need to use a framework that is very OO and you are trying to use it from F#, interop works but you can see there might be nicer ways to interact with it that will save you some time.

The talk: in this talk you will see how my attempt to wrap Duality (a game engine). What problems were encountered, what were interesting solutions, what was worth it, what wasn't.

Extras: this talk might include a montage of me working on the laptop, looking worried, then having epiphanies, then flipping tables and finally arriving to a solution.

Maybe bonus: there might be some 80s music for ambiance (no promises, ABBA or some other equally cheesy replacement on the table too).


## Context for the problem

This whole talk is about wrapping [Duality](http://duality.adamslair.net/) and in particular it's use from our own [Duality scripting plugin][1] in this [link](https://github.com/AdamsLair/duality/wiki/Getting-Started) is a tutorial about some common concepts of Duality in particular but also . The plugin allows you to write code that will run in the context of a GameObject. 

![Duality](/images/duality1.jpg)

* **Game components** when they change, we need to compile the code and this will restart the editor. Used for all systems and some low level gameplay.  
* **Scripts** they are automatically compiled when they are saved by the game engine via the [scripting plugin][1]. These are preferred on gameplay for faster feedback.
You can expose properties to the editor with both. 

Here you can see how a gameObject has a ScriptComponent that points to a script. The script exposes a property called `EmptyRefType that has a dropdown because the type of that property is an enum

![Duality](/images/duality2.jpg)

When you create a new script, this will generate a ScriptResource that will contain a csharp or fsharp script (depending on the one you choose). The template simply shows a default implementation, it looks something like this

``` FSharp
namespace Dualityscript

open ScriptingPlugin
open Duality

    type FSharpScript() =
        inherit DualityScript()
        
            override this.Update () =
                Log.Editor.Write "updated"
```


Ok, so now you understand (hopefully) how we work. Or if you have  questions please ask :D (twitter/PR whatever you feel more comfortable)

## Problems

So what are the problems with this approach? I mean, you can write F# right? well...

* One type per script: the way this works is that we implement the DualityScript type, and override available methods . Is this the correct approach? why not just be able to use functions? or modules?. 

* The code we write is dealing directly with an OO style framework, however there are some problems inherent to C#, ie there is still a lot of null checking. It would be nice to deal with this repetitive code in a more functional way. 

``` FSharp

  match cp, cp.GetComponent<Transform>() with
  | null, _ -> Log.Editor.Write("Could not create an instance of ControlPrefab {0}", this.ControlPrefab)
  | _, null -> Log.Editor.Write("The ControlPrefab {0} doesn't have a transform", this.ControlPrefab)
  | _, t ->                     
      t.Pos <- // actually do something

```

* The properties that are exposed are mutable

## Solutions



### Nulls

So the source of most of the pain as a user is the null checks, so I am obviously starting there.

Ideal

* Checks every gameObject for null
* Checks GetComponent (all 4 variations)
* Checks GetChildren (all  variations)
* Checks GetGameObject (all  variations)

so you can do

``` FSharp

let s = cp.GetComponent<Transform>()


```


## Research

Are there any papers or articles about this? I found the following

* [Crossing State Lines: Adapting Object-Oriented Frameworks to Functional Reactive Languages](http://cs.brown.edu/~sk/Publications/Papers/Published/ick-adapt-oo-fwk-frp/)

Are there any other libraries that successfully did this?

* Akka.net 
* FAKE?

Other sources:

* [](http://blog.geist.no/a-more-functionally-idiomatic-approach-to-akka-net/)
* ST style monads, generative functors

## Difficulties

* Trying to think in both OO and functional ways when switching contexts was pretty hard, keep going the wrong way
* It is hard to find out where is a good boundary 


[1]:https://github.com/BraveSirAndrew/DualityScripting