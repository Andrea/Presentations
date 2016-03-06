using System;
using Microsoft.FSharp.Core;
using QCon.InteropFSharp;

namespace QConConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            // Instanciating an F# Record
            var cat = new Animal("Cat", 4, FSharpOption<int>.None);


            // Using a discriminated union
            var platinumOrder = Order.NewPlatinumOrder( "Something");
            Console.WriteLine("Is platinum? " +platinumOrder.IsPlatinumOrder);
            Console.WriteLine("Extra info " +platinumOrder.OrderInfo);
            
            var goldOrder = Order.GoldOrder;
            Console.WriteLine("Is platinum? " + goldOrder.IsPlatinumOrder);

            //Another Discriminated uniong without the helper
            var customer = Customer.NewDragon("Dragon Treats"); 
            Console.WriteLine("Is a cat? " + customer.IsCat);
            // no access to dragon customer info

        }
    }
}
