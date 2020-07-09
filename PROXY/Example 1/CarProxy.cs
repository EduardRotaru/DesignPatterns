using System;

namespace PROXY
{
    // sinthetic example of a protection proxy, checks if you the right to call the right method 

    // E.g here is the person is older enough to drive 

    // Examples in ASP.NET if the current user have the access rights to call a particular method 
    // or access a particular REST resource you simply make a proxy component and replicate the API like decorator
    // the key thing here is we don't add new members just new functionality 

    public class CarProxy : ICar
    {
        private Driver Driver;
        private Car Car = new Car();

        public CarProxy(Driver driver)
        {
            this.Driver = driver;
        }
        public void Drive()
        {
            if (Driver.Age >= 18)
            {
                Car.Drive();
            }
            else
            {
                Console.WriteLine("To young");
            }
        }
    }
}