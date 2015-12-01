// From http://fsharpgamedev.codeplex.com/SourceControl/latest#FSharpBook/SimulationsAndGeneralDiscussion/FSharpBook/Chapter5.Examples.fs
namespace Chapter5
  (*
  Monads/Computation Expressions ( ** )

  In this section we discuss in depth the F# feature known as Computation Expressions.
  First of all, a note about the name: Computation Expressions are actually known as monads in Category Theory and other programming languages such as Haskell.
  Indeed, Haskell and F# are the two most prominent programming languages making heavy use of monads.

  Monads can be seen from a variety of perspectives.
  One one, more formal, hand, monads are a triplet composed of:
  - a generic datatype M<'a>
  - a bind operator with signature M<'a> -> (a -> M<'b>) -> M<'b>
  - a return operator with signature 'a -> M<'a>

  A monad can then be seen, at its most abstract, as a type with a constructor (return) and a composition operator (bind).
  Since monads come from a theoretical area of computer science, there are various theorems that guarantee useful and interesting properties ,
  provided that the monad definition satisifies a set of axioms that characterize "well-behaved" monads.
  We will not explore these aspects in depth.
  If you, dear reader, are interested in better comprehending (pun intended) monads, then start with http://en.wikipedia.org/wiki/Monad_(functional_programming) and work your way through the various references.
  Instead, we will observe that a monad is something quite intuitive.
  The datatype M<'a> may hold any type and data, and the return operation is simply a means to constructing an instance of M<'a>.
  A binding operation is a very general concatenation operator, which defines how we extract a value 'a,
  and plug it into something that from a value of type 'a is capable of building a new monad.
  Thus, binding can be seen as a generalization of binding.
  Usually, when we write a program, we have a series of bindings, each defined as:
  let x = EXPR
  BODY

  a let-binding can also be seen as:
  (fun x -> BODY) EXPR

  that is BODY needs a value for x, and this value is provided by EXPR.
  Monads allow us to process some additional information  when performing a binding seen with the functional definition above.

  In conclusion, monads offer us a way to pack values inside complex datatypes, and to define how let-binding monadic values propagates and processes these additional information.
  Monadic frameworks can be formulated for manipulating threads, sequences, coroutines, exceptions, backtracking and many other useful operations.
  This effectively amounts to redefining the base mechanisms of the language, thus building small languages embedded into F#.
  C# is the first very widespread commercial language that has used monads, first with LINQ in C# 3.0 and then in C# 5.0 with AWAIT, even though markteting reasons have prevented any reference to monads when presenting such features :).

  F#, like Haskell, allows us to define custom monads with syntactic sugar to easily define, consume and bind monads.
  F# offers syntactic sugar in the form of a series of implicit translation rules that greatly improve readability when using monads.
  We write code that looks like we are not using monads, apart from a few bangs (let!, do!, return!) and F# will appropriately insert the right monadic operators according to the following translation rules:

  m{
    let! x = m1
    m2
  }

  becomes
  m.Bind(m1, fun x -> m2)

  m{
    return v
  }

  becomes
  m.Return v

  m{
    return! v
  }
  becomes

  m.ReturnFrom(v)

  m{
    if c then
      m1
  }
  becomes
  if c then m1 else m.Zero()

  In general, for every construct of the language there exists a monadic version.
  We can write for-loops, exception handling with try-catch, etc. and each of these constructs might have a monadic equivalent.
  Apart from Bind and Return, though, all monadic operators are optional: a monad is not forced to support all language constructs.
  *)
  module Examples =
    (*
    Option/Maybe Monad ( ** )

    To see monads in action, we start with a simple monad known as Option.
    The option monad is used to encapsulate common operations that may result in:
    - a value of type 'a
    - an error

    the monadic datatype is Option<'a>.
    This monad is used for a lightweight, customized exception handling, where at each point the program may fail and the monad will start to correctly propagate the error.
    *)
    module OptionMonad =
      type OptionBuilder() =
        (*
        Returning a value encapsulates it in the Some constructor
        *)
        member this.Return(x:'a):Option<'a> = Some(x)
        (*
        To bind two options, we check the first.
        If it has no value, that is it represents a computation that has gone wrong, then we just propagate the error.
        If it has a value, then we pass the value to the continuation of the program and return its result:
        *)
        member this.Bind(p:Option<'a>,k:'a->Option<'b>):Option<'b> =
          match p with
          | None -> None
          | Some x -> k x
        (*
        Zero and ReturnFrom have a simple definition; this kind of definition (empty container for zero, identity for return_
        is very common:
        *)
        member this.Zero() : Option<'a> = None
        member this.ReturnFrom(p:Option<'a>) : Option<'a> = p

      (*
      We create an instance of the monad builder:
      *)
      let opt = OptionBuilder()

      (*
      We define a customized return operator, which will be useful to wrap a value in a corresponding Option:
      *)
      let ( ! ) x = Some x

      (*
      At this point we can define safe division between two options.
      We start by binding x and y, which effectively checks if they are None.
      If binding succeds and if y is different from zero then we return the result of the division, while the zero operation is invoked if the test fails:
      *)
      let ( / ) x y =
        opt{
          let! x = x
          let! y = y
          if y <> 0 then
            return x / y
        }

      (*
      We also define a common operation, known as lifting, which converts a normal operation into the realm of monads.
      Notice that the ( ! ) operator we have seen above is actually the lift0 operator.
      The idea is that we take a function that does not manipulate monads and we create a new function that is the same as the original but whose arguments are monadic:
      *)
      let lift2_opt f x y =
        opt{
          let! x = x
          let! y = y
          return f x y
        }

      (*
      With the lifting operations we can easily and quickly "import" into a monad a series of operations that have not been built around monads:
      *)
      let ( + ) = lift2_opt ( + )
      let ( * ) = lift2_opt ( * )
      let ( - ) = lift2_opt ( - )

      (*
      At this point let us define a safe epression evaluator that performs two divisions;
      notice that we do not have to perform any checks as those are implicit in the monad.
      Removing boilerplate code is always a big win!
      *)
      let expr a b c d =
        opt{
          return! a / b + c / d
        }

      (*
      We can invoke a few of our operations with regular parameters lifted with ( ! ):
      *)
      let opt_res1 = !10 / !0
      let opt_res2 = !10 / !5
      let opt_res3 = opt_res1 + opt_res2
      let opt_res4 = expr !1 !2 !3 !1

    (*
    List Monad ( ** )

    The second monad we see is the "sister" of the sequence comprehensions seen in a previous chapter: the List monad.
    With the list monad we can manipulate entire lists with a much simpler syntax, where binding an element is equivalent to iterating all of its elements.
    *)
    module ListMonad =

      type ListBuilder() =
        (*
        Returning an element creates a list with only that element.
        This is also called yielding; yielding and returning are essentially the same thing, but yield is more used in the context of sequences while
        return in the context of values and operations. Must not be confused with yielding for coroutines, which we have defined with a trailing underscore!
        *)
        member this.Return(x:'a):List<'a> = [x]
        member this.Yield(x:'a):List<'a> = this.Return(x)

        (*
        Binding for lists creates a list by appliying the continuation operation to each element of the input list and then
        concatenating the results:
        *)
        member this.Bind(p:List<'a>,k:'a->List<'b>):List<'b> =
          let pk = List.map k p
          List.concat pk
        (*
        The zero element of the monad is the empty list, and returning (or yielding) from a list is the identity
        *)
        member this.Zero() : List<'a> = []
        member this.ReturnFrom(p:List<'a>) : List<'a> = p
        member this.YieldFrom(p:List<'a>) : List<'a> = p

        (*
        Monads also allow iteration with for-loops, where the body of the for loop is a monadic expression with binds and returns;
        we apply the body to each element of the input sequence of iteration and concatenate the results; this is essentially a form of binding
        on an input sequence rather than an input list:
        *)
        member this.For(s:seq<'a>, body:'a->List<'b>):List<'b> =
          List.concat (Seq.map body s)

      (*
      We define our instance of the list monad:
      *)
      let lst = ListBuilder()

      (*
      Map simply binds a value to a list l, thereby iterating its elements.
      For each element the transformed element is returned.
      *)
      let map f l =
        lst{
          let! x = l
          return f x
        }

      (*
      Filter, too, binds a value to a list l to iterate it; then if the test succeeds the element is returned, otherwise the zero list (which is empty) is returned:
      *)
      let filter p l =
        lst{
          let! x = l
          if p x then
            return x
        }

      (*
      Building the cartesian product of two lists is extremely simple: we bind one, then the other, and then return the resulting pair; all pairs of elements are returned:
      *)
      let cartesian l1 l2 =
        lst{
          let! x = l1
          let! y = l2
          return x,y
        }

      (*
      Building all the permutations of a list is quite simple; if the list contains only one element, then it has just one permutation;
      otherwise, we iterate the elements of l with a binding, we iterate the permutations of the rest of the list and we return each combination:
      *)
      let rec permutations =
        function
        | [x] -> [[x]]
        | l ->
          lst{
            let! x = l
            let others = filter ((<>) x) l
            let! l' = permutations others
            return x :: l'
          }

      (*
      Given the similarity between for and bind in the list monad, we see how we can give the definition of map, filter and the cartesian product with for instead of bind.
      The definitions have a line-by-line correspondence with the definitions seen above, but instead of binding they do a for-loop on the input list:
      *)
      let map' f l =
        lst{
          for x in l do
            yield f x
        }
      let filter' p l =
        lst{
          for x in l do
            if p x then
              yield x
        }
      let cartesian' l1 l2 =
        lst{
          for x in l1 do
            for y in l2 do
              yield x,y
        }

    (*
    State Monad ( ** )

    In some cases we may need to control how the state of the program is propagated along various statements.
    This can become very useful if we have a complex system where many threads access a shared state, and we need to
    perform certain common operations every time we read or write a state location.
    *)
    module StateMonad =

      (*
      The definition of the state is a bit complicated at first;
      the state datatype represents a *computation* that accesses the state and returns a result at the same time.
      Such a computation takes as input the initial value of the state of the program, and returns the pair of
      the result 'a and the updated state.
      Imperative programmers are used to this kind of behavior; for example, when in C we write:
      int x = ++i;

      then what we expect is that the global state of the program changes (the value of i is incremented) and x is assigned the new value of i.
      *)
      type State<'a,'s> = 's -> ('a * 's)

      type StateBuilder() =
        (*
        Returning a value does not change the state, which is propagated without change together with the value to be returned:
        *)
        member this.Return(x:'a) : State<'a,'s> = fun s -> x,s
        (*
        Binding two states p and k requires to:
        - get the external state s
        - pass it to p, thereby obtaining its result x and a new state s
        - pass both x and s to k and return its result
        *)
        member this.Bind(p:State<'a,'s>,k:'a -> State<'b,'s>) : State<'b,'s> =
          fun s ->
            let x,s = p s
            k x s
        (*
        Zero and return! have the usual definitions:
        *)
        member this.Zero() = fun s -> (),s
        member this.ReturnFrom(s:State<'a,'s>) : State<'a,'s> = s

      let st = StateBuilder()

      (*
      We define a type for our state with only two memory locations, both integers;
      we also define a monadic getter and a setter for each of these locations, that is functions that return a value of type State<'a,MyState>:
      *)
      type MyState = { x : int; y : int }
      (*
      get_x and get_y have type State<int,MyState>, that is they extract an integer from a state of type MyState:
      *)
      let get_x s = s.x,s
      (*
      set_x and set_y have type int -> State>unit,MyState>, that is they take as input an integer and return an operation that modifies the state and returns no value:
      *)
      let get_y s = s.y,s
      let set_x v s = (),{ s with x = v }
      let set_y v s = (),{ s with y = v }

      (*
      We now define a function that passes around a value of type MyState that is modified at each step of the computation;
      in particular our function extracts the x and the y from the state, sums them and stores the result into the x:
      *)
      let sum_to_x =
        st{
          let! x = get_x
          let! y = get_y
          do! set_x (x+y)
        }

    (*
    F# contains two important monads that are already implemented: seq and async;
    seq we have seen already and is almost identical to the list monad we have seen above;
    async is a monad for defining threaded computations, where binding a value means that the current thread needs to wait for the bound thread to finish and returning signals the conclusion of the current thread.
    We report here a small sample taken from the MSDN library (http://msdn.microsoft.com/en-us/library/dd233250.aspx).
    *)
    module AsyncSample =
      open System.Net
      open Microsoft.FSharp.Control.WebExtensions

      (*
      We define the input and output of our computations;
      the input of a computation is a website name, the computation index and a url; the output is a string that describes how many bytes were read from each website:
      *)
      let urlList = [ "Microsoft.com", 0, "http://www.microsoft.com/"
                      "MSDN", 1, "http://msdn.microsoft.com/"
                      "Bing", 2, "http://www.bing.com"
                    ]
      let results = [| ""; ""; "" |]

      (*
      The asynchronous computation (we might see it as a thread, albeit a lightweight one) uses the webClient.AsyncDownloadString method to create an asynchronous computation that, when executed, will download a webpage.
      This asynchronous computation is bound to the "html" identifier, which is then used to write the result; notice that the async monad supports exception handling by redefining the TryWith method:
      *)
      let fetchAsync(name, i, url:string) =
          async {
              try
                  let uri = System.Uri(url)
                  let webClient = new WebClient()
                  let! html = webClient.AsyncDownloadString(uri)
                  results.[i] <- (sprintf "Read %d characters for %s" html.Length name)
              with
                  | ex -> printfn "%s" (ex.Message);
          }

      (*
      The runAll function uses the Async.Parallel function to wrap together a sequence of asynchronous computations into a single one,
      and the Async.RunSynchronously function to run an asynchronous computation and waiting for its result.
      After all computations are completed, we print the results array:
      *)
      let runAll() =
        do  ignore(
              Async.RunSynchronously
                (Async.Parallel(Seq.map fetchAsync urlList)))
        do printf "%A\n" results

(*
[[[[
Full List of Supported Monadic Operators ( ** )

Essentially every F# construct, from let to do to try-with can be redefined inside monads.
This alone makes it possible to define a custom language where each keyword has a different meaning and which works at a higher level of abstraction than F#.
We use monads to hide boilerplate code that would hinder readability or to ensure that certain complex but repetitive operations are performed correctly.

The following list is taken from http://msdn.microsoft.com/en-us/library/dd233182.aspx and describes the set of constructs that we may redefein in monads, along with their possible type signatures:

- Bind : M<'T> * ('T -> M<'U>) -> M<'U>, Called for let! and do! in computation expressions.
- Return : 'T -> M<'T>, Called for return in computation expressions.
- Combine : M<'T> * M<'T> -> M<'T> or M<unit> * M<'T> -> M<'T>, Called for sequencing in computation expressions.
- ReturnFrom, M<'T> -> M<'T>, Called for return! in computation expressions.
- For : seq<'T> * ('T -> M<'U>) -> M<'U> or seq<'T> * ('T -> M<'U>) -> seq<M<'U>>, Called for for...do expressions in computation expressions.
- TryFinally : M<'T> * (unit -> unit) -> M<'T>, Called for try...finally expressions in computation expressions.
- TryWith : M<'T> * (exn -> M<'T>) -> M<'T>, Called for try...with expressions in computation expressions.
- Using : 'T * ('T -> M<'U>) -> M<'U> when 'U :> IDisposable, Called for use bindings in computation expressions.
- While : (unit -> bool) * M<'T> -> M<'T>, Called for while...do expressions in computation expressions.
- Yield : M<'T> -> M<'T>, Called for yield expressions in computation expressions.
- YieldFrom : M<'T> -> M<'T>, Called for yield! expressions in computation expressions.
- Zero : unit -> M<'T>, Called for empty else branches of if...then expressions in computation expressions.
- Delay : (unit -> M<'T>) -> M<'T>, Wraps a computation expression as a function.
- Run : M<'T> -> M<'T> or M<'T> -> 'T, Executes a computation expression.

]]]]
*)

    (*
    A note on F# Classes ( ** )

    F# is a truly multi-paradigm language, that is it does not focus on a specific paradigm such as functional, procedural, etc but tries to make all of its paradigms coexist as best as possible.
    F# supports:
    - functional programming
    - imperative programming
    - object-oriented programming

    mixed as we want.

    In this section we discuss how we can use some of the object-oriented features of F#, in particualr how we can define classes, inheritance and interfaces.

    We have seen above a new yntax for defining types:
    type NAME() =
      MEMBERS

    this syntax is the syntax for defining clases with a single constructor; the full syntax actually is:
    type NAME(CONSTRUCTOR-PARAMS) =
      CONSTRUCTOR-BODY
      MEMBERS

    where CONSTRUCTOR-PARAMS is a tupled series of  parameters, and CONSTRUCTOR-BODY is a series of let-bindings and do's that are executed when an instance of the class is created.
    *)
    module Classes =

      (*
      We start with a simple definition of a generic counter class, which takes as input a generic zero, one and sum operation;
      *)
      type Counter<'a>(zero:'a,one:'a,plus) =
        (*
        The constructor initializes a mutable variable (the counter) to zero and prints a message that says that the counter has been initialized:
        *)
        let mutable x = zero
        do printf "A new counter with 0=%A and 1=%A has just been initialized\n" zero one

        (*
        The class has three members, which, respectively:
        - increment the current counter by one
        - zeroes the counter
        - returns the current value of the counter
        *)
        member this.Incr() = x <- plus x one
        member this.Zero() = x <- zero
        member this.Value = x

      (*
      We define two classes that inherit the Counter<int> class, and that directly invoke its constructor with appropriate parameters.
      These classess could, of course, take as input parameters for their constructor, perform operations and have any additional methods
      as the Coutner class did:
      *)
      type NormalCounter() =
        inherit Counter<int>(0,1,(+))
      type FasterCounter() =
        inherit Counter<int>(0,2,(+))

      (*
      We define a function that takes as input a counter of integers and which zeroes it, increments it ten times and then prints its value:
      *)
      let use_counter (c:Counter<int>) =
        do c.Zero()
        for i = 1 to 10 do
          do c.Incr()
        do printf "%A\n" c.Value

      (*
      We now define a Dog interface that specifies that adherents to this interface must have a Bark method;
      when all members of a class are abstract and the class has no constructors then it is automatically inferred to be an interface:
      *)
      type Dog =
        abstract member Bark : string

      (*
      The "interface" keyword is used to define an interface that a class implements.
      Under the "interface" keyword we define the various methods of the interface, and below those we can define any additional methods we may want:
      *)
      type Yorkshire() =
        interface Dog with
          member this.Bark = "Yip, yip"
        member this.LikesToPlay = true
      type SussexSpaniel() =
        interface Dog with
          member this.Bark = "Woof, yip"
        member this.LikesToPlay = true

//module ComputationExpression =
//
//  open System
//
//  let sleepAndPrint x = 
//      printfn "sleeping and printing"
//      Async.Sleep x
//  let sleepWorkflow  = async{
//      printfn "Starting sleep workflow at %O" DateTime.Now.TimeOfDay
//      do! sleepAndPrint 2000
//      printfn "Finished sleep workflow at %O" DateTime.Now.TimeOfDay
//      }
//
//  Async.RunSynchronously sleepWorkflow
