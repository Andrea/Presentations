
module Monad

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

type GameObject = string
type Result = Success of GameObject | DivByZero

let divide x y = 
    match y with
    | 0.0 -> DivByZero
    | _ -> Success (x/y)
