#r "packages/FsCheck.2.2.3/lib/net45/FsCheck.dll"
#r "packages/NUnit.Runners.2.6.4/tools/lib/nunit.core.dll"
#r "packages/NUnit.Runners.2.6.4/tools/lib/nunit.core.interfaces.dll"


open NUnit.Framework
open FsCheck

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

    
    let `` Z is the neutral element``(v:T) =
        Z ++ v = v && v ++ Z = v


    let ``The operation is commutative``(a:T, b:T, c:T)=
        a ++( b ++ c) = (a ++ b ++ c)