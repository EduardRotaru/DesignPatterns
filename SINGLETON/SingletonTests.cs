using Autofac;
using NUnit.Framework;

namespace SINGLETON
{
    // testing against a live database is a bad idea
    // singleton binds us to a live database 
    [TestFixture]
    public class SingletonTests
    {
        [Test]
        public void IsSingletonTest()
        {
            var db = SingletonDatabase.Instance;
            var db2 = SingletonDatabase.Instance;

            Assert.That(db, Is.SameAs(db2));
            Assert.That(SingletonDatabase.Count, Is.EqualTo(1));
        }

        [Test]
        public void SingletonPopulationTest()
        {
            var rf = new SingletonRecordFinder();
            var names = new[] { "Seol", "Mexico City" };
            int tp = rf.TotalPopulation(names);

            Assert.That(tp, Is.EqualTo(111111 + 120000));
        }

        // Everything is ok for now except that we are testing on a live database.
        // First of all need to look in the database for values
        // Someone removes one city in db, test will fail 
        // Fake the database but we cannot get it, record finder have a hardcoded reference to the instance, the problem with singleton, we hardcode a reference everywhere and we cannot substitute this

        [Test]
        public void ConfigurablePopulationTest()
        {
            var rf = new ConfigurableRecordFinder(new DummyDatabase());
            var names = new[] { "alpha", "gamma" };
            int tp = rf.GetTotalPopulation(names);

            Assert.That(tp, Is.EqualTo(4));
        }

        // In real world instead of building singleton yourself you delegate responsability for having something in singleton form through DI
        [Test]
        public void DIPopulationtest()
        {
            // Make ContainerBuilder - Autofac
            var cb = new ContainerBuilder();

            // Register container ConfigurableRecordFinder
            // Is going to be once per request, brand new instance
            // we register type as a singleton
            cb.RegisterType<OrdinaryDatabase>()
                .As<IDatabase>()
                .SingleInstance(); // important
            cb.RegisterType<ConfigurableRecordFinder>();

            using ( var c = cb.Build())
            {
                // is initialized in constructor with a singleton instance of IDatabase
                // but it could be easily be DummyDatabase too
                var rf = c.Resolve<ConfigurableRecordFinder>();
            }
        }
    }
}
