using PROXY.DynamicProxy;
using static System.Console;

namespace PROXY
{
    class Program
    {
        static void Main(string[] args)
        {
            // We can substitute a Car with CarProxy because it have the same API
            //ICar car = new CarProxy(new Driver { Age = 12 });
            //car.Drive();

            //Why();
            //ExampleValueProxy();
            DynamicProxyMethod();
        }

        // lets suppose why
        static void Why()
        {
            var c = new Creature();
            c.Agility = 10;
            // c.set_agility(10) its not going to happen
            // In C# you cannot overload the assigment operator
            // what happens with assigment we are using an implicit conversion from T
            // an implicit conversation makes a new object instead of changing existing one
            // so you are replacing with a new property
            // Esentially we are doing c.Agility = new Property<int>(10)

            // In C# to achieve this we need to rewrite the property as a combination of field and property

            // calling it twice
            c.Agility = 10;

            // imiplicit implementation only assigns once
            // limitations typically 
        }

        // A value proxy is a proxy typically constructed over a primitive type
        // Why over an int or float, you want stronger typing
        static void Foo(int price) // cannot explicitily say what x is, can be negative
        {
        }

        // ex2 storing percentanges
        // 50% = 0.5 100
        static void ExampleValueProxy()
        {
            WriteLine(10f * 5.Percent());
            WriteLine(2.Percent() + 5.Percent()); // 5%
        }

        // combination of proxy and composite design patterns
        // allows you to implement a pattern or a construct which is called 
        static void CompositeProxy()
        {
            // every single creature increase the X to move them 
            var creatures = new Creature2[100]; // ARRAY OF STRUCTURES

            // Age X Y Age X Y Age X Y - cpu has to jump on X's

            // Much faster, how can we implement this
            // Age Age Age Age
            // X X X X
            // Y Y Y Y
            foreach (var c in creatures)
            {
                // is not memory efficient
                // modern cpus like data next to each other in a predictable sequence 
                c.X++;
            }

            var creatures2 = new Creatures(100); // STRUCTURE OF ARRAYS
            // AOS/SOA DUALITY REPRESENT THINGS ONE WAY OR ANOTHER, in other languages is implemented implicitily 
            // We can build proxies that pretty much do the same thing.
            // type of c is Creatures.CreatureProxy
            foreach (var c in creatures2)
            {
                // modify each one of the creatures
                // we modify something on the proxy

                // I am referencing creatures.x[index] and use thisr eference to increment the value
                c.X++;
            }
        }

        static void DynamicWithoutProxyMethod()
        {
            var ba =  new BankAccount();

            ba.Deposit(100);
            ba.Withdraw(50);

            WriteLine(ba);
        }

        // we make a dynamic proxy which is going to log a bank account but its gonna return an interface of IBankAccount.
        // It gives you the interface from an dynamic object which proxies the call from the actual subject 
        static void DynamicProxyMethod()
        {
            var ba = Log<BankAccount>.As<IBankAccount>();

            ba.Deposit(100);
            ba.Withdraw(50);

            WriteLine(ba);

            // how to expose the actual object
            WriteLine(ba);
        }

        // Dynamic costs can be big but we are getting the flexibility and don't have to reinplement the same interface members I can just intercept using TryInvokeMember
        // trick with ImpromptuInterface is that allows you to have a dynamic object acting as its implementing a particular interface
    }
}
 