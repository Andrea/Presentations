namespace ComputationExpressionsInPractice
module Simplest =
    
    type OnlyContinueBuilder () =
        // all Bind does is make the binding happen, i.e. apply x to f
        member this.Bind(x,f) = f x
        member this.Return(x) = x

    let onlyContinue = new OnlyContinueBuilder() 

    let x =onlyContinue 
            {
                // Bindings NEVER fail, yeehaw!
                let! foo = "bar"
                let! bar = "baz"
                let! fooBar = foo + bar
                return fooBar
            }