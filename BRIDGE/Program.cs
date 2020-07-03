using static System.Console;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;

namespace BRIDGE
{
    public interface IRenderer
    {
        void RenderCircle(float radius);
    }

    public class VectorRenderer : IRenderer
    {
        public void RenderCircle(float radius)
        {
            WriteLine($"Drawing a circle of radius {radius}");
        }
    }

    public class RasterRenderer : IRenderer
    {
        public void RenderCircle(float radius)
        {
            WriteLine($"Drawing pixels for circle with radius {radius}");
        }
    }

    // Build a bridge
    public abstract class Shape
    {
        protected IRenderer renderer;

        protected Shape(IRenderer renderer)
        {
            this.renderer = renderer;
        }

        public abstract void Draw();
        public abstract void Resize(float factor);
    }

    public class Circle : Shape
    {
        private float radius;

        public Circle(IRenderer renderer, float radius) 
            : base(renderer)
        {
            this.radius = radius;
        }

        public override void Draw()
        {
            renderer.RenderCircle(radius);
        }

        public override void Resize(float factor)
        {
            radius *= factor;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            //IRenderer renderer = new RasterRenderer();
            var renderer = new VectorRenderer();
            var circle = new Circle(renderer, 5);

            circle.Draw();
            circle.Resize(2);
            circle.Draw();

            // DI container better
            var cb = new ContainerBuilder();
            cb.RegisterType<VectorRenderer>()
                .As<IRenderer>()
                .SingleInstance();

            cb.Register((c, p)
                => new Circle(c.Resolve<IRenderer>(),
                   p.Positional<float>(0)));

            using (var c = cb.Build())
            {
                var circle2 = c.Resolve<Circle>(
                    new PositionalParameter(0, 5.0f)
                );

                circle2.Draw();
                circle2.Resize(2);
                circle2.Draw();
            }
        }

        // Bridge pattern is nothing more than a way of connecting parts of the system
        // eg circle connecting to the different parts like drawing 
    }
}
