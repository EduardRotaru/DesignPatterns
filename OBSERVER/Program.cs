using System;
using static System.Console;

namespace OBSERVER
{
    public class FallsIllEventArgs
    {
        public string Address;
    }

    public class Person
    {
        public void CatchACold()
        {
            // this depends on what signature the event has
            FallsIll?.Invoke(this, new FallsIllEventArgs { Address = "123 london"});
        }

        public event EventHandler<FallsIllEventArgs> FallsIll;
    }

    class Program
    {
        static void Main(string[] args)
        {
           RxSamples.Demo2();
        }

        private static void CallDoctor(object sender, FallsIllEventArgs eventArgs)
        {
            WriteLine($"A doctor has been called to {eventArgs.Address}");
        }

        private static void EventsDemo()
        {
            var person = new Person();

            // who and where fire this event
            person.FallsIll += CallDoctor;

            person.CatchACold();

            person.FallsIll -= CallDoctor;
        }
    }
}
