using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Eventing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Console;

namespace ITERATOR
{
    // how to setup an interator
    public class Node<T>
    {
        public T Value;
        public Node<T> Left, Right;
        public Node<T> Parent;

        public Node(T value)
        {
            Value = value;
        }

        public Node(T value, Node<T> left, Node<T> right)
        {
            Value = value;
            Left = left;
            Right = right;

            left.Parent = right.Parent = this;
        }
    }

    public class BinaryTree<T>
    {
        private Node<T> root;

        public BinaryTree(Node<T> root)
        {
            this.root = root;
        }

        // We automatically get a sequence by having an IEnumerable
        // Is readable and recursive, we recursivle doing the Left part and yielding the current element then the Right part

        public IEnumerable<Node<T>> InOrder
        {
            // problem is that the state machine that reads the item has to reference
            // the current item being returned
            // There is no argument of the current state to do it in a recursive way
            get
            {
                // in C# 7 we can embbeded a method inside a method
                // we implement a method which implement a traversal to keep that state in that argument
                IEnumerable<Node<T>> Traverse(Node<T> current)
                {
                    if (current.Left != null)
                    {
                        foreach (var left in Traverse(current.Left))
                            yield return left;
                    }

                    yield return current;
                    if (current.Right != null)
                    {
                        foreach (var right in Traverse(current.Right))
                            yield return right;
                    }
                }

                // we have to do this folded yield returns because there is no way to yield return an IEnumerable by expanding the whole IEnumerable
                foreach (var node in Traverse(root))
                    yield return node;
            }
        }

        // The BinaryTree is not IEnumerable, it doesn't inherit IEnumerable<Node<T>>
        // this works because of duck typing, foreach doesn't care if the type is IEnumerable
        public InOrderIterator<T> GetEnumertor()
        {
            return new InOrderIterator<T>(root);
        }
    }

    // Part 2: We built in the first part a state machine where the current reference is the state
    // and moving to each to another state using MoveNext()
    // in C# state machines are built automatically when using IEnumerable and yield and using yield return and yield break I can control the flow of what this enumeration is presenting
    public class InOrderIterator<T> // part two making this an ordinary method
    {
        public Node<T> Current { get; set; } // indicates the current node which the iterator node points

        // changed from a field to an automatic property, we can use now this as an iterator in a foreach statement
        private readonly Node<T> root;
        private bool yieldedStart; // wether we yielded start value

        public InOrderIterator(Node<T> root)
        {
            this.root = Current = root;
            while (Current.Left != null)
                Current = Current.Left; // starting point

            //  1 <- root
            // /\
            //2  3
            //^ Current
        }

        // attempts to move you to the next element being iterated and returns a boolean if the operation succeeded
        // when last element will return false, if is not will move to the next element in-order
        public bool MoveNext()
        {
            if (!yieldedStart)
            {
                yieldedStart = true; // starting already 
                return true; // indicates if the current element pointed is valid
            }

            // We are not using IEnumerable and yield, we check right part and navigate all the way to the left
            if (Current.Right != null)
            {
                Current = Current.Right;
                while (Current.Left != null)
                {
                    Current = Current.Left;
                }

                return true; // we navigate to the most left node
            }
            else
            {
                // we navigate up the tree
                var p = Current.Parent;
                while (p != null && Current == p.Right)
                {
                    Current = p; // current to the parent
                    p = p.Parent; // walk up the parent stack
                }

                Current = p;
                return Current != null; // In order traversal 
            }
        }

        // resets iterator to the starting position
        public void Reset()
        {
            Current = root;
            yieldedStart = true;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            //  1 
            //  /\
            // 2  3

            // in-order traversal: 213
            // preorder: 123

            // This is the tree we will traverse in-order with iterator pattern
            var root = new Node<int>(1, new Node<int>(2), new Node<int>(3));
            var it = new InOrderIterator<int>(root);
            while (it.MoveNext())
            {
                Write(it.Current.Value);
                Write(',');
            }

            WriteLine();

            var tree = new BinaryTree<int>(root);
            WriteLine(string.Join(",", tree.InOrder.Select(x => x.Value)));

            var binaryTree = new BinaryTree<int>(root);
            foreach (var node in binaryTree) // binary tree is not enumerable,
                                             // we need an GetEnumerator method that returns an Iterator
                                             // requirements for this is it needs a MoveNExt and returning a current element
            {
                WriteLine(node.value);
            }
        }
    }

    // Iterator in C# works under the hood
    // like a foreach statement, move next, yield and so on
    // In order traversal shouldnt be like this but recursive

    // Part 2 implementing recursive and have not having a separate iterator object 
    // C# will build a state machine for this separate iterator

    // In binary tree what we have done was very flexible
    // we have had a method or a property that returned an in order traversal

    // Part 3
    // Array backed properties
    public class Creature : IEnumerable<int>
    {
        private int[] stats = new int[3];
        private const int strength = 0;

        public int Strength
        {
            // implementation of array backing property
            // the reason behind this is the getter is more stable in a sense if I add 
            // additional status I don't have to rewrite the getter again
            // because this getter devolves to a simple call "stats.Average"
            get { return stats[strength]; }
            set { stats[strength] = value; }
        }

        public int Agility { get; set; }
        public int Intelligence { get; set; }

        // This works with a single getter and don't add more statistics
        // other way we need to iterate classes properties
        // simple and elegant solution, all the properties have an automatic backing field
        // is up to the programmer what actual backing fields are
        public double AverageState
        {
            get { return (Strength + Agility + Intelligence) / 3.0; }
        }

        // because we having a backing store which is an array we get the benefits of linq
        // everything can be called IEnumerable of int is available on your stats array
        // Min Max Sum can be done automatically, linq has implemented those aggregate operators 
        public double AverageState2 => stats.Average();

        // Is an iteration of different properties 
        public IEnumerator<int> GetEnumerator()
        {
            return stats.AsEnumerable().GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        // Observable collection, having stats collection exposed and thereby can monitor the changes on values,
        // we react to them, in typical reactive extensions.
        // choose a backing field  for several properties to be a single array and the benefits are huge
        public int this[int index]
        {
            get { return stats[index]; }
            set { stats[index] = value; }
        }
    }
}
