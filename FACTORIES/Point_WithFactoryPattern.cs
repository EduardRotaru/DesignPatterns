using System;

namespace FACTORIES
{
    // Factory method
    // Advancements: 
    // 1. We get to have an overload with the same set of arguments but they have different descriptive names so api tells you what arguments you are providing 
    // 2. The names of the factory names are unique, giving a suggestion what kind of point we are creating
    public class Point_WithFactoryPattern
    {
        // Breaks SRP having factories inside Point
        //public static Point NewCartesianPoint(double x, double y) 
        //    => new Point(x, y);

        //public static Point NewPolarPoint(double rho, double theta) 
        //    => new Point(rho * Math.Cos(theta), rho * Math.Sin(theta));

        private double _x, _y;

        // we can make it private if we use it only in this assembly and have an external class
        private Point_WithFactoryPattern(double x, double y)
        {
            _x = x;
            _y = y;
        }

        public override string ToString()
        {
            return $"{nameof(_x)}: {_x}, {nameof(_y)}: {_y}";
        }

        // Highlights confusion
        public static Point_WithFactoryPattern Origin => new Point_WithFactoryPattern(0, 0); // property
        // yielding fixed values without a getter or setter
        public static Point_WithFactoryPattern Origin2 = new Point_WithFactoryPattern(0, 0); // field and better

        //public static Factory PFactory => new Factory();

        public static class Factory
        {
            // Initial problem is that the constructor of Point is private and cannot be an outside class
            // Making constructor public will allow to create a new instance of Point without rho and theta so its not desirable
            public static Point_WithFactoryPattern NewCartesianPoint(double x, double y)
                => new Point_WithFactoryPattern(x, y);

            public static Point_WithFactoryPattern NewPolarPoint(double rho, double theta)
                => new Point_WithFactoryPattern(rho * Math.Cos(theta), rho * Math.Sin(theta));
        }
    }

    // Separation of concerns, SRP. Construction of an object is a separate responsability for what the object does
    // If I am doing a factory why not take it in a separate class instead of having a bunch of methods inside point 
    // Creating a Point factory and taking methods there is just better
}
