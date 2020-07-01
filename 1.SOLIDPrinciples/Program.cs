using _1.SOLIDPrinciples.DependencyInversionBad;
using _1.SOLIDPrinciples.LiskovSubstitution;
using _1.SOLIDPrinciples.OpenClosed;
using _1.SOLIDPrinciples.SingleResponsability;
using System.Diagnostics;
using static System.Console;

namespace _1.SOLIDPrinciples
{
    class Program
    {
        static void Main(string[] args)
        {
            DIP();
        }

        public static void SRP()
        {
            var j = new Journal();
            j.AddEntry("I cried today");
            j.AddEntry("I ate a bug");
            WriteLine(j);

            var p = new Persistance();
            var filename = @"d:\journal.txt";
            p.SaveToFile(j, filename, true);
            Process.Start(filename);
        }
        public static void OPC()
        {
            var apple = new Product("Apple", Color.Green, Size.Small);
            var tree = new Product("Tree", Color.Green, Size.Large);
            var house = new Product("House", Color.Blue, Size.Large);

            var products = new Product[] { apple, tree, house };
            var pf = new ProductFilter();
            WriteLine("Green products (old):");

            foreach (var p in pf.FilterByColor(products, Color.Green))
            {
                WriteLine($" - {p._name} is {p._color}");
            }

            var bf = new BetterFilter();

            WriteLine("Green products (new): ");
            foreach (var item in bf.Filter(products, new ColorSpecification(Color.Green)))
            {
                WriteLine($" - {item._name} is {item._color}");
            }

            WriteLine("Large blue items");
            foreach (var item in bf.Filter(products,
                new AndSpecification<Product>(
                    new ColorSpecification(Color.Blue),
                    new SizeSpecification(Size.Large)
                )))
            {
                WriteLine($" - {item._name} is {item._color}");
            }
        }
        public static void LSP()
        {
            var rectangle = new Rectangle(2, 3);

            WriteLine($"{rectangle} has area {LSPDemo.Area(rectangle)}");

            Square sq = new Square(); // Its perfectly fine to replace Square with parent class Rectangle but it breaks LSP (we always should be able to upcast to a base class)
            sq.Width = 4;
            WriteLine($"{sq} has area {LSPDemo.Area(sq)}");
        }
        public static void DIP()
        {
            Research.DIPBadExample();
        }
    }
}
