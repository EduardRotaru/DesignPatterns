using static System.Console;

namespace BUILDER
{
    public class Person
    {
        public string _name;
        public string _position;

        public class Builder : PersonJobBuilder<Builder>
        {

        }

        public static Builder New => new Builder();

        public override string ToString()
        {
            return $"{nameof(_name)}: {_name}, {nameof(_position)}: {_position}";
        }
    }

    public abstract class PersonBuilder
    {
        protected Person _person = new Person();

        public Person Build()
        {
            return _person;
        }
    }

    // SELF REFERS TO THE OBJ INHERITING FROM THE OBJ
    // class Foo : Bar<Foo> in this case the SELF argument refers to the class doing the inheritance
    // 
    public class PersonInfoBuilder<SELF>
        : PersonBuilder
        where SELF : PersonInfoBuilder<SELF>
    { 
        public SELF Called(string name)
        {
            _person._name = name;
            return (SELF)this;
        }
    }

    public class PersonJobBuilder<SELF>
        : PersonInfoBuilder<PersonJobBuilder<SELF>>
        where SELF : PersonJobBuilder<SELF>
    {
        public SELF WorkAsA(string position)
        {
            _person._position = position;
            return (SELF)this;
        }
    }

    public class FluentBuilder
    {
        public static void FluentBuilderDemo()
        {
           var me =  Person.New
                .Called("dimitri")
                .WorkAsA("quant")
                .Build();

            WriteLine(me);
        }
    }

    // WorkAsA isn't working because when we call the Called method we return a PersonInfoBuilder
    // and PersonInfoBuilder doesn't know anything about WorkAsA because is not part of its inheritance hierarchy.
    // PersonBuilder just gives you an interface of PersonInfoBuilder.

    // Problem with inheritance on fluent interfaces we are not allowed to use the containing the type as the returning type
    // it makles no sense, if you were to do this then eventually something calls this method you are degrading your builder
    // from a PersonJobBuilder to a PersonInfoBuilder

    // to fix this, we need to replace return type with a recursive generic.

    // Previously what happened when we used Called method, what happened Called method returned a personinfobuilder
    // Whats happening now is that we get the right type when creating a variable through the inheritance chain. 
    // Inheriting fluent interfaces is difficult and it requires making generic classes.

}
