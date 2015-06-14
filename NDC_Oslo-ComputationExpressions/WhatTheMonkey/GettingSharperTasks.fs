namespace WhatTheMonekey

type Monoid<'a> = 
    {
        neutral : 'a
        op      : 'a -> 'a -> 'a
    }

module ``Check monoid axioms for list``=
    let listM<'l> : Monoid<List<'l>> = 
        {
            neutral = []
            op      = fun a b -> a @ b
        }

    open FsCheck.NUnit

    type T = List<int>
    let M = listM<int>
    let Z = M.neutral
    let (++) = M.op

    [<Property>]
    let `` Z is the neutral element``(v:T) =
        Z ++ v = v && v ++ Z = v

    [<Property>]
    let ``The operation is commutative``(a:T, b:T, c:T)=
        a ++( b ++ c) = (a ++ b ++ c)

module  ``Check monoid axioms for  int multiplication`` =

    let intMult : Monoid<int> = 
        {
            neutral = 1
            op = (*)
        }
    open FsCheck.NUnit

    type T = int
    let M = intMult
    let Z = M.neutral
    let (++) = M.op

    [<Property(Verbose = true)>]
    let `` Z is the neutral element``(v:T) =
        Z ++ v = v && v ++ Z = v

    [<Property>]
    let ``The operation is commutative``(a:T, b:T, c:T)=
        a ++( b ++ c) = (a ++ b ++ c)

module `` Check monoid axioms for  int addition `` =

    open FsCheck.NUnit

    let intPlus : Monoid<int> = 
        {
            neutral = 0
            op = (+)
        }

    type T = int
    let M = intPlus
    let Z = M.neutral
    let (++) = M.op

    [<Property(Verbose = true)>]
    let `` Z is the neutral element``(v:T) =
        Z ++ v = v && v ++ Z = v

    [<Property>]
    let ``The operation is commutative``(a:T, b:T, c:T)=
        a ++( b ++ c) = (a ++ b ++ c)

module ``Check monoid axioms for  bool`` =
    let binCon : Monoid<bool> =
        {
            neutral = true
            op = (&&)
        }

    let orCon : Monoid<bool> =
        {
            neutral = false
            op = (||)
        }
module ``functions``=
    let funs<'a> : Monoid<'a -> 'a> =
        {
            neutral = id
            op = (>>)
        }

        //uh oh!! :D

module ``stringys``=
    let stringies : Monoid<string> =
        {
            neutral = ""
            op = (+)
        }

module ``String is a monoid   or is it? `` =

    open FsCheck.NUnit
    open FsCheck

    type T = string
    let M = ``stringys``.stringies
    let Z = M.neutral
    let (++) = M.op

    let isNotNull (x:string) = not (x = null) 

    [<Property(Verbose = true)>]
    let `` Z is taahe neutral element``() =
        true ==> (1 = 1)

    //watch out, a string can be null!!
    [<Property(Verbose = true)>]
    let `` Z is the neutral element``(v:T) =
        (isNotNull v) ==> (Z ++ v = v && v ++ Z = v)

    [<Property>]
    let ``The operation is commutative``(a:T, b:T, c:T)=
        a ++( b ++ c) = (a ++ b ++ c)