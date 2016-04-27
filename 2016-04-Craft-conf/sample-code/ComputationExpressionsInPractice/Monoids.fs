namespace ComputationExpressionsInPractice
open FsCheck

module MonoidSetup =
  type Colour = 
      { r : byte
        g : byte
        b : byte
        a : byte }

  let addColour c1 c2 = 
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
  open MonoidSetup

  let addColours (colours : Colour list) = 
      let mutable res = 
          { r = 0uy
            g = 0uy
            b = 0uy
            a = 0uy }
      for i in colours do
          res <- addColour res i
      res


  type Monoid<'a> = 
      { neutral : 'a
        op : 'a -> 'a -> 'a }


  let l = [ c1; c2; neutral ] |> List.reduce (addColour)

  let (+++) = addColour
  let x = c1 +++ c2 +++ { c2 with a = 254uy }

  type TColour = Colour
  let colourAdd : Monoid<Colour> = 
    { neutral = neutral
      op = (addColour) }

  let M = colourAdd
  let Z = M.neutral
  let (++) = M.op

  let `` Z is the neutral element`` (v : TColour) = 
    Z ++ v = v && v ++ Z = v

  let ``The operation is commutative`` (a : TColour, b : TColour, c : TColour) = 
    a ++ (b ++ c) = (a ++ b ++ c)

  Check.Quick `` Z is the neutral element`` 
  Check.Quick ``The operation is commutative``

module MonoidsWithComputationExpressions = 
  open MonoidSetup

  type MonoidBuilder ()= 
    member this.Zero() = neutral

    member this.Combine(x, y) = addColour x  y
    
    member x.For(sequence, f) =
        let combine a b = x.Combine(a, f b)
        let Z = x.Zero()
        Seq.fold combine Z sequence

    member x.Yield (a) = a


  let monoid = new MonoidBuilder()

  let monoidAdd xs= monoid {
       for x in xs do
         yield x
       }
  
  let ``Adding colours in reverse result in the same colour``(xs : Colour list) = 
    let sxs = List.ofSeq xs
    monoidAdd sxs = monoidAdd (List.rev sxs)
  
  Check.Quick ``Adding colours in reverse result in the same colour``

 