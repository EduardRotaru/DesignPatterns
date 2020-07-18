using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Channels;
using static System.Console;

namespace COMMAND
{
    class Program
    {
        static void Main(string[] args)
        {
            var ba= new BankAccount();
            var commands = new List<BankAccountCommand>
            {
                new BankAccountCommand(ba, BankAccountCommand.Action.Deposit, 100),
                //new BankAccountCommand(ba, BankAccountCommand.Action.Withdraw, 50),

                // This will break above because we assumed deposit is reverse of withdrew 
                // we reverted to 100 because we assumed the deposit is the opposite of withdraw.
                new BankAccountCommand(ba, BankAccountCommand.Action.Withdraw, 1000),
            };
            
            WriteLine(ba);

            foreach (var c in commands)
            {
                c.Call();
            }

            WriteLine(ba);

            // why not doing command reverse. Reverse is a part of a List<T> and its reversing a list in place
            // is a mutating operation the only way to call linq operation using an extension method .Reverse
            foreach (var c in Enumerable.Reverse(commands))
            {
                c.Undo();
            }
        }
    }

    public class BankAccount
    {
        private int balance;
        private int overdraftLimit = -500; 

        public void Deposit(int amount)
        {
            balance += amount;
            WriteLine($"Deposited ${amount}, balance is now {balance}");
        }
 
        public void Withdraw(int amount)
        {
            if (balance - amount >= overdraftLimit)
            {
                balance -= amount;
                WriteLine($"Withdrew ${amount}, balance is now {balance}");
            }
        }

        // To fix it we need to know if succeeded or not
        public bool WithdrawSuceeeded(int amount)
        {
            if (balance - amount >= overdraftLimit)
            {
                balance -= amount;
                WriteLine($"Withdrew ${amount}, balance is now {balance}");
                return true;
            }

            return false;
        }

        public override string ToString()
        {
            return $"{nameof(balance)}: {balance}, {nameof(overdraftLimit)}: {overdraftLimit}";
        }
    }

    //  Instead of sending information requesting imparatevely by calling methods we actually put in commands
    // Information isn't lost, we still have a trail of commands
    //
    public interface ICommand
    {
        void Call();
        void Undo();
    }

    public class BankAccountCommand : ICommand
    {
        private BankAccount account;

        public enum Action
        {
            Deposit,
            Withdraw
        }

        private Action action;
        private int amount;
        private bool succeeded;

        public BankAccountCommand(BankAccount account, Action action, int amount)
        {
            this.account = account;
            this.action = action;
            this.amount = amount;
        }

        public void Call()
        {
            switch (action)
            {
                case Action.Deposit:
                    account.Deposit(amount);
                    succeeded = true;
                    break;
                case Action.Withdraw:
                    succeeded = account.WithdrawSuceeeded(amount);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public void Undo()
        {
            if (!succeeded) return;
            switch (action)
            {
                case Action.Deposit:
                    account.Withdraw(amount);
                    break;
                case Action.Withdraw:
                    account.Deposit(amount);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }

    // Part two 
    // We can have an UNDO operation for the command or sequence of commands
}
