using static System.Console;
using System.Collections.Generic;
using System.Linq;


namespace _1.SOLIDPrinciples.DependencyInversionBad
{
    public enum Relationship
    {
        Parent, Child, Sibling
    }

    public class Person
    {
        public string Name;
    }

    public class Relationships : IRelationshipBrowser
    {
        private List<(Person, Relationship, Person)> relations
            = new List<(Person, Relationship, Person)>();

        public void AddParentChild(Person parent, Person child)
        {
            relations.Add((parent, Relationship.Parent, child));
            relations.Add((child, Relationship.Parent, parent));
        }

        // We have a low level module now
        // We depend on an abstraction which is the itnerface
        public IEnumerable<Person> FindAllChildren(string name)
        {
            foreach (var r in relations.Where(
                x => x.Item1.Name == name &&
                     x.Item2 == Relationship.Parent))
            {
                yield return r.Item3;
            }
        }

        //public List<(Person, Relationship, Person)> Relations => relations;
    }

    public interface IRelationshipBrowser
    {
        IEnumerable<Person> FindAllChildren(string name);
    }

    // high level module
    public class Research
    {
        //public Research(Relationships relationships)
        //{
        //    var relations = relationships.Relations;
        //    foreach (var r in relations.Where(
        //        x => x.Item1.Name == "John" &&
        //             x.Item2 == Relationship.Parent))
        //    {
        //        WriteLine($"John has a child called {r.Item3.Name}");
        //    }
        //}

        public Research(IRelationshipBrowser browser)
        {
            foreach (var r in browser.FindAllChildren("John"))
                WriteLine($"John has a child called {r.Name}");
        }

        public static void DIPBadExample()
        {
            var parent = new Person { Name = "John" };
            var child1 = new Person { Name = "Bart" };
            var child2 = new Person { Name = "Mary" };

            var relationships = new Relationships();
            relationships.AddParentChild(parent, child1);
            relationships.AddParentChild(parent, child2);

            new Research(relationships);
        }
    }
    // We are accessing a very low level part of the relationships class
    // We are accessing its data store, specifically design way that is exposing private field as public
    // In practical way: Relationships cannot change its way on how to store the relationships
    // Class cannot change its way the way its storing data.

    // No low level dependency and relations field can be changed the way it stores the data structure because its no longer dependent on high level modules which are actually consuming it
}
