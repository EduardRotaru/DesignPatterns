using System;
using System.Collections.Generic;

namespace PROXY
{
    // property proxy the idea is using an object as a proxy instead of a literal value
    public class Property<T> : IEquatable<Property<T>> where T : new()
    {
        private T value;

        public T Value
        {
            // in our model we want to avoid duplicate assigments
            get => value;
            set
            {
                if (Equals(this.value, value)) return;
                Console.WriteLine($"assigning value to {value}");
                this.value = value;
            }
        }

        public Property() :this(default(T))
        //: this(Activator.CreateInstance<T>())
        // :this(default(T)) Default of T would give a null of a reference type
        // it can be a reference type I might aswell intializes with the default of type T
        {
        }

        public Property(T value)
        {
            this.value = value;
        }

        // implicit conversions to T
        public static implicit operator T(Property<T> property)
        {
            return property.value; // int n = p_int;
        }

        // reverse case
        public static implicit operator Property<T>(T value)
        {
            return new Property<T>(value); // Property<int> p = 123;
        }

        // Generated code
        // there are caviats here like hashcode, non readonly value, the object value changes the hashcode will change
        // it can't be null if we want to return value
        public bool Equals(Property<T> other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return EqualityComparer<T>.Default.Equals(value, other.value);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Property<T>) obj);
        }

        public override int GetHashCode()
        {
            return EqualityComparer<T>.Default.GetHashCode(value);
        }

        public static bool operator ==(Property<T> left, Property<T> right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(Property<T> left, Property<T> right)
        {
            return !Equals(left, right);
        }
    }
}