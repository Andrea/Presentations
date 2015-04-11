namespace WhatTheMonekey

type Monoid<'a> = 
    {
        neutral : 'a
        op      : 'a -> 'a -> 'a
    }

module Examples = 
    let listM<'l> : Monoid<List<'l>> = 
        {
            neutral = []
            op      = fun a b -> a @ b
        }

module ``Check monoid axioms``=

    open FsCheck.NUnit

    type T = List<int>
    let M = Examples.listM<int>
    let Z = M.neutral
    let (++) = M.op

    [<Property>]
    let `` Z is the neutral element``(v:T) =
        Z ++ v = v && v ++ Z = v

    [<Property>]
    let ``The operation is commutative``(a:T, b:T, c:T)=
        a ++( b ++ c) = (a ++ b ++ c)

module  ``check proof`` =
    
    let rec concat (ls : 'a list)(ls' : 'a list) =
        match ls with
        | [] -> ls'
        | (l::ls) -> l :: concat ls ls'
module ``more proofs`` =
    let intPlus : Monoid<int> = 
        {
            neutral = 0
            op = (+)
        }

    let intMult : Monoid<int> = 
        {
            neutral = 1
            op = (*)
        }
module `` check int mult `` =

    open FsCheck.NUnit

    type T = int
    let M = ``more proofs``.intMult
    let Z = M.neutral
    let (++) = M.op

    [<Property(Verbose = true)>]
    let `` Z is the neutral element``(v:T) =
        Z ++ v = v && v ++ Z = v

    [<Property>]
    let ``The operation is commutative``(a:T, b:T, c:T)=
        a ++( b ++ c) = (a ++ b ++ c)

module `` check int plus `` =

    open FsCheck.NUnit

    type T = int
    let M = ``more proofs``.intPlus
    let Z = M.neutral
    let (++) = M.op

    [<Property(Verbose = true)>]
    let `` Z is the neutral element``(v:T) =
        Z ++ v = v && v ++ Z = v

    [<Property>]
    let ``The operation is commutative``(a:T, b:T, c:T)=
        a ++( b ++ c) = (a ++ b ++ c)

module ``bool proofs`` =
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

module ``stringys``=
    let stringies : Monoid<string> =
        {
            neutral = ""
            op = (+)
        }

module `` string is a monoid `` =

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