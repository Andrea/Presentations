﻿module MonoidsExample

open FsCheck

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

let neutral = { 
      r = 0uy
      g = 0uy
      b = 0uy
      a = 0uy }


let c1 = { neutral with g = 254uy }
let c2 = { neutral with r = 254uy }

module AddingTwoColours =
  let addColours (colours : Colour list) = 
      let mutable res = 
          { r = 0uy
            g = 0uy
            b = 0uy
            a = 0uy }
      for i in colours do
          res <- addTwo res i
      res


  type Monoid<'a> = 
      { neutral : 'a
        op : 'a -> 'a -> 'a }


  let l = [ c1; c2; neutral ] |> List.reduce (addTwo)

  let (+++) = addTwo
  let x = c1 +++ c2 +++ { c2 with a = 254uy }

  type TColour = Colour
  let colourAdd : Monoid<Colour> = 
    { neutral = neutral
      op = (addTwo) }

  let M = colourAdd
  let Z = M.neutral
  let (++) = M.op

  let `` Z is the neutral element`` (v : TColour) = Z ++ v = v && v ++ Z = v

  let ``The operation is commutative`` (a : TColour, b : TColour, c : TColour) = a ++ (b ++ c) = (a ++ b ++ c)

  Check.Quick `` Z is the neutral element`` 
  Check.Quick ``The operation is commutative``

module MonoidsWithComputationExpressions = 

  type MonoidBuilder ()= 
    member this.Zero() = neutral

    member this.Combine(x, y) = addTwo x  y
    
    member x.For(sequence, body) =
        let combine a b = x.Combine(a, body b)
        let Z = x.Zero()
        Seq.fold combine Z sequence
    member x.Yield (a) = a


  let monoid = new MonoidBuilder()

  let monAdd xs= monoid {
       for x in xs do
         yield x
       }
  
  
  let ``Adding colours in reverse result in the same colour``(xs : Colour list) = 
    let sxs = List.ofSeq xs
    monAdd sxs = monAdd (List.rev sxs)
  
  Check.Quick ``Adding colours in reverse result in the same colour``

  

module ``intMax with check`` = 
  open FsCheck
  open System
  open AddingTwoColours
    
  let intMax : Monoid<int> = 
      { neutral = Int32.MinValue
        op = (max) }
    
  type T = int
    
  let M = intMax
  let Z = M.neutral
  let (++) = M.op
    
    
  let `` Z is the neutral element`` (v : T) = Z ++ v = v && v ++ Z = v
    
  let ``The operation is commutative`` (a : T, b : T, c : T) = a ++ (b ++ c) = (a ++ b ++ c)
