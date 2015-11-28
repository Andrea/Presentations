namespace WhatTheMonekey
open FsCheck

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
    

    type T = List<int>
    let M = listM<int>
    let Z = M.neutral
    let (++) = M.op

    
    let `` Z is the neutral element``(v:T) =
        Z ++ v = v && v ++ Z = v

    
    let ``The operation is commutative``(a:T, b:T, c:T)=
        a ++( b ++ c) = (a ++ b ++ c)

module  ``Check monoid axioms for  int multiplication`` =

    let intMult : Monoid<int> = 
        {
            neutral = 1
            op = (*)
        }


    type T = int
    let M = intMult
    let Z = M.neutral
    let (++) = M.op

    
    let `` Z is the neutral element``(v:T) = Z ++ v = v && v ++ Z = v
    Check.Quick `` Z is the neutral element`` 
    
    let ``The operation is commutative``(a:T, b:T, c:T)=
        a ++( b ++ c) = (a ++ b ++ c)

module `` Check monoid axioms for  int addition `` =

    

    let intPlus : Monoid<int> = 
        {
            neutral = 0
            op = (+)
        }

    type T = int
    let M = intPlus
    let Z = M.neutral
    let (++) = M.op

    
    let `` Z is the neutral element``(v:T) =
        Z ++ v = v && v ++ Z = v

    
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

  open FsCheck

  type StringIsMonoid() =
    
    let M = ``stringys``.stringies
    let Z = M.neutral
    let (++) = M.op

    let isNotNull (x:string) = (x <> null) 
    
    member __.`` Z is taahe neutral element``() =
        true ==> (1 = 1)

   
    member __.`` Z is the neutral element``(v:string) =
        (isNotNull v) ==> (Z ++ v = v && v ++ Z = v)


    member __.``The operation is commutative``(a:string, b:string, c:string)=
        a ++( b ++ c) = (a ++ b ++ c)

  Check.QuickAll<StringIsMonoid>()