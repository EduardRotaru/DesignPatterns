using System.Collections.Generic;
using static  System.Console;

namespace MEMENTO
{
    public class Memento
    {
        public int Balance { get; }

        public Memento(int balance)
        {
            Balance = balance; // cannot be changed because the setter is private
        }
    }

    public class BankAccount
    {
        private int _balance;
        private List<Memento> changes = new List<Memento>();
        private int current; // which memento we are curently on

        public BankAccount(int balance)
        {
            this._balance = balance;
            changes.Add(new Memento(balance));
        }

        // We are gonna have Withdraw and Deposit methods
        // instead of making Deposit method void we gonna return a Memento
        public Memento Deposit(int amount)
        {
            _balance += amount;
            var m =  new Memento(_balance);
            changes.Add(m);
            ++current;

            return m;
        }

        public Memento Restore(Memento m)
        {
            if (m != null) // if there is nothing to undo, memento will return null
            {
                _balance = m.Balance;
                changes.Add(m);
                return m;
            }

            return null;
        }

        public Memento Undo()
        {
            if (current > 0)
            {
                var m = changes[--current];
                _balance = m.Balance;
                return m;
            }

            return null;
        }

        public Memento Redo()
        {
            if (current + 1 < changes.Count)
            {
                var m = changes[++current];
                _balance = m.Balance;
                return m;
            }

            return null;
        }

        public override string ToString()
        {
            return $"{nameof(_balance)}: {_balance}";
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
           var ba = new BankAccount(100);
           ba.Deposit(50);
           ba.Deposit(25);
           WriteLine(ba);

           ba.Undo();
           WriteLine($"Undo 1: {ba}");
           ba.Undo();
           WriteLine($"Undo 2: {ba}");
           ba.Redo();
           WriteLine($"Redo: {ba}");
        }

        static void MementoImplementationWithoutRedoAndUndo()
        {
            var ba = new BankAccount(100);
            var m1 = ba.Deposit(50); // 150
            var m2 = ba.Deposit(25); // 175
            WriteLine(ba);

            ba.Restore(m1);
            WriteLine(ba);

            ba.Restore(m2);
            WriteLine(ba);
        }
    }

    // 
}
