namespace QCon.InteropFSharp

type Animal = {
  Name: string
  NumberOfLegs: int
  NumberOfWings : int option
  PreferedFood: string
}

type Order =
  | GoldOrder 
  | PlatinumOrder of string

  member this.OrderInfo =
    match this with 
    | GoldOrder -> ""
    | PlatinumOrder(extraInfo ) -> "A foamy latte"  + extraInfo

type Customer =
  | Dragon of string
  | Cat 
  | Monkey
