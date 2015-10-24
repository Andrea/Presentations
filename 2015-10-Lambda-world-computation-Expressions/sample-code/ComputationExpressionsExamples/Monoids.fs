module MonoidsExample

open FsCheck.NUnit
open System

type Colour = 
    { r : byte
      g : byte
      b : byte
      a : byte }

let addTwo c1 c2 = 
    { r = c1.r + c2.r
      g = c1.g + c2.g
      b = c1.b + c2.b
      a = c1.a + c2.a }

let addColours (colours : Colour list) = 
    let mutable res = 
        { r = 0uy
          g = 0uy
          b = 0uy
          a = 0uy }
    for i in colours do
        res <- addTwo res i
    res

let addColours' colours =
    colours
    |> List.reduce(addTwo)

type Monoid<'a> = 
    { neutral : 'a
      op : 'a -> 'a -> 'a }

let black = { 
      r = 0uy
      g = 0uy
      b = 0uy
      a = 0uy }

let colourAdd : Monoid<Colour> = 
    { neutral = black
      op = (addTwo) }

let c1 = { black with g = 254uy }
let c2 = { black with r = 254uy }
let l = [ c1; c2; black ] |> List.reduce (addTwo)

let (+++) = addTwo
let x = c1 +++ c2 +++ { c2 with a = 254uy }



type T = Colour

let M = colourAdd
let Z = M.neutral
let (++) = M.op

[<Property>]
let `` Z is the neutral element`` (v : T) = Z ++ v = v && v ++ Z = v

[<Property>]
let ``The operation is commutative`` (a : T, b : T, c : T) = a ++ (b ++ c) = (a ++ b ++ c)


module ``intMax with check`` = 
    open FsCheck.NUnit
    open System
    
    let intMax : Monoid<int> = 
        { neutral = Int32.MinValue
          op = (max) }
    
    type T = int
    
    let M = intMax
    let Z = M.neutral
    let (++) = M.op
    
    [<Property>]
    let `` Z is the neutral element`` (v : T) = Z ++ v = v && v ++ Z = v
    
    [<Property>]
    let ``The operation is commutative`` (a : T, b : T, c : T) = a ++ (b ++ c) = (a ++ b ++ c)


module MonoidComputationExpression =
    
    type Colour = {
      r : byte
      g : byte
      b : byte
      a : byte }

    let addTwo c1 c2 = 
        { r = c1.r + c2.r
          g = c1.g + c2.g
          b = c1.b + c2.b
          a = c1.a + c2.a }

    let black = { 
          r = 0uy
          g = 0uy
          b = 0uy
          a = 0uy }

    type MonoidBuilder ()=
        member x.Combine(a,b) = addTwo a b 
        member x.Zero() = black
        member x.Yield(s) = s
        member x.Delay f  = f() 
    let monoid = MonoidBuilder()

    let result = monoid{
            yield {r = 254uy; g=254uy; b= 33uy; a = 66uy}
            yield {r = 54uy; g=254uy; b= 33uy; a = 100uy}
        }