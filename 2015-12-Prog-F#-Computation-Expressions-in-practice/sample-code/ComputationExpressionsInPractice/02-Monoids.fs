namespace ComputationExpressionsInPractice

open FsCheck
open System

type Monoid<'a> = 
    { Neutral : 'a
      Operation : 'a -> 'a -> 'a }
module MonoidsExample =

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

  let neutral = { 
        r = 0uy
        g = 0uy
        b = 0uy
        a = 0uy }

  let colourAdd : Monoid<Colour> = 
      { Neutral = neutral
        Operation = (addTwo) }

  let c1 = { neutral with g = 254uy }
  let c2 = { neutral with r = 254uy }
  let l = [ c1; c2; neutral ] |> List.reduce (addTwo)

  let (+++) = addTwo
  let x = c1 +++ c2 +++ { c2 with a = 254uy }

  type T = Colour

  let M = colourAdd
  let Z = M.Neutral
  let (++) = M.Operation

  (* FsCheck tests *)

  let `` Z is the neutral element`` (v : T) = Z ++ v = v && v ++ Z = v

  let ``The operation is commutative`` (a : T, b : T, c : T) = a ++ (b ++ c) = (a ++ b ++ c)

  Check.Quick `` Z is the neutral element``
  Check.Quick ``The operation is commutative`` 

module ``intMax with check`` =
    
    let intMax : Monoid<int> = 
        { Neutral = Int32.MinValue
          Operation = (max) }
    
    type T = int
    
    let M = intMax
    let Z = M.Neutral
    let (++) = M.Operation
        
    let `` Z is the neutral element`` (v : T) = Z ++ v = v && v ++ Z = v
    
    let ``The operation is commutative`` (a : T, b : T, c : T) = a ++ (b ++ c) = (a ++ b ++ c)

    Check.Quick `` Z is the neutral element``
    Check.Quick ``The operation is commutative`` 

module MonoidsWithComputationExpressions =

  