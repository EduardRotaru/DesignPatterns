using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FACTORIES
{
    // No Longer required once we get the factory
    public enum CoordinateSystem
    {
        Cartesian,
        Polar
    }

    class Point_WithoutFactoryPattern
    {
        private double _x, _y;

        // Constructor needs to match class name
        // Initialize a point from EITHER cartesian or polar
        // x if cartesian, rho if polar
        // 
        public Point_WithoutFactoryPattern(double a, double b,
            CoordinateSystem system = CoordinateSystem.Cartesian)
        {
            switch (system)
            {
                case CoordinateSystem.Cartesian:
                    _x = a;
                    _y = b;
                    break;
                case CoordinateSystem.Polar:
                    _x = a * Math.Cos(b);
                    _y = a * Math.Sin(b);
                    break;
                default:
                    break;
            }
        }

        //public double Point(double rho, double theta) { } // cannot do this
    }
}
