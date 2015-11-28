namespace WhatTheMonkey
    
type MaybeBuilder() = 
        
        member __.Bind(value, func) = 
            match value with
            | Some value -> func value
            | None -> None
        
        member __.Return value = Some value
        member this.ReturnFrom value = this.Bind(value, this.Return)
    

module ``divide `` = 
    let random = new System.Random()
    
    let division a b c d = 
        match b with
        | 0 -> None
        | _ -> 
            match c with
            | 0 -> None
            | _ -> 
                match d with
                | 0 -> None
                | _ -> Some(((a / b) / c) / d)
    
    let divide a b = 
        match b with
        | 0 -> None
        | _ -> Some(a / b)
    let maybe = MaybeBuilder()

    let divisionM a b c d = maybe { let! x = divide a b
                                    let! y = divide x c
                                    let! z = divide y d
                                    return z }
    


module CalculateResistance = 
    type Result = 
        | Success of float
        | DivByZero
    
    let divide x y = 
        match y with
        | 0.0 -> DivByZero
        | _ -> Success(x / y)
    
    type DefineBuilder() = 
        
        member this.Bind(x : Result, rest : float -> Result) = 
            match x with
            | Success(x) -> rest x
            | DivByZero -> DivByZero
        
        member this.Return(x : 'a) = x
    
    let definer = DefineBuilder()
    let totalResistance r1 r2 r3 = definer { let! x = divide 1.0 r1
                                             let! y = divide 1.0 r2
                                             let! z = divide 1.0 r3
                                             return divide 1.0 (x + y + z) }

// Example from Programming F# 3

module InventoryExample = 
    open System.Collections.Generic
    
    type ProductId = string
    type Price = float
    
    type Inventory() = 
        let inv = new Dictionary<ProductId, Price>()
        member this.Stock (id : ProductId) (price : Price) = inv.Add(id, price)
        member this.Price(id : ProductId) = 
            try 
                Some(inv.[id])
            with :? KeyNotFoundException -> None

    let maybe = MaybeBuilder()

    let (|@|) p1 p2 = maybe { 
        let! v1 = p1
        let! v2 = p2
        return! Some(v1 + v2) }
    
    let reporter priceSum  = 
        match priceSum with
        | Some(p) -> printfn "Total price: %g." p
        | None -> printfn "One or more id not found."    
    
    let inventory = new Inventory()
    
    inventory.Stock "MyWidget" 10.3
    inventory.Stock "Gizmos" 4.34
    inventory.Stock "Foo1000" 8.12

    //Sum prices
    inventory.Price("MyWidget") |@| inventory.Price("Gizmos") |> reporter
    
    //A further step, price sum pipelining
    inventory.Price("MyWidget")
    |> (|@|) (inventory.Price("Gizmos"))
    |> (|@|) (inventory.Price("Foo1000"))
    |> reporter
    
    //A failing computation
    inventory.Price("MyWidget")
    |> (|@|) (inventory.Price("Gizmos"))
    |> (|@|) (inventory.Price("DoesNotExist"))
    |> reporter    
    
    let sumAndReport (inventory : Inventory) ids = 
        let basket = List.map (fun pid -> inventory.Price(pid)) ids
        List.reduce (fun p1 p2 -> p1 |@| p2) basket |> reporter

// Example from https://alfredodinapoli.wordpress.com/2012/04/02/humbly-simple-f-maybe-monad-application-scenario/
// but using local Maybe definition 


module Results = 

type DbResult<'a> = 
    | Success of 'a
    | Error of string

let getCustomerId name =
    if (name = "") 
    then Error "getCustomerId failed"
    else Success "Cust42"

let getLastOrderForCustomer custId =
    if (custId = "") 
    then Error "getLastOrderForCustomer failed"
    else Success "Order31415"

let getLastProductForOrder orderId =
    if (orderId  = "") 
    then Error "getLastProductForOrder failed"
    else Success "Product27182"

type EitherBuilder() = 
        
        member __.Bind(value, func) = 
            match value with
            | Success a -> 
                printfn "Tracing success since -2001 %A" a
                func a
            | Error _ -> value
        
        member __.Return value = Success value
       
let dbResult = new EitherBuilder()
let product' = 
    dbResult {
        let! custId = getCustomerId "Alice"
        let! orderId = getLastOrderForCustomer custId
        let! productId = getLastProductForOrder "" 
        printfn "Product is %s" productId
        return productId
        }
printfn "%A" product'

module Statefulness = 

type StringCheckerBuilder() =         
        member __.Bind(value, func) = 
            match value with
            | Some i  ->             
                func i
            | None -> 0
        
        member __.Return value = value
        
let strToInt (s: string) = 
    match System.Int32.TryParse s with
    | true, i -> Some i
    | false, _ -> None
                        
let stringCheck = StringCheckerBuilder()
let stringAddWorkflow x y z = 
    stringCheck 
        {
        let! a = strToInt x
        let! b = strToInt y
        let! c = strToInt z
        return a + b + c
        }    

 
let good = stringAddWorkflow "12" "3" "2"
let bad = stringAddWorkflow "12" "xyz" "2"