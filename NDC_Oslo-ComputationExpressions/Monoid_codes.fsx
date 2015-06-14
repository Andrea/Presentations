

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