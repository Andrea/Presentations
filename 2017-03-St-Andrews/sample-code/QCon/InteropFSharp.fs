namespace QCon.InteropFSharp

// A record type. named values...Immutable, structural equality for free
type Animal = {
  Name: string
  NumberOfLegs: int
  NumberOfWings : int option
  PreferedFood: string
}
// create a cat
//{Name="cat";NumberOfLegs=4;NumberOfWings= None ;PreferedFood= "chicken"};;
 

// Discriminated Union. can have values
type Order =
  | GoldOrder 
  | PlatinumOrder of string

  // expose OrderInfo so that C# can access this
  member this.OrderInfo =
    match this with 
    | GoldOrder -> ""
    | PlatinumOrder(extraInfo) -> "A foamy latte"  + extraInfo

type Customer =
  | Dragon of string
  | Cat 
  | Monkey

