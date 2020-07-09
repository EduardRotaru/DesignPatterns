using System;

namespace PROXY
{
    public struct Percentange : IEquatable<Percentange>
    {
        public bool Equals(Percentange other)
        {
            return value.Equals(other.value);
        }

        public override bool Equals(object obj)
        {
            return obj is Percentange other && Equals(other);
        }

        public override int GetHashCode()
        {
            return value.GetHashCode();
        }

        public static bool operator ==(Percentange left, Percentange right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Percentange left, Percentange right)
        {
            return !left.Equals(right);
        }

        private readonly float value;

        internal Percentange(float value)
        {
            this.value = value;
        }

        public static float operator *(float f, Percentange p)
        {
            return f * p.value;
        }

        public static Percentange operator +(Percentange a, Percentange b)
        {
            return new Percentange(a.value + b.value);
        }

        public override string ToString()
        {
            return $"{value * 100}%";
        }
    }
}