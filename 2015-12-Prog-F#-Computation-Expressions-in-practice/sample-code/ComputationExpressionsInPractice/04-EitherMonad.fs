namespace ComputationExpressionsInPractice    

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


  type TraceBuilder() =
    member this.Bind(m, f) = 
        match m with 
        | None -> 
            printfn "Binding with None. Exiting."
        | Some a -> 
            printfn "Binding with Some(%A). Continuing" a
        Option.bind f m

    member this.Return(x) = 
        printfn "Returning a unwrapped %A as an option" x
        Some x

    member this.ReturnFrom(m) = 
        printfn "Returning an option (%A) directly" m
        m
     member this.Zero() = 
            let s = printfn "Zero"
            None


// make an instance of the workflow 
  let trace = new TraceBuilder()

    // test
  trace { 
      printfn "hello world"
      } |> printfn "Result for simple expression: %A" 

  trace { 
      if false then return 1
      } |> printfn "Result for if without else: %A" 


