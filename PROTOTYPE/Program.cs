using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Serialization;
using static System.Console;

namespace PROTOTYPE
{
    class Program
    {
        static void Main(string[] args)
        {
            DemoPrototype.Demo();
        }
    }

    // Interfaces mean we need to feed every single object participating in the serialization and tag with this marker interface
    //public interface IPrototype<T>
    //{
    //    T DeepCopy();
    //}

    public static class ExtensionMethods
    {
        public static T DeepCopy<T>(this T self)
        {
            var stream = new MemoryStream();
            var formater = new BinaryFormatter();

            formater.Serialize(stream, self);
            stream.Seek(0, SeekOrigin.Begin);
            object copy = formater.Deserialize(stream);
            stream.Close();

            return (T)copy;
        }

        public static T DeepCopyXml<T>(this T self)
        {
            using (var ms = new MemoryStream())
            {
                var s = new XmlSerializer(typeof(T));
                s.Serialize(ms, self);
                ms.Position = 0;
                return (T)s.Deserialize(ms);
            }
        }
    }

    //[Serializable]
    public class Person
    //: IPrototype<Person>
    //: ICloneable
    // https://stackoverflow.com/questions/21116554/proper-way-to-implement-icloneable
    {
        public string[] _names;
        public Address _address;

        public Person()
        {
        }

        public Person(string[] names, Address address)
        {
            _names = names;
            _address = address;
        }

        // copy constructor
        //public Person(Person other)
        //{
        //    _names = other._names;
        //    _address = new Address(other._address);
        //}

        // returns object
        //public object Clone()
        //{
        //    //_names.Clone // shallow copies on the array tells in the signature of Clone()
        //    return new Person(_names, (Address)_address.Clone());
        //}

        //public Person DeepCopy()
        //{
        //    return new Person(_names, _address.DeepCopy());
        //}

        public override string ToString()
        {
            return $"{nameof(_names)}: {string.Join(" ", _names)}, {nameof(_address)}: {_address}";
        }
    }
    //[Serializable]
    public class Address
    //: IPrototype<Address>
    //: ICloneable
    {
        public string _streetName;
        public int _houseNumber;

        public Address()
        {
        }

        public Address(string streetName, int houseNumber)
        {
            _streetName = streetName;
            _houseNumber = houseNumber;
        }

        // copy constructor
        //public Address(Address other)
        //{
        //    _streetName = other._streetName;
        //    _houseNumber = other._houseNumber;
        //}

        //public object Clone()
        //{
        //    return new Address(_streetName, _houseNumber);
        //}


        //public Address DeepCopy()
        //{
        //    return new Address(_streetName, _houseNumber);
        //}

        public override string ToString()
        {
            return $"{nameof(_streetName)}: {_streetName}, {nameof(_houseNumber)}: {_houseNumber}";
        } 
    }

    public static class DemoPrototype
    {
        public static void Demo()
        {
            var john = new Person(new[] { "John", "Smith" },
                new Address("Londong Road", 123));

            //var jane = john;
            //jane._names[0] = "Jane";

            //WriteLine(john);
            //WriteLine(jane); // modifying same reference

            //var jane2 = (Person)john.Clone();
            //jane2._address._houseNumber = 321; // both have 321 
            //// we made a shallow copy, simply copied the reference
            ///

            // Deep Constructor
            //var jane2 = new Person(john);
            //jane2._address._houseNumber = 321;

            //WriteLine(jane2);

            // this is a lot of work if there are too many classes
            //var jane3 = john.DeepCopy();
            //jane3._address._houseNumber = 3213;

            //WriteLine(jane3);

            var jane4 = john.DeepCopyXml();
            jane4._names[0] = "Jane";
            jane4._address._houseNumber = 77;

            WriteLine(john);
            WriteLine(jane4);

        }
    }

    // How do we copy objects generally
    // IClonable
    // Problem is we never know if is shallow cloning or decloning(copying in set fields or references recurseviley)

    // Implementing this copying logic is really tedious 
    // Serialization in the real world

    // we get an exception
    // we are not using anymore interfaces to go through the hierarchies anymore and modify every single class
    // every type we try to serialize needs to be serializable.

    // on xml serialization we get an exception because we explicitily need to have a parameterless constructor

    // there are requirements that need to be satisfied by classes for some serializers
}
