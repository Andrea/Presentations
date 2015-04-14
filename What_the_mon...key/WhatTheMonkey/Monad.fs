namespace WhatTheMonkey

module DivideTowerResistance =
    let random = new System.Random()    
    let division a b c d=
        match b with
        | 0 -> None
        | _ -> match c with
               | 0 -> None
               | _ -> match d with
                      | 0 -> None
                      | _ -> Some (((a/b)/c)/d)

    let divide a b =
        match b with
        | 0 -> None
        | _ -> Some (a/b )
    
    type MaybeBuilder() =
        member __.Bind(value, func) =
            match value with
            | Some value -> func value
            | None -> None
        member __.Return value = Some value
        member this.ReturnFrom value = this.Bind(value, this.Return)

    let maybe  = MaybeBuilder()

    let divideM a b c d=
        maybe{
            let! x = divide a b
            let! y = divide x c
            let! z = divide y d
            return z
        }

    let sumSpecial a b = 
        match random.Next 10 with
        | z when z > 5 -> None
        | _ -> Some a + b 

   

module CalculateResistance =

    type Result = Success of float | DivByZero

    let divide x y = 
        match y with
        | 0.0 -> DivByZero
        | _ -> Success (x/y)

    type DefineBuilder () = 
        member this.Bind(x:Result, rest:(float -> Result)) =
            match x with
            | Success(x) -> rest x
            | DivByZero -> DivByZero
        member this.Return (x:'a) = x

    let definer = DefineBuilder()

    let totalResistance r1 r2 r3 =
        definer {
                    let! x = divide 1.0 r1
                    let! y = divide 1.0 r2
                    let! z = divide 1.0 r3
                    return divide 1.0 (x+y+z)
                }
module GameObjectChildren =

    
    



    