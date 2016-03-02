open System
//the input is a string of characters, for now a string

// compiler services


let A_parser str =
  if String.IsNullOrEmpty(str) then (false, "")
  else
    if(str.[0] = 'A') then
      let remaining = str.[1..]
      (true, remaining)
    else
      (false, str)

let inputABC = "ABC"
A_parser inputABC

let inputZBC = "ZBC"
A_parser inputZBC


let pchar (charToMatch, str) = 
  if (String.IsNullOrEmpty(str))then
    let msg = "no more"
    (msg, "")
  else
    let first = str.[0]
    
    if(first = charToMatch) then
      let remaining = str.[1..]
      (sprintf "found %c" charToMatch, remaining)
    else
      let msg = sprintf "expecting %c. got %c"charToMatch first
      (msg, str)

pchar('Z',inputZBC)



type Result<'a> =
    | Success of 'a
    | Failure of string

 
let pchar1 (charToMatch, str) = 
  if (String.IsNullOrEmpty(str))then
    Failure "no more"      
  else
    let first = str.[0]    
    if(first = charToMatch) then
      let remaining = str.[1..]
      Success (sprintf "found %c" charToMatch, remaining)
    else
      let msg = sprintf "expecting %c. got %c"charToMatch first
      Failure msg

let pchar2 charToMatch  str = 
  if (String.IsNullOrEmpty(str))then
    Failure "no more"      
  else
    let first = str.[0]    
    if(first = charToMatch) then
      let remaining = str.[1..]
      Success (sprintf "found %c" charToMatch, remaining)
    else
      let msg = sprintf "expecting %c. got %c"charToMatch first
      Failure msg


let pchar3 charToMatch  =
  let innerFn str = 
    if (String.IsNullOrEmpty(str))then
      Failure "no more"      
    else
      let first = str.[0]    
      if(first = charToMatch) then
        let remaining = str.[1..]
        Success (sprintf "found %c" charToMatch, remaining)
      else
        let msg = sprintf "expecting %c. got %c"charToMatch first
        Failure msg
  innerFn

type Parser<'T> = Parser of (string -> Result<'T * string>)

let pchar5 charToMatch  =
  let innerFn str = 
    if (String.IsNullOrEmpty(str))then
      Failure "no more"      
    else
      let first = str.[0]    
      if(first = charToMatch) then
        let remaining = str.[1..]
        Success (charToMatch, remaining)
      else
        let msg = sprintf "expecting %c. got %c"charToMatch first
        Failure msg
  Parser innerFn

let run parser input =     
    printfn "Input %A" input
    let (Parser innerFn) = parser     
    innerFn input

let parseA = pchar5 'A'
run parseA "ABC"

// combining parsers

let parseB = pchar5 'B'
let parseC = pchar5 'C'

// computer says no
//let parseAthenB = parseB >> parseC

let andThen parser1 parser2 =
  let innerFn input  =
    let result1 = run parser1 input
    match result1 with 
    | Failure err -> Failure err
    | Success (v1, rem1) -> 
        let result2 = run parser2 rem1
        match result2 with
        | Failure err -> Failure err
        | Success (value2, remaining) -> 
            let newValue = (v1, value2)
            Success(newValue, remaining)
  Parser innerFn

let (.>>.)= andThen

let parseAthenB = andThen parseB   parseC

run parseAthenB "BCZ"
run parseAthenB "XC"
run parseAthenB "BXC"

let orElse parser1 parser2 =
  let innerFn input  =
    let result1 = run parser1 input
    match result1 with     
    | Success _ -> result1
    | Failure _ ->run parser2 input        
  Parser innerFn

let (<|>) = orElse
let parseAorB = parseA <|> parseB
run parseAorB "AZ"
run parseAorB "BZ"
run parseAorB "ZAB"

let bOrElseC = parseB <|> parseC
let aAndThenBorC = parseA .>>. bOrElseC
run aAndThenBorC "AB"

let choice listParsers = List.reduce (<|>) listParsers

let anyOf listOfChars =
  listOfChars
  |> List.map pchar5
  |> choice

let parseLowercase = anyOf ['a'..'z']

let parseDigit = anyOf ['0'..'9']
run parseLowercase "aBC"
run parseLowercase "ABC"

run parseDigit "1ABC"  
run parseDigit "9ABC"  
run parseDigit "|ABC"  




let parse3Digits = parseDigit .>>. parseDigit .>>. parseDigit
run parse3Digits "123A"

let mapP f parser = 
  let innerFn input  =
    let result = run parser input
    match result with     
    | Success (value, rem) -> 
        let newValue = f value
        Success(newValue,rem)
    | Failure err ->  Failure err
  Parser innerFn

let (<!>) = mapP

let (|>>) x f = mapP f x

let parseThreeDigitsAsString =
  let tupleParser = parseDigit .>>. parseDigit .>>. parseDigit
  let transformTuple ((c1, c2), c3) = 
        String [| c1; c2; c3 |]
  mapP transformTuple tupleParser

let parseThreeDigitsAsStr = 
    (parseDigit .>>. parseDigit .>>. parseDigit)
    |>> fun ((c1, c2), c3) -> String [| c1; c2; c3 |]

run parseThreeDigitsAsStr "123A"

let parseThreeDigitsAsInt =
  mapP int parseThreeDigitsAsStr

run parseThreeDigitsAsInt "123A"

let returnP x=
  let innerFn input = Success(x, input)
  Parser innerFn


let applyP fP xP =
  fP .>>. xP
  |> mapP (fun (f,x) -> f x)
let (<*>) = applyP


    