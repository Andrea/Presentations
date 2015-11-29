namespace StateMachines

// What is a co-routine
module Coroutines =

  open System
  
  type Coroutine<'a> = Unit -> CoroutineStep<'a>
  and CoroutineStep<'a> =
    | Return of 'a
    | Yield of Coroutine<'a>
    | ArrowYield of Coroutine<'a>

  type CoroutineBuilder() =
    member this.Return(x) : Coroutine<'a>= 
      fun () -> Return x
    member this.Bind(pcoroutine : Coroutine<'a>, kcontinuation : 'a -> Coroutine<'b> ) : Coroutine<'b> =
      fun () ->
        match pcoroutine() with
        | Return x -> kcontinuation x ()
        | Yield p' -> Yield (this.Bind(p', kcontinuation))
        | ArrowYield p' -> ArrowYield(this.Bind(p', kcontinuation))
    member this.Combine(p1 : Coroutine<'a>, p2: Coroutine<'b>) : Coroutine<'b> =
      this.Bind(p1 , fun _ -> p2)
    member this.Zero () : Coroutine<Unit> = 
      this.Return()
    member this.ReturnFrom(s:Coroutine<'a>) = s
    member this.Delay s = s()
    member this.Run s = s

  let coroutine = CoroutineBuilder()

  let ss = coroutine {
            let! x = 

          }



  type Ship = {
      mutable Position: Vector2<m>
      mutable Velocity: Vector2<m/s>
      DryMass: float<kg>
      mutable Fuel: float<kg>
      MaxFuel: float<kg>
      Thrust: float<N/s>
      FuelBurn: float<kg/s>
      mutable Force: Vector2<N>
      mutable Integrity: float<Life>
      MaxIntegrity: float<Life>
      Damage: float<Life/s>
      WeaponsRange: float<m>
      mutable AI: Coroutine<Unit>
      }

  type  AttackAI = 
      | MovingTowards of Ship
      | Fighting of Ship
  type PirateAI = 
      | AttackingCargo of AttackAI
      | AttackingPolice of AttackAI

