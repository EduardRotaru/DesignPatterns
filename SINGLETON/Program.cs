using System;
using System.Collections.Generic;
using System.IO;
using static System.Console;
using System.Linq;
using MoreLinq;

namespace SINGLETON
{
    public interface IDatabase
    {
        int GetPopulation(string name);
    }

    public class SingletonDatabase : IDatabase
    {
        private Dictionary<string, int> capitals;
        private static int instanceCount;
        public static int Count => instanceCount;

        private SingletonDatabase()
        {
            WriteLine("Initializing database");
            instanceCount++;

            capitals = File.ReadAllLines(Path.Combine(
                new FileInfo(typeof(IDatabase).Assembly.Location).DirectoryName, 
                            "Cities.txt"))
                .Batch(2)
                .ToDictionary(
                    list => list.ElementAt(0).Trim(),
                    list => int.Parse(list.ElementAt(1))
                );
        }

        public int GetPopulation(string name)
        {
            return capitals[name];
        }

        // Preventing making more instances
        //private SingletonDatabase instance = new SingletonDatabase();
        //public static SingletonDatabase Instance => Instance;

        // Improving construction of singleton database by making it lazy
        // Why? Because we might not list of capitals but we still paying the price of initialization
        //      this constructor is causing reading a file or database, need to be avoided if the client doesn't needed it

        private static Lazy<SingletonDatabase> instance = 
            // lambda for nitialization
            new Lazy<SingletonDatabase>(() => new SingletonDatabase());

        // this constructor allows you to create singletonDatabase only when we create the database
        // because thats why when I get the value
        public static SingletonDatabase Instance => instance.Value;
    }

    // Using this as a singleton provider that will use dependency injection
    public class OrdinaryDatabase : IDatabase
    {
        private Dictionary<string, int> capitals;

        private OrdinaryDatabase()
        {
            WriteLine("Initializing database");

            capitals = File.ReadAllLines(Path.Combine(
                new FileInfo(typeof(IDatabase).Assembly.Location).DirectoryName,
                            "Cities.txt"))
                .Batch(2)
                .ToDictionary(
                    list => list.ElementAt(0).Trim(),
                    list => int.Parse(list.ElementAt(1))
                );
        }

        public int GetPopulation(string name)
        {
            return capitals[name];
        }
    }


    public class SingletonRecordFinder
    {
        public int TotalPopulation(IEnumerable<string> names)
        {
            int result = 0;

            foreach (var n in names)
            {
                result += SingletonDatabase.Instance.GetPopulation(n);
            }

            return result;
        }
    }

    public class ConfigurableRecordFinder
    {
        private IDatabase _database;

        public ConfigurableRecordFinder(IDatabase database)
        {
            _database = database;
        }

        public int GetTotalPopulation(IEnumerable<string> names)
        {
            int result = 0;

            foreach (var n in names)
            {
                result += _database.GetPopulation(n);
            }

            return result;
        }
    }

    public class DummyDatabase : IDatabase
    {
        public int GetPopulation(string name)
        {
            return new Dictionary<string, int>
            {
                ["alpha"] = 1,
                ["beta"] = 2,
                ["gamma"] = 3
            }[name];
        }
    }

    public static class DemoSingleton
    {
        public static void Demo()
        {
            // No point of multiple instances because we only need to read that file once
            // We need to prevent people to call the constructor multiple times. 
            // We make constructor private
            var db = SingletonDatabase.Instance;
            WriteLine(db.GetPopulation("Tokyo"));
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            DemoSingleton.Demo();

            var ceo = new CEO();
            ceo.Name = "Adam";
            ceo.Age = 12;

            // different location but they both refer to the same data
            // mapped to static fields
            var ceo2 = new CEO();
            WriteLine(ceo2);

            // bizzare approach to singleton because this pattern approach is to prevent using the constructor but yet we allow creating new instances with static fields 

        }
    }
}
