using static System.Console;

namespace BUILDER
{
    // Sometimes we want multiple builders to build several different aspects of a particular object.

    public class Person2
    {
        public string _streetAddress, _postcode, _city;
        public string _companyName, _position;
        public int _anualIncome;

        public override string ToString()
        {
            return $"{nameof(_streetAddress)}: {_streetAddress}" +
                $"{nameof(_postcode)}: {_postcode}" +
                $"{nameof(_city)}: {_city}" +
                $"{nameof(_companyName)}: {_position}" +
                $"{nameof(_anualIncome)}: {_anualIncome}";
        }
    }

    public class PersonBuilder2 // builder
    {
        // reference
        protected Person2 _person = new Person2();

        public PersonJobBuilder Works => new PersonJobBuilder(_person);
        public PersonAddressBuilder Lives => new PersonAddressBuilder(_person);

        public static implicit operator Person2(PersonBuilder2 pb) => pb._person;
    }

    public class PersonAddressBuilder : PersonBuilder2
    {
        public PersonAddressBuilder(Person2 person)
        {
            _person = person;
        }

        public PersonAddressBuilder At(string streetAddress)
        {
            _person._streetAddress = streetAddress;
            return this;
        }

        public PersonAddressBuilder WithPostCode(string postcode)
        {
            _person._postcode = postcode;
            return this;
        }

        public PersonAddressBuilder In(string city)
        {
            _person._city = city;
            return this;
        }
    }

    public class PersonJobBuilder : PersonBuilder2
    {
        public PersonJobBuilder(Person2 person)
        {
            _person = person;
        }

        public PersonJobBuilder At(string companyName)
        {
            _person._companyName = companyName;
            return this;
        }

        public PersonJobBuilder AsA(string position)
        {
            _person._position = position;
            return this;
        }

        public PersonJobBuilder Earning(int amount)
        {
            _person._anualIncome = amount;
            return this;
        }
    }

    public class FacedBuilder
    {
        public static void Demo()
        {
            var pb = new PersonBuilder2();
            Person2 person = pb
                .Lives.At("123 crystal palace")
                      .In("London")
                      .WithPostCode("SQW12C")
                .Works.At("Fabrikam")
                      .AsA("Engineer")
                      .Earning(12300);

            WriteLine(person);
        }
    }
    // built a facade a component that hides a lot of information behind.
    // builders could be inner classes of PersonBuilder that hide their internals from the consumer

}
