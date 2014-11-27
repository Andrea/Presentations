- title : F# and open source
- description : Introduction to FsReveal
- author : Andrea Magnorsky
- theme : solarized
- transition : default

***
<div style=".state-background {
    background-image: images/fsharp_logo.png;
} ">

## MTUG Dublin
## F# and Open Source

### Andrea Magnorsky

Digital Furnace Games ␣ ▀ ␣ BatCat Games ␣ ▀ ␣ GameCraft Foundation

- @SilverSpoon 
- [roundcrisis.com](roundcrisis.com)


---

#### Working on OniKira: Demon Killer 

Available on Steam Early Access




<iframe width="853" height="480" src="//www.youtube.com/embed/8OH31zfRlDs?rel=0" frameborder="0" allowfullscreen></iframe>

---
***

### F# OSS and you
####

- F# Open sourced 4 years ago. [Announced by Don Syme](http://blogs.msdn.com/b/dsyme/archive/2010/11/04/announcing-the-f-compiler-library-source-code-drop.aspx)
- F# Software Foundation at [fsharp.org](http://fsharp.org/)
- User groups growth 
- 100+ pull requests for F# 4.0

<img src="images/fsharp_logo.png" alt="fs" style="width: 200px;"/>

---
###
<iframe width="1024" height="768" src="//fsharp.org" frameborder="0" allowfullscreen></iframe>
---
***

### Latest announcements, Big wins !

- Great for language adoption
- Visual Studio community with extensions
- CLR in a package FTW

***

    // one-liners
    [1..100] |> List.sum |> printfn "sum=%d"

    // no curly braces, semicolons or parentheses
    let square x = x * x
    let sq = square 42 

    // simple types in one line
    type Person = {First:string; Last:string}

    // complex types in a few lines
    type Employee = 
      | Worker of Person
      | Manager of Employee list

    // type inference
    let jdoe = {First="John";Last="Doe"}
    let worker = Worker jdoe

Visit **F# for Fun and Profit** for more examples

***
#### Some examples

Light syntax, POCOs 

---

#### C#

    [lang=cs]
    public class Person
    {
        public Person(string name, int age)
        {
            _name = name;
            _age = age;
        }

        private readonly string _name;
        private readonly int _age;

        public string Name
        {
            get { return _name; }
        }

        public int Age
        {
            get { return _age; }
        }
    }

---

#### F# 

    type Person( name:string, age:int) =

    /// Full name
    member person.Name = name

    /// Age in years
    member person.Age = age


---


***

#### Some examples

Testing

---

#### C#

    [lang=cs]
    using NUnit.Framework;

    [TestFixture]
    public class MathTest
    {
        [Test]
        public void TwoPlusTwoShouldEqualFour()
        {
            Assert.AreEqual(2 + 2, 4);
        }
    }


---

#### F# 

    module MathTest =

    open NUnit.Framework

    let [<Test>] ``2 + 2 should equal 4``() =
        Assert.AreEqual(2 + 2, 4)



---
#### FsCheck 

    module MathTest =

    open FsCheck.Nunit

    [<Property>]
    let ``Given a, a + a should equal a * 2`` ( a: int) =
        a + a = 2 * a


You can use FsCheck from F# and C#.

---



***

#### Some examples

C# <-> F# Interop

---

#### C# consuming F# code

Use namespaces in F# or prefix with global::YourModuleName

    [lang=cs]
    using System;
    class Program
    {
        static void Main(string[] args)
        {
            var s = Calculator.Calc.add("4 4", "+");
            Console.WriteLine("The sum is {0}", s);
        }
    }
and the F# side

    namespace Calculator
    module Calc =

        open System
        let add numbers delimiter =    
            // Do stuff to add numbers            



---

#### F# consuming C# code

    module MathTest =

    open NUnit.Framework

    let [<Test>] ``2 + 2 should equal 4``() =
        Assert.AreEqual(2 + 2, 4)



---



***


**Like games?** Dublin GameCraft on the 6th of December @ Microsoft

F# and games workshop from 10am to noon.

<img src="images/gamecraft-logo.png" alt="GC" style="width: 200px;"/>


**Functional Kats** Monthly meetup 
![FK](images/fk.jpeg)  


***

### Thanks :D

![onikira](images/onikira.jpg)

- @SilverSpoon 
- [roundcrisis.com](roundcrisis.com)

</div>