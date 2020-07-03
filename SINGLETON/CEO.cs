namespace SINGLETON
{
    // If we want a singleton why not make the class static with static members
    // Is terrible because it doesn't even have a constructor, refering by its name and not using DI
    // Monostate a variation of singleton
    // We only want one CEO and preventing people creating more than one on the other hand we want people to call the constructor and instantiate the object
    // State of the CEO being static and exposed as non-static
    public class CEO
    {
        private static string _name;
        private static int _age;

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        public int Age
        {
            get { return _age; }
            set { _age = value; }
        }

        public override string ToString()
        {
            return $"{nameof(Name)}: {Name}, {nameof(Age)}: {Age}";
        }
    }
}
