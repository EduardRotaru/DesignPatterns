using System;
using static System.Console;

namespace FACTORIES
{
    public static class Demo
    {
        public static void FactoryExample()
        {
            var point = Point_WithFactoryPattern.Factory.NewPolarPoint(1.0, Math.PI / 2);
            WriteLine(point);

            var origin = Point_WithFactoryPattern.Origin;
        }
    }

    // Separation of concerns, SRP. Construction of an object is a separate responsability for what the object does
    // If I am doing a factory why not take it in a separate class instead of having a bunch of methods inside point 
    // Creating a Point factory and taking methods there is just better
}
