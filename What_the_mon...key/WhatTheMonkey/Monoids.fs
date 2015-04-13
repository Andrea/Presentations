namespace MonoidsExamples

type Monoid<'a> = 
    {
        neutral : 'a
        op      : 'a -> 'a -> 'a
    }

module ``Colour adding with check``=

    open FsCheck.NUnit
    open System
    
    type Colour = 
        {
            r: byte
            g: byte
            b: byte
            a: byte                            
        } 
    let neutral = { r = 0uy; g = 0uy;b = 0uy;a = 0uy}
    
    let add (c:Colour) (c1:Colour): Colour =        
        {
            r = c.r + c1.r
            g = c.g + c1.g
            b = c.b + c1.b
            a = c.a + c1.a
        }
        

    let colourAdd : Monoid<Colour> = 
        {
            neutral = neutral
            op = (add)
        }
    let c1 = {neutral with g=254uy}
    let c2 = {neutral with r=254uy}
    let (+++) = add
    let x = c1 +++ c2 +++ {c2 with a=254uy}
    let l = [c1; c2 ; neutral]
            |> List.reduce (+++)


    type T = Colour
    let M = colourAdd
    let Z = M.neutral
    let (++) = M.op

    [<Property>]
    let `` Z is the neutral element``(v:T) =
        Z ++ v = v && v ++ Z = v

    [<Property>]
    let ``The operation is commutative``(a:T, b:T, c:T)=
        a ++( b ++ c) = (a ++ b ++ c)



module ``intMax with check``=

    open FsCheck.NUnit
    open System
        
    let intMax : Monoid<int> = 
        {
            neutral = Int32.MinValue
            op = (max)
        }

    type T = int
    let M = intMax
    let Z = M.neutral
    let (++) = M.op

    [<Property>]
    let `` Z is the neutral element``(v:T) =
        Z ++ v = v && v ++ Z = v

    [<Property>]
    let ``The operation is commutative``(a:T, b:T, c:T)=
        a ++( b ++ c) = (a ++ b ++ c)