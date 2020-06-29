using System;
using System.Collections.Generic;

namespace _1.SOLIDPrinciples.OpenClosed
{
    public enum Color
    {
        Red, Green, Blue
    }

    public enum Size
    {
        Small, Medium, Large, Huge
    }

    public class Product
    {
        public string _name;
        public Color _color;
        public Size _size;

        public Product(string name, Color color, Size size)
        {
            _name = name ?? throw new ArgumentNullException(paramName: nameof(name));
            _color = color;
            _size = size;
        }
    }

    public class ProductFilter
    {
        public IEnumerable<Product> FilterBySize(IEnumerable<Product> products, Size size)
        {
            foreach (var p in products)
            {
                if (p._size == size)
                     yield return p;
            }
        }

        // Adding this
        // Only duplication and renaming
        public IEnumerable<Product> FilterByColor(IEnumerable<Product> products, Color color)
        {
            foreach (var p in products)
            {
                if (p._color == color)
                    yield return p;
            }
        }

        // Adding again in the class
        // More complex than duplication
        public IEnumerable<Product> FilterByColorAndSize(IEnumerable<Product> products, Size size, Color color)
        {
            foreach (var p in products)
            {
                if (p._size == size && 
                    p._color == color)
                    yield return p;
            }
        }

        // This class should be open for extension but closed for modification
        // We need to implement a pattern called Specification Pattern
    }

    // Implements Specific pattern dictates wether or not a product fulfill any criteria
    // Think about ISpecification as a private predicate that operates on any type T
    public interface ISpecification<T>
    {
        bool IsSatisfied(T t);
    }

    public interface IFilter<T>
    {
        IEnumerable<T> Filter(IEnumerable<T> items, ISpecification<T> spec);
    }

    public class ColorSpecification : ISpecification<Product>
    {
        private Color _color;

        public ColorSpecification(Color color)
        {
            _color = color;
        }

        public bool IsSatisfied(Product p)
        {
            return p._color == _color;
        }
    }

    public class BetterFilter : IFilter<Product>
    {
        public IEnumerable<Product> Filter(IEnumerable<Product> items, ISpecification<Product> spec)
        {
            foreach (var item in items)
            {
                if (spec.IsSatisfied(item))
                {
                    yield return item;
                }
            }
        }
    }

    public class SizeSpecification : ISpecification<Product>
    {
        private Size _size;

        public SizeSpecification(Size size)
        {
            _size = size;
        }

        public bool IsSatisfied(Product t)
        {
            return _size == t._size;
        }
    }

    // using a combinator
    public class AndSpecification<T> : ISpecification<T>
    {
        ISpecification<T> _first, _second;

        public AndSpecification(ISpecification<T> first, ISpecification<T> second)
        {
            _first = first ?? throw new ArgumentNullException(paramName: nameof(first));
            _second = second ?? throw new ArgumentNullException(paramName: nameof(second));
        }

        public bool IsSatisfied(T t)
        {
            return _first.IsSatisfied(t) && _second.IsSatisfied(t);
        }
    }
}
