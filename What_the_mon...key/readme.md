
#Computation expression in context : a history of the otter king


I heard that computation expressions are kind of like monads, I heard that monads are like burritos or space suits and that they are pretty much magical, I also heard that they are monoids in the category of endofunctors... 
As a writer of code that all sounds awesome, and I (and I am sure you too) want to know and understand what those terms actually mean, and most importantly why we should care about them, What are their practical uses?. So, in this talk we are going to attempt to do just that. Failure is possible but the attempt is valuable. It's a lot to chew so make sure you had your tea/coffee.


-----------------

A problem

gameObject 

getChildren -> string -> gameObject list



-----------

Disclaimer : some of the content of this talk is incorrect and some pictures I show have absolutely nothing to do with what I will be talking about
so ask questions if confused ;).

I started of doing what I assume most of us do when I heard first about monads, I went and read a couple of posts and learned absolutely nothing about it. 
Interestingly, the first person to describe monads was Eugenio Moggi, he published a  paper called ["Notions of computation and monads"](http://www.disi.unige.it/person/MoggiE/ftp/ic91.pdf) this paper is maths heavy tho I am sure some of you will be interested in looking at this.

I do remember tho that this other person, Philip Wadler

![wadler](https://dreamsongs.com/OOPSLA2007/Photos/Impressions%20Pix/wadler.gif)
`source : https://dreamsongs.com/OOPSLA2007/Impressions.html

there was an interesting common pattern, he looked at error handling, state (in a pure language) and generating output and he made a link between that common pattern and category theory.. and he kept working at it and realized a bunch of other things, like for example that there are these three rules that this patter have in common...



![Moggi](http://en.wikipedia.org/wiki/Eugenio_Moggi#/media/File:Eugenio_Moggi.jpg)

So I read more things and still I knew I didn't get it, but the good news is that Haskell syntax did look less daunting and *bind* and *return* were things that needed to be there for monads to be a thing, or was it, this familiarity helped a bit when I returned to the papers [Comprehending Monads][1] and [Monads for functional programming][2] the funny thing is that I was looking forward to understand monads, surely it feels like this

![yes](http://i.imgur.com/rGkpZ5U.jpg)
![yes](http://i.imgur.com/HhEDPde.jpg)

![super-squirrel](http://data1.blog.de/blog/c/cheer-up/img/Super-Squirrel_01.jpg)

### Monoids

In reality the thing that truly helped me understand monads was to start by trying to understand what is monoid and how they can help in your programming life.

![scared-otter](http://wereblog.com/wp-content/uploads/2014/06/otter.png)

lets say you want to add two colours, lets say those are RGBA ...


```FSharp
    type Colour = 
        {
            r: byte
            g: byte
            b: byte
            a: byte                            
        } 
    
    let neutral = { r = 0uy; g = 0uy;b = 0uy;a = 0uy}

    let add (c:Colour) (c1:Colour): Colour =        
        {
            r = c.r + c1.r
            g = c.g + c1.g
            b = c.b + c1.b
            a = c.a + c1.a
        }    

    let colourAdd : Monoid<Colour> = 
        {
            neutral = neutral
            op = (add)
        }
    
    let c1 = {neutral with g=254uy}
    let c2 = {neutral with r=254uy}
    
    let x = c1 +++ c2 +++ {c2 with a=254uy}
    let l = [c1; c2 ; neutral]
            |> List.reduce (+++)        

    
```
3 rules

* neutral value
* closure
* identity

closure is super useful because you can chain together multiple objects using the operation
identity means you can re


> I have to admit that here I took a detour and went to the amazing F# for fun and profit and the [series on Monoids][3] as well as Carsten Koning recent > posts on understanding monoids [1][http://gettingsharper.de/2015/03/03/understanding-monoids-using-f/] and [2][http://gettingsharper.de/2015/03/04/more-on-monoids-in-f-exploiting-static-constraints/] 


#### Why this concept 

This is a typical example of a concept that is general, very useful and just has terrible marketing :D, monoid can be quite a scary name for trying to work 



Because of closures-> We can convert pairwise operations into operations that work on collections
Because of associativity ->  Divide and conquer algorithms
                              Parallelization
                              Incrementalism
Because of identity -> you can actually perform certain of the above

If you have ever done map reduce, then this should make a lot of sense :) 



### Monads


No need for types
No need to understand category theory 



### Computation expressions or workflows

Computation expressions have been available in F# since 2007 and they are fully documented in the [F# language specification][]

![win](http://fc09.deviantart.net/fs71/i/2010/082/f/8/King_Otter_by_Pee_reviver.jpg)

### Resources list

* [Abstraction, intuition, and the “monad tutorial fallacy”](https://byorgey.wordpress.com/2009/01/12/abstraction-intuition-and-the-monad-tutorial-fallacy/)
* [Some Details on F# Computation Expressions](http://blogs.msdn.com/b/dsyme/archive/2007/09/22/some-details-on-f-computation-expressions-aka-monadic-or-workflow-syntax.aspx)
* [Beyond Foundations of F# - Workflows](http://www.infoq.com/articles/pickering-fsharp-workflow)
* [Monads, Arrows and idioms](http://homepages.inf.ed.ac.uk/wadler/topics/monads.html) there is a bunch of papers here.
* [Why a monad is like a writing desk](http://www.infoq.com/presentations/Why-is-a-Monad-Like-a-Writing-Desk) Video 
* [Understanding Monoids][3] F# for fun and profit 
* [Syntax Matters: Writing abstract computations in F#](http://tomasp.net/academic/papers/computation-zoo/syntax-matters.pdf) paper by Tomas Petricek and Don Syme about computation expression
* [Monads explained from a maths point of view](https://www.youtube.com/watch?v=9fohXBj2UEI) Video
* [Comprehending Monads][1] P.Wadler paper
* [Monads for functional programming][2]P.Wadler paper


[1]:(http://ncatlab.org/nlab/files/WadlerMonads.pdf)
[2]:(http://homepages.inf.ed.ac.uk/wadler/papers/marktoberdorf/baastad.pdf)
[3]:(http://fsharpforfunandprofit.com/posts/monoids-without-tears/#series-toc)