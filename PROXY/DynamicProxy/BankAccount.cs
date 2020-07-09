using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Text;
using ImpromptuInterface;
using static System.Console;

namespace PROXY.DynamicProxy
{
    // Proxy options are static or dynamic, static are the fastest
    // Dynamic are constructed at runtime with the associated perfomance costs
    public class BankAccount : IBankAccount
    {
        private int balance;
        private int overdraftLimit = -500;

        public void Deposit(int amount)
        {
            balance += amount;
            Console.WriteLine($"Deposited ${amount}, balance is now {balance}");
        }

        public bool Withdraw(int amount)
        {
            if (balance - amount >= overdraftLimit)
            {
                balance -= amount;
                Console.WriteLine($"Withdrew ${amount}, balance is now {balance}");
                return true;
            }
            return false;
        }

        public override string ToString()
        {
            return $"{nameof(balance)}: {balance}";
        }
    }

    // Goal here is to implemented logging, note the number of times methods are called
    // We don't want to reimplemented all the methods for a static proxy

    // impromptuInterface lirbrary generating appropiate interfaces.

    // Complicated stuff ahead
    public class Log<T> : DynamicObject
        where T : class, new()
    {
        private readonly T _subject;
        // name method, count
        private Dictionary<string, int> methodCallCount = new Dictionary<string, int>();

        public Log(T subject)
        {
            _subject = subject;
        }

        // factory method to force our dynamic object as if implements the interface IBankAccount
        // to implement logging since we are in a dynamic object we can do the following

        // generated method
        // we use this when we want invoke something on the object
        public override bool TryInvokeMember(InvokeMemberBinder binder, object[] args, out object result)
        {
            // invoke member and log the call
            try
            {
                //diagnostic information
                WriteLine($"Invoking {_subject.GetType().Name}.{binder.Name} with arguments [{string.Join(",", args)}]");

                // logging by adding method call count
                if (methodCallCount.ContainsKey(binder.Name)) methodCallCount[binder.Name]++;
                else methodCallCount.Add(binder.Name, 1);

                result = _subject.GetType().GetMethod(binder.Name)?.Invoke(_subject, args);
                return true;
            }
            catch
            {
                // if out result fails
                result = null;
                return false;
            }
        }

        // we need to make Log<T> to give us an interface
        // generic factory method
        public static I As<I>() where I : class
        {
            if (!typeof(I).IsInterface)
                throw new ArgumentNullException("I must be an interface");
            return new Log<T>(new T()).ActLike<I>();
        }

        public string Info
        { 
            get
            {
                var sb = new StringBuilder();
                foreach (var kv in methodCallCount)
                {
                    sb.AppendLine($"{kv.Key} called {kv.Value} time(s)");
                }

                return sb.ToString();
            }
        }

        public override string ToString()
        {
            return $"{Info}{_subject}";
        }
    }
}