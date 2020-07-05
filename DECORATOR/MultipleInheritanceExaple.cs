using System;

namespace DECORATOR
{
    public class MultipleInheritanceExaple
    {
        public interface IBird
        {
            void Fly();
            int Weight { get; set; }
        }

        public interface ILizard
        {
            void Crawl();
            int Weight { get; set; }
        }

        public class Bird : IBird
        {
            public int Weight { get; set; }
            public void Fly()
            {
                Console.WriteLine($"Soaring in the sky with weight {Weight}");
            }
        }

        public class Lizard : ILizard
        {
            public void Crawl()
            {
                Console.WriteLine($"Crawling in the dirt with weight {Weight}");
            }

            public int Weight { get; set; }
        }

        // Multiple inheritance 
        // DIamond inheritance problem, both classes implement the same thing 
        public class Dragon : IBird, ILizard
        {
            private Bird bird = new Bird();
            private Lizard lizard = new Lizard();

            public void Crawl()
            {
                lizard.Crawl();
            }

            public void Fly()
            {
                bird.Fly();
            }

            // we get explicit implementation
            // how getter and setter are different
            // is private because of explicit implementation
            int ILizard.Weight { get; set; }
            int IBird.Weight { get; set; }

            // satisfy interface,
            // setter is wrong, bird and lizard need to have their own property implemented corectly
            //public int Weight { get; set; }

            private int _weight;
            public int Weight
            {
                get { return Weight; }
                set
                {
                    _weight = value;
                    bird.Weight = value;
                    lizard.Weight = value;
                }
            }
        }
    }
}