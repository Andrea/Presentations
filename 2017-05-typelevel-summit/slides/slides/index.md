- title : Inviting everyone to the party
- description : A talk about Scala and it's many paradigms
- author : Andrea Magnorsky
- theme : solarized
- transition : default


***
- data-background : images/typelevel.png
- data-background-size : 800px

<div class="fragment">
Thank you 
</div>

***
- data-background : images/animalparty.jpg

 
 
<div class="fragment">
## Inviting everyone to the party

###  June 2017
</div>



' ... in which we discuss programming paradigms from the perspective of ... ~well, me~.
' there are many programming languages less paradigms
' you see the same  problems appearing in different languges 

***

Disclaimer: 
# CONTEXT 
is really important for most of the assertions here. So please stay in context.


***
- data-background : #FF7777


' Bacon decided to learn FP, after many years of Imperative and Object Oriented programming

### This is Bacon

![Bacon](images/bacon.jpg)


---
- data-background : #FF7777

#### Delphi
<div class="fragment">
#### VB.net
</div>
<div class="fragment">
#### C#     (some Js when jQuery was new, some Java)
</div>

<div class="fragment">

## F\# / C\#  <- started learning FP woo!
</div>
<div class="fragment">
## Scala
</div>

---
- data-background : #FF7777

' <img class="plain"  src="images/bacon.jpg"/>

### Referencial transparency 
### less mutable state!!
### No exepcions for flow control!!
###.. and more



---
- data-background : #FF7777

<div class="fragment">
### Bacon's friends felt awkward
</div>

![tweet](images/bacon-tweet.png)


---
- data-background : #FF7777

## Bacon's  FP

* **Typed** FP
* FP everywhere 
* Aspiring to purity / Total functions (but not always achieving it .. why?)
* When dealing with OO aspects use a functional wrapper


' functional wrappers can work but they don't scale and sometimes the performance cost is not acceptable
' how does this evolve?

---
- data-background : #FF7777

## Problems with FP as Bacon understands it

* Dependency management
* Type tetris
* Complicated concepts 

' Are monad transformers really necessary
' surely these problems supported by the language
' that their peers are not groking (choice or not)

... is it worth it?... is it the best way?

---
- data-background : #FF7777

## Bacon dreams of well structured programs


> Well-structured software is easy to write and to debug, and provides a collection of modules that can be reused
to reduce future programming costs. (Why FP matters. John Hughes)



***
- data-background : #AAFFEE

## Meet Ooooo

<img class="plain"  src="images/ooo.jpg"/>


---
- data-background : #AAFFEE


* Works with Bacon
* Performance is everything
* Curious about functional approach
* SOLID looks FP-ish when you squint

' but ooo is an owl, do they even squint

---
- data-background : #AAFFEE

### "Functional programming has emerged since the mid-2000s as an attractive basis for software construction. One reason is the increasing importance of parallelism and distribution in computing." [Odersky, Rompf April 2014](https://cacm.acm.org/magazines/2014/4/173220-unifying-functional-and-object-oriented-programming-with-scala/fulltext)

'  Its integration of functional and object-oriented concepts leads to a scalable language, in the sense that the same concepts work well for very small, as well as very large, programs.

---
- data-background : #AAFFEE

### "...especially its(Scala) focus on pragmatic choices that unify traditionally disparate programming-language philosophies (such as object-oriented and functional programming) [Odersky, Rompf April 2014](https://cacm.acm.org/magazines/2014/4/173220-unifying-functional-and-object-oriented-programming-with-scala/fulltext)

' this is not what Oooo sees when they talk to Bacon

---
- data-background : #AAFFEE

' maybe quote ? example

## SOLID looks a lot like FP when you squint 

' example?


***

# On Scala and F#/C# 
(From it's creators)
some pic to break it up

' mention how scala and F#/C# have similar struggles


---

' And then I come to Scala, a language that int he words of Don Syme

>Scala is very much about better component oriented programming for the Java platform. Although we do a good job of object oriented programming which is very nice in F#, we haven't thought to make fundamental improvements at the component level, in a sense. We are quite happy to say "You are making components? OK, make it a .NET component". [Don Syme - March 2009](https://www.infoq.com/interviews/F-Sharp-Don-Syme)

' so, when I read this it was totally a light bulb moment
' F# is designed to be used in conjunction with C#, to do the more OCaml-ey bits Scala is supposed to replace Java


---

> "...[Scala] focus on pragmatic choices that unify traditionally disparate programming-language philosophies (such as object-oriented and functional programming). The key lesson is these philosophies need not be contradictory in practice. 

---


> Regarding functional and object-oriented programming, one fundamental choice is where to define pieces of functionality (...) ...and Scala gives programmers the choice. 

<div class="fragment">

> Choice also involves responsibility, and in many cases novice Scala programmers need guidance to develop an intuitive sense of how to structure programs effectively
[Odersky, Rompf April 2014](https://cacm.acm.org/magazines/2014/4/173220-unifying-functional-and-object-oriented-programming-with-scala/fulltext)
</div>

***
- data-background : images/fight.jpg


> When Oooo and Bacon talk, many times they disagree and call each others names

' the statements by Syme and Odersky are at odds with the reality of programmers


' The chase for best practises perhaps removed the 
' The way we see paradigms as working programmers perhaps is a little different from academics?
' There is some elements of confusion here
' 
' - FP != Typed FP
' - FP definition is loose, in the context of different languages FP means something different to each community
' - so we talk past each other and not agree
' - fomr both F#/ C# camps and within scala I see the same conflicts within the comunity
' too busy trying to figure out who is right to try to find something that even the creators of the langues (oderski and Syme 
' on camera) have been telling us for ever
' Objects at large, functional at small

***

' does it matter? we need to build stuff in the end

#### We build Systems with:

<div class="fragment">
### language(s)
</div>
<div class="fragment">
### tools: libraries, frameworks
</div>
<div class="fragment">
### context: users and community 
</div>

<div class="fragment">
### Context matters
</div>




***
- data-background : #FFFFFF

' At this point to types of questions come to mind, those related with

* Paradigms and how they interact
* Paradigms and how they shift



***

## A programming paradigm 

...is an approach to programming a computer based on a mathematical theory or a coherent set of
principles.

' lets get some proper definitions

---


>  All but the smallest toy problems require different sets of concepts for different parts. This is why programming languages should support many paradigms.

CTM - Peter Van Roy

' this agrees wit Odersky's statement and Syme statements
' object-oriented programming is best for problems with a large number of related data abstractions organized in a hierarchy

---

> A language should ideally support many concepts in a well-factored way, so that the programmer can choose the right concepts wheneverthey are needed without being encumbered by the others



' called multiparadigm programming, 
' 'in our experience it is clear that it should be the normal way of programming

---
- data-background : images/WAT.jpg

<div class="fragment">
>  ...it is certainly not true that there is one “best” paradigm
</div>

***




***
- data-background : images/kuhn.jpg


## A paradigm shifts

---
- data-background : #BBBBBB


' yes, we are haivng a funcitonal revolution, it is necesary because the lunch has been over for a while
' and many of us are actually having to deal with all this machines doing stuff at the same time and shared memory
' is not a thing that will make sense for most

' a revolution requires the knowledge and the comunity to be ready to receive this new ideas


Every langauge is adding functional features:

* function passing
* expression oriented programming
* inmutable data structures

C++, Java, C# 

' higher abstractions that are easy to grok?  yes, lambdas are coming everywhere Java, C++
' in C# linq has been a major success and this is where RX was born, not as populat but IObservable is part of the BCL 


---
- data-background : #BBBBBB

> The decision to reject one paradigm is always simultaneously the decision to accept another, and the judgment leading to that decision involves the comparison of both paradigms with nature and with each other.

<small>Kuhn, Thomas S.. The Structure of Scientific Revolutions: 50th Anniversary Edition (p. 78). University of Chicago Press. Kindle Edition. </small>

----
- data-background : #BBBBBB

> It is, I think, particularly in periods of acknowledged crisis that scientists have turned to philosophical analysis as a device for unlocking the riddles of their field. Scientists have not generally needed or wanted to be philosophers.

<small> Kuhn, Thomas S.. The Structure of Scientific Revolutions: 50th Anniversary Edition (p. 88). University of Chicago Press. Kindle Edition. 
</small>

****

# All these silver bullets

---

In the end we need to get things done

As a working professional we see/take part on

- cut corners
- religious wars
- deal with terrible code
- deal with other people's terrible code
- complain about the shortcommings of the current language we are using

***

Learning a new paradigm has been great, open the world to many more paradigms out there
easier to 
but the impetus came from me, and 
a) I still have a lot to learn
b) it's been hard sometimes
 
during this trip I have 

- told people this new thing ( I am discovering, but it's not new) is great and how this other silly thing I was using before is subpar (in my case I was comparing F# to C#)
the way that sounded is to anyone listen is
probably like I was some sort of self centered elitist ivory tower eejit 
I was truly, honestly trying to help, I felt like I should have known about thsi stuff before
and maybe there was all this other people out there , that just needed to hear about it, the way I did.  
Very soon I started seeing this in other people, and I could relate, but I also started seeing how it sounded like from outside of it



introduce OOoo and Bacon as OO background person and FP person that doesn't know OO

not only at a comunity level, also the same personal struggles, the same questions on the level of abstraction to work at .. talks about purity and lazyness and how haskell is great but idris is greater and when can we finally get a decent type system
because in the end while traversing the path to fp we had to figure

- how to deal with dependencies?
- how to interop with the other more imperative/oo side of the world (via libraries or via our own system that we are probably converting over to )
- how do we make this weird thing work



there are some interesting things happening

***

' getting things done is the important you'll just use whatever you know
' so you better know a lot of stuff so you can get it done fast so we should learn lots 
but not everyone does , not everyone learns the same things so in the end we wind up with 
strange definitions of good
this is the good we found 
abstractions levels keep raising trying to have good mental models for solutions, however 
the abstractions come , generally without the context


***
- data-background : images/battlestar.jpg
- data-background-size: 1200px


<div class="fragment">

## All this has happened before and it will happen again
</div>

***

## What is simplest?

```
var result = 0;
for (int i = 0; myNumbers.Lenght; i++ )
{
    result += myNumbers[i];
}
```

```
myNumbers |> List.sum
```
---


***
when trying to get things done there is this two things that in my opinion are
very important 

- dependecies and how to manage them
- fault tolerance / correctness




***
- data-background : images/Elm.png
- data-background-size: 1200px

...And in the beginning there was functions

WE ll 
***

brief intro with some history points


OO 

Simula 67 
- C.A.R. Hoare , Ivan Sutherland

- Alan Kay 
ideas from Lisp to create OO (hilars)



---

move to paradigms

***

> "a proliferation of compelling articulations, the willingness to try anything, the expression of explicit discontent, the recourse to philosophy and to debate over fundamentals"

*** 

1978 Turing Award lecture by Floyd

> To the designer of programming languages, I say: unless you can support the paradigms I use when I program, or at least support my extending your language
into one that does support my programming methods, I
don't need your shiny new languages; like an old car or
house, the old language has limitations that I have learned to live with

***



*** 

## Take aways

You could argue that these problems have been solved already but, they are partial solutions
I think a solution is one when everyone can play with all the toys...
everyone is invited to the party

***

### Sources | References

#### papers

* [Programming Paradigms for Dummies: What Every Programmer Should Know - Peter Van Roy](https://www.info.ucl.ac.be/~pvr/VanRoyChapter.pdf)
* [The paradigms of programming](https://pdfs.semanticscholar.org/a57d/cde5113855aec888b2a4e1fdd6e3956ce2e6.pdf)
* [The next 700 programming languages by peter landin](http://www.thecorememory.com/Next_700.pdf)
* [Why Functional Programming Matters by John Hughes](http://www.cse.chalmers.se/~rjmh/Papers/whyfp.html)
* [Joe Armstrong Thesis](http://erlang.org/download/armstrong_thesis_2003.pdf)
 
---
#### articles, posts, videos 

- [A punchcard ate my programme by Walt Mankowski](https://www.youtube.com/watch?v=PF6JEK0BpPU)
- [Clojure spec](https://clojure.org/guides/spec)
- [Lenses in F\#](http://bugsquash.blogspot.co.uk/2011/11/lenses-in-f.html)
- [F# Don Syme](https://www.infoq.com/interviews/F-Sharp-Don-Syme)
- [Programming paradigm](https://en.wikipedia.org/wiki/Programming_paradigm)
- [The expression problem](https://en.wikipedia.org/wiki/Expression_problem)

---

#### Images
- Animal party [link](https://commons.wikimedia.org/wiki/File:Animal_Party.jpg)
- Tea ceremony japan [link](https://commons.wikimedia.org/wiki/Commons:Featured_pictures#/media/File:Japan_tea_ceremony_1165.jpg)