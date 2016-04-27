namespace ComputationExpressionsInPractice


module Coroutine =
  (* https://github.com/fsprojects/FSharpx.Extras/blob/master/src/FSharpx.Extras/ComputationExpressions/Continuation.fs
  *)
  open System

  open System.Collections.Generic

  type Cont<'T,'r> = ('T -> 'r) -> (exn -> 'r) -> 'r

  let private protect f x cont econt =
      let res = try Choice1Of2 (f x) with err -> Choice2Of2 err
      match res with
      | Choice1Of2 v -> cont v
      | Choice2Of2 v -> econt v

  let runCont (c:Cont<_,_>) cont econt = c cont econt
  let throw exn : Cont<'T,'r> = fun cont econt -> econt exn
  let callcc (f: ('T -> Cont<'b,'r>) -> Cont<'T,'r>) : Cont<'T,'r> =
      fun cont econt -> runCont (f (fun a -> (fun _ _ -> cont a))) cont econt
  let bind f comp1 = 
      fun cont econt ->
          runCont comp1 (fun a -> protect f a (fun comp2 -> runCont comp2 cont econt) econt) econt     


  type ContinuationBuilder() =
      member this.Return(a) : Cont<_,_> = fun cont econt -> cont a
      member this.ReturnFrom(comp:Cont<_,_>) = comp
      member this.Bind(comp1, f) = bind f comp1
      member this.Catch(comp:Cont<_,_>) : Cont<Choice<_, exn>, _> = fun cont econt ->
          runCont comp (fun v -> cont (Choice1Of2 v)) (fun err -> cont (Choice2Of2 err))
      member this.Zero() =
          this.Return ()
      member this.TryWith(tryBlock, catchBlock) =
          this.Bind(this.Catch tryBlock, (function Choice1Of2 v -> this.Return v 
                                                  | Choice2Of2 exn -> catchBlock exn))
      member this.TryFinally(tryBlock, finallyBlock) =
          this.Bind(this.Catch tryBlock, (function Choice1Of2 v -> finallyBlock(); this.Return v 
                                                  | Choice2Of2 exn -> finallyBlock(); throw exn))
      member this.Using(res:#IDisposable, body) =
          this.TryFinally(body res, (fun () -> match res with null -> () | disp -> disp.Dispose()))
      member this.Combine(comp1, comp2) = this.Bind(comp1, (fun () -> comp2))
      member this.Delay(f) = this.Bind(this.Return (), f)
      member this.While(pred, body) =
          if pred() then this.Bind(body, (fun () -> this.While(pred,body))) else this.Return ()
      member this.For(items:seq<_>, body) =
          this.Using(items.GetEnumerator(),
              (fun enum -> this.While((fun () -> enum.MoveNext()), this.Delay(fun () -> body enum.Current))))

  let cont = ContinuationBuilder()


  type Coroutine() =
      let tasks = Queue<Cont<unit,unit>>()
                
      member this.Put(task) =
          cont {
              do! callcc <| fun exit ->
                  task <| callcc (fun c -> 
                  tasks.Enqueue(c())
                  exit())
              if tasks.Count <> 0 then
                  do! tasks.Dequeue()
          } |> tasks.Enqueue

      member this.Run() =
          runCont (tasks.Dequeue()) ignore raise

  let coroutine = Coroutine()

  coroutine.Put(fun yield' -> cont {
      printfn "A"
      do! yield'
      printfn "B"
      do! yield'
      printfn "C"
  })

  coroutine.Put(fun yield' -> cont {
      printfn "1"
      do! yield'
      printfn "2"
  })

  coroutine.Run()