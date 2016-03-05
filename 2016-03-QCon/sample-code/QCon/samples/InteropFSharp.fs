namespace QCon.InteropFSharp

type Animal = {
  Name: string
  NumberOfLegs: int
  NumberOfWings : int option
}

type Order =
  | GoldOrder 
  | PlatinumOrder of string 
  | Enterprise

  member this.OrderInfo =
    match this with 
    | GoldOrder -> ""
    | PlatinumOrder(extraInfo ) -> "A foamy latte"

type Customer =
  | DragonCustomer of string
  | Cat
  | Monkey
