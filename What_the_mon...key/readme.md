
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

I started of doing what I assume most of us do when I heard first about monads, I went and search and learned absolutely nothing about it in the hour or so I spent reading about it. I do remember tho that this person, Philip Wadler 

![wadler](https://dreamsongs.com/OOPSLA2007/Photos/Impressions%20Pix/wadler.gif)

realized there was an interesting common pattern, he looked at error handling, state (in a pure language) and generating output and he made a link between that common pattern and category theory.. and he kept working at it and realized a bunch of other things, like for example that there are these three rules that this patter have in common...

So I read more things and still I knew I didn't get it, but the good news is that Haskell syntax did look less daunting and *bind* and *return* were things that needed to be there for monads to be a thing, or was it, this familiarity helped a bit when I returned to the papers [Comprehending Monads][1] and [Monads for functional programming][2] the funny thing is that I was looking forward to understand monads, surely it feels like this

![yes](http://i.imgur.com/rGkpZ5U.jpg)
![yes](http://i.imgur.com/HhEDPde.jpg)

![super-squirrel](http://data1.blog.de/blog/c/cheer-up/img/Super-Squirrel_01.jpg)

### Monoids

In reality the thing that truly helped me understand monads was to start by trying to understand what is monoid and why you should care.

lets say you want to add two colours, lets say those are RGBA ...

```Fhsarp

    let intMax : Monoid<int> = 
        {
            neutral = Int32.MinValue
            op = (max)
        }

    type T = int
    let M = intMax
    let Z = M.neutral
    let (++) = M.op
```

I have to admit that here I took a detour and went to the amazing F# for fun and profit and the [series on Monoids][3] as well as Carsten Koning recent posts on understanding monoids [1][http://gettingsharper.de/2015/03/03/understanding-monoids-using-f/] and [2][http://gettingsharper.de/2015/03/04/more-on-monoids-in-f-exploiting-static-constraints/] 




#### Examples 

type Monoid<'a> = 
    {
        neutral : 'a
        op      : 'a -> 'a -> 'a
    }


#### Why this concept 

* you can chain together multiple objects using the operation.

This is a typical example of a concept that is general, very useful and just has terrible marketing :D, monoid can be quite a scary name for trying to work 

![scared-otter](http://wereblog.com/wp-content/uploads/2014/06/otter.png)

Don't worry, lets see some examples





Another thing I read is that most of us constantly use monads (and we didn't even know it!!) 





// need to mention 3 laws
// bind  retrun wtf?


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

[1]:(http://ncatlab.org/nlab/files/WadlerMonads.pdf)
[2]:(http://homepages.inf.ed.ac.uk/wadler/papers/marktoberdorf/baastad.pdf)
[3]:(http://fsharpforfunandprofit.com/posts/monoids-without-tears/#series-toc)