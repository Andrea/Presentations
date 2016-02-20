namespace ComputationExpressionsInPractice
    
type MaybeBuilder() = 
(*
3. For this step you will need to implement Bind
*)
      member __.Bind(maybeValue: 'a option, func) = 
          match maybeValue with
          | Some value -> func value
          | None -> None
        
      member __.Return value = Some value
      member this.ReturnFrom value = this.Bind(value, this.Return)
    

module ``Divide by zero `` =
    
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

    let divisionMCE a b c d = 
      maybe { let! x = divide a b
              let! y = divide x c
              let! z = divide y d
              return z }


    let divisionM a b c d = 
      maybe.Bind(divide a b, fun x ->
        maybe.Bind(divide x c, fun y ->
          maybe.Bind( divide y d, fun z ->
            maybe.Return(z))))

    do divisionM 120 4 3 2 = divisionMCE 120 4 3 2 |> printfn "Should be true: %A"
    
