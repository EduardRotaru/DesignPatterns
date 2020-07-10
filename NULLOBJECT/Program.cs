using System;
using System.Dynamic;
using Autofac;
using ImpromptuInterface;
using JetBrains.Annotations;
using static System.Console;


namespace NULLOBJECT
{
    class Program
    {
        // how a typical API will handle if we pass a null?
            // [CanBeNull] annotation so we can pass a null
                // So every call needs to be a safe call .? this requires a upfront design
                    // 1, deal with upfront
                    // 2. DI doesn't like passing nulls
        // lets say there is no upfront design so no CanBeNull decorator and even throw exception
        // we have to implement null pattern
        // we need to provide a log that does absolutely nothing

        static void Main(string[] args)
        {
            var cb = new ContainerBuilder();

            // We have to specify the argument for BankAccount. We will break the OCP if we change BankAccount class but don't have an argument log
            // passing null will throws null exception

            //var log = new ConsoleLog();
            //var ba = new BankAccount(log);
            //ba.Deposit(100);

            // registry the type of bankaccount and then build the container

            //cb.RegisterType<BankAccount>();
            //cb.RegisterInstance((ILog) null);

            //cb.Register(ctx => new BankAccount(null)); // works but..

            cb.RegisterType<BankAccount>();
            cb.RegisterType<NullLog>().As<ILog>();

            using (var c = cb.Build())
            {
                var ba = c.Resolve<BankAccount>();
                ba.Deposit(1000);
            }

            // DLR has a performance hit here
            var log = Null<ILog>.Instace; // null ILog
            log.Info("asdsadsa");
            var ba2 = new BankAccount(log);
            ba2.Deposit(100);
            // does nothing
        }
    }

    public class BankAccount
    {
        private ILog log;
        private int balance;

        public BankAccount([CanBeNull] ILog log)
        {
            this.log = log;
        }

        public void Deposit(int amount)
        {
            balance += amount;
            // ?. to make sure things are invoked correctly
            log?.Info($"Deposited {amount}, balance is now {balance}");

            // upfront design
            // problem 1: everything done upfront
            // problem 2: DI doesn't like providing nulls
        }
    }

    public interface ILog
    {
        void Info(string msg);
        void Warn(string msg);
    }

    class ConsoleLog : ILog
    {
        public void Info(string msg)
        {
            WriteLine(msg);
        }

        public void Warn(string msg)
        {
            WriteLine("WARNING!!" + msg);
        }
    }

    // Simply implement an object that conforms to the interface and they do nothing
    // With properties it can get complicated
    public class NullLog : ILog
    {
        public void Info(string msg)
        {
            // DO NOTHING
        }

        public void Warn(string msg)
        {
        }
    }

    // ----------------------------- PART 2 Dynamic NullObject
    // Example above just a simple Interface ILog with 2 methods, but there are alternatives DLR paying the price of dynamic but we will preserve ILog interface
    // We need an object that acts like ILog but does nothing
    public class Null<TInterface> : DynamicObject where TInterface : class
    {
        // returns of an instance of this type
        public static TInterface Instace => new Null<TInterface>().ActLike<TInterface>();

        // TLR hackery that allows us to make dynamic object that doesn't do anything but it exposes itself not as a dynamic object but the object of the correct interface.
        public override bool TryInvokeMember(InvokeMemberBinder binder, object[] args, out object result)
        {
            // out object result, idea is someone calls the method we want to return what this method is returning
            // if is an int return default constructor of int
            result = Activator.CreateInstance(binder.ReturnType);
            // this can be problematic if the type we are returning doesn't have a default constructor
            return true;
        }
    }

}
