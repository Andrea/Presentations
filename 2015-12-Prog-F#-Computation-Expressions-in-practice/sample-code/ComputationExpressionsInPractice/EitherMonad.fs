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

 


