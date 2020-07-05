using static System.Console;

namespace DECORATOR
{
    class Program
    {
        // static composition not great in C# typically done in c++
        // c++ supports can inherit from a generic parmaeter
        //public class ColoredShape<T> : T // CRTP can't be done in c#

        public abstract class Shape
        {
           public abstract string AsString();
        }

        public class Circle : Shape
        {
            private float radius;

            public Circle():this(0.0f)
            {
            }

            public Circle(float radius)
            {
                this.radius = radius;
            }

            public void Resize(float factor)
            {
                radius *= factor;
            }

            public override string AsString()
            {
                return $"A circle with radius {radius}";
            }
        }

        public class Square : Shape
        {
            private float side;

            public Square() : this(0.0f)
            {
                
            }

            public Square(float side)
            {
                this.side = side;
            }

            public override string AsString()
            {
                return $"A square with side {side}";
            }
        }

        public class ColoredShape : Shape
        {
            private Shape shape;
            private string color;

            public ColoredShape(Shape shape, string color)
            {
                this.shape = shape;
                this.color = color;
            }

            public override string AsString()
            {
                return $"{shape.AsString()} has the color {color}";
            }
        }

        public class TransparentShape<T> : Shape
            where T : Shape, new()
        {
            private float transparency;
            private T shape = new T();

            public TransparentShape(): this(0.0f)
            {
            }

            public TransparentShape(float transparency)
            {
                this.transparency = transparency;
            }
            public override string AsString()
            {
                return $"{shape.AsString()} has the {transparency * 100.0f}% transparency";
            }
        }

        public class ColoredShape<T> : Shape
            where T : Shape, new()
        {
            private string color;
            private T shape = new T();

            // because of the where constraint
            public ColoredShape() : this("black")
            {
            }

            public ColoredShape(string color)
            {
                this.color = color;
            }

            // How static composition works
            public override string AsString()
            {
                return $"{shape.AsString()} has the color {color}";
            }
        }

        static void Main(string[] args)
        {
            // in C# you cannot set constructor forwarding, so we can't initialize colroedShapre or square
           var redSquare = new TransparentShape<ColoredShape<Square>>(4.0f);
           WriteLine(redSquare.AsString());

           var circle = new TransparentShape<ColoredShape<Circle>>(4.0f);
            //circle.Radius doesn't work because we are using template pattern
            // we are not using inheritance
            // static approach isn't good enough in C#
           WriteLine(redSquare.AsString());
        }

        private static void DyanmicCompositionDemo()
        {
            var square = new DynamicCompositionOnDecorators.Square(1.23f);
            WriteLine(square.AsString());

            var redSquare = new DynamicCompositionOnDecorators.ColoredShape(square, "red");
            WriteLine(redSquare.AsString());

            var redHalfTransparent = new DynamicCompositionOnDecorators.TransparentShape(redSquare, 0.5f);
            WriteLine(redHalfTransparent.AsString());
        }

        private static void MultipleInheritanceDemo()
        {
            var d = new MultipleInheritanceExaple.Dragon { Weight = 123 };
            d.Fly();
            d.Crawl();
        }

        private static void AdapterDecoratorDemo()
        {
            // not efficient because string is immutable
            var s = "hello";
            s += "world";
            WriteLine(s);

            //not gonna work (Strinbuilder)
            MyStringBuilder s2 = "hello";
            s2 += "world";
            WriteLine(s2);
        }

        static void StringBuilderExample()
        {
            // how to build a decorator over a single class that cannot be inherited
            var cb = new CodeBuilder();

            cb.AppendLine("Class Foo")
                .AppendLine("{")
                .AppendLine("}");

            WriteLine(cb);
        }
    }
}
