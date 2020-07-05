namespace DECORATOR
{
    public class DynamicCompositionOnDecorators
    {
        // are decorators composable
        // is posible to have a decorator on a decorator
        // 2 ways: dynamic/static way

        public interface IShape
        {
            string AsString();
        }

        public class Circle : IShape
        {
            public float Radius { get; set; }

            public Circle(float radius)
            {
                this.Radius = radius;
            }

            public void Resize(float factor)
            {
                Radius *= factor;
            }

            public string AsString()
            {
                return $"A circle with radius {Radius}";
            }
        }

        public class Square : IShape
        {
            private float side;

            public Square(float side)
            {
                this.side = side;
            }

            public string AsString()
            {
                return $"A square with side {side}";
            }
        }

        // dynamic decorator, making them at runtime than compile time
        public class ColoredShape : IShape
        {
            private IShape shape;
            private string color;

            public ColoredShape(IShape shape, string color)
            {
                this.shape = shape;
                this.color = color;
            }

            public string AsString()
            {
                return $"{shape.AsString()} has the color {color}";
            }
        }

        public class TransparentShape : IShape
        {
            private IShape shape;
            public float transparency;

            public TransparentShape(IShape shape, float transparency)
            {
                this.shape = shape;
                this.transparency = transparency;
            }

            public string AsString()
            {
                return $"{shape.AsString()} has the  {transparency * 100.0} transparency";
            }
        }
    }
}