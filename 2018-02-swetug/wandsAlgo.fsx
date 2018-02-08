

type Expression = 
    | INT of int
    | BOOL of bool 
    | ARG of string // name of argument
    | LAMBDA of Expression * Expression // argument | body
    | IF of Expression * Expression * Expression  // condition yes no
    | APPLY of Expression * Expression // lambda argument
    | EQUAL of Expression * Expression


let simpleFunc bla  = if bla =  5 then 2 else 3

let simpleFuncAst = LAMBDA(ARG "bla",
                    IF(EQUAL(
                        ARG("bla"), INT(5)), 
                        INT(2),
                        INT(3)
                    ))
    
let rec eval_ exp (context: Map<string, Expression>) : Expression = 
    match exp with
    | INT(v) -> INT(v)
    | BOOL(v) -> BOOL(v)
    | ARG(name) -> eval_ context.[name] context
    | IF(exp, trueBranch, falseBranch) -> 
            match eval_ exp context with// TODO: this might not be the way?
            | BOOL(t) when t -> eval_ trueBranch context
            | BOOL(t) when not t -> eval_ falseBranch context
    | LAMBDA(arg, body) -> eval_ body context //avoiding arg because I have the info in 'context'
    | EQUAL(left,right) -> 
        let l = eval_ left context
        let r = eval_ right context
        BOOL(l = r)
    | APPLY(name, lambda) -> (* eval name context ? what for?*) 
        eval_ lambda context // also avoid the name?

let context =  [("bla", INT 5)] |> Map.ofList
eval_ simpleFuncAst context


type AstExpression = 
    | INT of int * string
    | BOOL of bool * string
    | ARG of string * string // name of argument
    | LAMBDA of AstExpression * AstExpression * string //argument / body
    | IF of AstExpression * AstExpression * AstExpression *string  // condition yes no
    | APPLY of AstExpression * AstExpression * string// lambda / argument
    | EQUAL of AstExpression * AstExpression * string
let mutable counter = 0

let rec labelExp ast (names:Map<string, string>) = 
    let genName() =
        counter <- counter + 1
        //let r = sprintf "😺_%i" counter
        let r = sprintf "t_%i" counter
        printf "the counter is %i" counter
        r
    match ast with
    | Expression.INT(x) ->  INT (x, genName()),names
    | Expression.BOOL(x) -> BOOL(x, genName()), names
    | Expression.ARG(name) -> 
        let label = if names.ContainsKey name then names.[name] else genName()
        ARG(name, label), names.Add( name, label)
    | Expression.IF(conditional, trueB, falseB) -> //but the if needs  a laberl too??
        let (c,n1) = labelExp conditional names 
        let (t, n2)= labelExp trueB n1 
        let (f,n3) = labelExp falseB n2
        IF(c,t,f, genName()), n3  
    | Expression.EQUAL(left, right) -> 
        let (l, n1) = labelExp left names
        let (r, n2) = labelExp right n1
        EQUAL(l,r, genName()),n2
    | Expression.LAMBDA(arg, body) ->
        let (a, n1) = labelExp arg names
        let (b, n2) = labelExp body n1
        LAMBDA(a,b,genName()), n2
    | Expression.APPLY(name, lambda) ->
        let (n, n1) = labelExp name names
        let (l, n2) = labelExp lambda n1
        LAMBDA(n,l,genName()), n2   
    


let getTag exp =
    match exp with
    | EQUAL(_,_,name) -> name
    | IF(_, _,_,name) -> name
    | LAMBDA(_,_,name) -> name
    | ARG(_,name) -> name
    | APPLY(_,_,name) -> name
    | INT(_,name) -> name
    | BOOL(_,name) -> name

type TypeBounded = 
    | Bound of string
    | Unbound of string

type Constraint =
    | Equality of TypeBounded * TypeBounded  // ie left === right
    | GoesTo of TypeBounded * TypeBounded * TypeBounded// ie name === a -> b

let rec constraints tree (resultSet: Set<Constraint>) :  Set<Constraint> =
    match tree with
    | IF (cond, yes, no, name) ->         
        
        let rsac = resultSet.Add(Equality(getTag cond|> Unbound, Bound "BOOL"))
        let condr = constraints cond rsac
        let yesrc = constraints yes condr
        let norc  = constraints no yesrc
        let ifrn = Equality(getTag no |> Unbound , Unbound name)
        let ifry = Equality(getTag yes |> Unbound , Unbound name)
        (norc.Add ifrn).Add ifry
    | INT(_, name) -> 
        resultSet.Add(Equality(Unbound name ,Bound "INT"))
    | BOOL(_, name) ->
        resultSet.Add(Equality(Unbound name , Bound"BOOL"))
    | EQUAL(l,r,name) ->
        let lrs = constraints l resultSet
        let rrs = constraints r lrs 
        rrs
            .Add(Equality(getTag l |> Unbound ,getTag r |> Unbound))
            .Add(Equality(Unbound name, Bound "BOOL"))
    | LAMBDA(arg,fn, name) ->
        let ln = GoesTo(Unbound name, 
                             getTag arg |> Unbound , getTag fn |> Unbound)
        let lbr = resultSet.Add(ln)
        constraints fn  lbr
    | ARG(arg, name) -> 
        resultSet //not sure about this one
    | APPLY(lamb, arg, name) ->
        resultSet // also not so sure about this CH but I don't have an apply in use CH CH

// let astCon = constraints (fst astE) Set.empty
let simpleFuncAstExpression =  labelExp simpleFuncAst Map.empty|> fst
constraints simpleFuncAstExpression Set.empty



open System.Collections.Generic

let unifyClean (orig: Set<Constraint>) (bound: Dictionary<string, string> )=
    let ordered = orig |> Set.toList 
                       |> List.sortBy(function  Equality(Unbound _, Bound _) -> 0
                                                | Equality(Unbound _, Unbound _) -> 1 
                                                | _ -> 2)
   
    let tryAddWithUnificationFail k v =
        printf "trying to add key %A value %A" k v
        let res, value = bound.TryGetValue k
        if(res) then 
            if(v <> value) then sprintf "Unification problem %A is not %A" res value |> failwith
            else () // do nothing, trying to add something you already have
        else 
            bound.Add(k,v)

    for c in ordered do
        match c with
        | Equality(Unbound(x), Bound(i)) -> 
            match bound.TryGetValue(x) with
            | false, _ -> 
                bound.Add(x, i)
            | _ -> ()
        | Equality(Unbound(l), Unbound(r)) ->            
            match (bound.TryGetValue(l), bound.TryGetValue(r)) with
            | ((true, lval), (false, _)) -> 
                tryAddWithUnificationFail r lval 

            | ((false, _), (true, rval)) -> 
                tryAddWithUnificationFail l rval
            | _ -> ()
        | GoesTo(Unbound(x), Unbound(z), Unbound(y)) -> 
            match (bound.TryGetValue(z), bound.TryGetValue(y)) with
            | ((true, zvalue),(true,yvalue)) -> 
                tryAddWithUnificationFail x (zvalue + "->"+ yvalue)
            | ((false, _),(true, yvalue)) -> 
                bound.Add(x,z + "->"+ yvalue)
            | ((true, zvalue),(false, _)) -> 
                bound.Add(x,zvalue + "->"+ y)
            | ((false, _), (false, _)) -> bound.Add(x, z + "->"+ y) 
        | _ -> ()
    bound

let bound  = new Dictionary<string, string>()
let simpleAstConstraints = constraints simpleFuncAstExpression Set.empty

unifyClean simpleAstConstraints bound