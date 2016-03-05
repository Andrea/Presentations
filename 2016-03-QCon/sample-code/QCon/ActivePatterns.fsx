open System
open System.IO
open System.Threading

type Days =
  | Monday
  | Tuesday
  | Wednesday
  | Thursday  
  | Friday
  | Saturday
  | Sunday

// Equivalent 
let weekDays day = 
  match day with

//let weekDays =
//  function
  | Monday -> "is blue."
  | Tuesday | Wednesday -> "grey."
  | Thursday -> "doesn't even start."
  | Friday -> "I am in love"
  | Saturday -> "Caturday"  //yes I know, not part of the song but you get the drift
  | Sunday -> "always come too late"


let moreDays day =
  match day with
  | firstWeek when day > 0 && day < 7 -> 
      sprintf "First week %A" firstWeek
  | secondWeek when day > 6 && day < 14 -> 
      sprintf "First week %A" secondWeek
  | _ -> "not really a day the month, maybe a star date?"

//----

let openPictures path =
  sprintf "gimp.exe %s" path
let openText path =
  sprintf "emacs.exe %s" path


// When guards :( 
let openFile (filePath) =
  match filePath with
  | path when Path.GetExtension(path) = ".txt" ||
              Path.GetExtension(path) = ".md" -> openText path
  | path when Path.GetExtension(path) = ".jpg" ||
              Path.GetExtension(path) = ".png" ||
              Path.GetExtension(path) = ".gif" -> openText path
  
  | _ -> "oh noes"

// A better way
let (|Extension|) (path: string) = Path.GetExtension <| path.ToLower()

let openFile' path =
  match path with
  | Extension ".png" 
  | Extension ".jpg" 
  | Extension ".gif" -> openPictures path
  | Extension ".txt"   
  | Extension ".md" -> openText path
  | _ -> "oh noes"


//An even better example 
open System.IO
let (|FileExtension|) filePath = Path.GetExtension(filePath)

let (|FileName|) filePath = Path.GetFileNameWithoutExtension(filePath)

let (|FileLocation|) filePath = Path.GetDirectoryName(filePath)

let determineFileType filePath =
    match filePath with
    | FileExtension (".jpg" | ".png" | ".gif")    
    | FileName "logo"
    | FileLocation "c:\images"
        -> printfn "It is on possibly an image."
    | FileName "readme"
        -> printfn "It is a Read Me file"
    | ext
        -> printfn "Unknown [%s]" filePath

// ----
type Key =
  | Space = 0
  | A = 1
  | I = 2
  | Left =3
  | Right =4
  
type KeyboardInput() =   
   
  member this.KeyPressed (key: Key) :bool =
    if key = Key.Space then 
      true
    else
      false
  member this.KeyPressedFor(key:Key, milis:int) : bool =
    key = Key.A && milis > 0

type DualityApp ()=
  static let mutable  keyboard: KeyboardInput = KeyboardInput()

  static member Keyboard 
        with get() = keyboard
        and   set(value) = keyboard <- value
type Action =
  | Jump
  | DoubleJump  
  | Idle        
  | Left
  | Right
    
let playerGo action = 
  printfn "Player game object does a %A" action

let (|SpaceKey|) (keyboard:KeyboardInput) = 
    keyboard.KeyPressed(Key.Space)

let (|``Hold I 100ms``|) (keyboard:KeyboardInput) = 
    keyboard.KeyPressedFor(Key.I, 100)  

let doSomething =   // the problem with static
  match DualityApp.Keyboard with        
  | SpaceKey true & ``Hold I 100ms`` false -> playerGo Jump
  | SpaceKey true & ``Hold I 100ms`` true -> playerGo DoubleJump
  | _ -> playerGo Idle


let (|LeftKey|RightKey|OtherKey|) (keyboard:KeyboardInput) = 
  if keyboard.KeyPressed(Key.Left) then LeftKey
  elif keyboard.KeyPressed(Key.Right) then RightKey
  else OtherKey "Hi, you pressed a key...well that alright"


let onUpdate()=    

  match DualityApp.Keyboard with            
  | LeftKey  -> playerGo Left
  | RightKey-> playerGo Right
  | OtherKey s-> ()


// Working with data can be hard 

let dateTime = DateTime.Now
let date2, time2 = dateTime.Date, dateTime.TimeOfDay

let (|Date|) (d:DateTime) = d.Date
let (|Time|) (d:DateTime) = d.TimeOfDay
let (|Hour|) (d:DateTime) = d.TimeOfDay.Hours

//active patterns without pattern matching, because every elt binding and parameter
// is a pattern match

let Date date1 = dateTime
let Time time1 = dateTime

let Date date & Time time = dateTime


let myFunction (d:DateTime) =
  sprintf "%A" d

let myFunctionDate (Date d) =
  sprintf "%A" d

myFunctionDate dateTime

let myFunctionDateAndTime (Date d & Time t & Hour h) =
  printfn "Date:%A Time: %A Hour  %A" d t h

myFunctionDate DateTime.Now

//---

let (|NonEmpty|) value =
  if String.IsNullOrEmpty value then 
    failwith "String must not be empty"
  else
    value

let (|GreaterThan|) reference value =
  if value < reference then
    failwith (sprintf "Value %A must be greater than %A" value reference)
  else 
    value

let aFunction(NonEmpty name) = 
  sprintf "Hello %A" name

// problem is failures are exceptions and that sucks 



let (|ParseOr0|) v =   
  match Int32.TryParse v with
  | true, r ->  r
  | _,_ ->  0
  
let contrivedAdd (ParseOr0 c) = c + 5

contrivedAdd "457"
contrivedAdd "Monkeys"



// Inline functions are functions that are integrated directly into the calling code.
let inline (|Optional|) (y: 'T) (x:'T option): 'T =
  match x with
  | Some v -> v
  | _ -> y

let inline (|NotNull|) (x:'T): 'T =
  if isNull x then failwith "argument is null sad times"
  x

let f (NotNull (s:string)) (Optional 0 v) = s.Length + v

f "aaa" None

f null (Some 5)


let eirik =
  match Some(41 , fun i -> Some(i+1)) with
  | Some( y, (|Pattern|_|)) ->
    match y with 
    | Pattern z -> printfn "%d" z
    | _ -> ()
  | None -> ()

let myFn (): Choice<int, Exception> =
  //Choice1Of2(1)
  Choice2Of2(new Exception("ba lind"))

let run (|Success|Failure|) =
  match () with
  | Success t -> t
  | Failure e -> 
        printfn "before raising"
        raise e

run myFn