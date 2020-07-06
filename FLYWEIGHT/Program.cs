using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JetBrains.dotMemoryUnit;
using static System.Console;
using NUnit.Framework;

namespace FLYWEIGHT
{
    // We are gonna work with strings and if we are working with indentical strings,
    // they will refer to the same location in the memory and since strings are immutable

    public class User
    {
        private string fullName;

        public User(string fullName)
        {
            this.fullName = fullName;
        }
    }

    public class User2
    {
        static List<string> strings = new List<string>();
        private int[] names;

        public User2(string fullName)
        {
            // keep all the names in this list
            int GetOrAdd(string s)
            {
                int idx = strings.IndexOf(s);
                if (idx != -1) return idx;
                else
                {
                    strings.Add(s);
                    return strings.Count - 1;
                }
            }

            names = fullName.Split(' ').Select(GetOrAdd).ToArray();
        }

        public string FullName => string.Join(" ", names.Select(i => strings[i]));
    }

    [TestFixture]
    public class Demo
    {
        [Test]
        public void TestUser() // 2049147 This is the size in bytes all the memory this test occupied 
        {
            var firstNmes = Enumerable.Range(0, 100)
                .Select(_ => RandomString());

            var lastNames = Enumerable.Range(0, 100)
                .Select(_ => RandomString());

            var users = new List<User>();

            foreach (var firstName in firstNmes)
            {
                foreach (var lastName in lastNames)
                {
                    users.Add(new User($"{firstName} {lastName}"));
                }
            }

            ForceGC();

            dotMemory.Check(memory =>
            {
                WriteLine(memory.SizeInBytes);
            });
        }

        [Test]
        public void TestUser2() // 1691197 Second approach saved difference between first and second test
        {
            var firstNmes = Enumerable.Range(0, 100)
                .Select(_ => RandomString());

            var lastNames = Enumerable.Range(0, 100)
                .Select(_ => RandomString());

            var users = new List<User2>();

            foreach (var firstName in firstNmes)
            {
                foreach (var lastName in lastNames)
                {
                    users.Add(new User2($"{firstName} {lastName}"));
                }
            }

            ForceGC();

            dotMemory.Check(memory =>
            {
                WriteLine(memory.SizeInBytes);
            });
        }

        private void ForceGC()
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();
        }

        private string RandomString()
        {
            var rand = new Random();

            return new string(Enumerable.Range(0, 10)
                .Select(i => (char)('a' + rand.Next(26)))
                .ToArray());
        }
    }

    // Text formatting example
    // Edit formatting text can be done with memory saving technique
    public class FormattedText
    {
        private readonly string plainText;

        // Problem with this approach we are spending a lot of memory for flags
        // Might want to specify a bunch of ranges that need to be capitalized, indication of starting and ending value
        private bool[] capitalize;

        public FormattedText(string plainText)
        {
            this.plainText = plainText;
            capitalize = new bool[plainText.Length];
        }

        public void Capitalize(int start, int end)
        {
            for (int i = start; i < end; i++)
            {
                capitalize[i] = true;
            }
        }

        public override string ToString()
        {
            var sb = new StringBuilder();

            for (int i = 0; i < plainText.Length; i++)
            {
                var c = plainText[i];
                sb.Append(capitalize[i] ? char.ToUpper(c) : c);
            }

            return sb.ToString();
        }
    }

    public class BetterFormattedText
    {
        private string PlainText;
        private List<TextRange> formatting = new List<TextRange>();

        public BetterFormattedText(string plainText)
        {
            this.PlainText = plainText;
        }

        public TextRange GetRange(int start, int end)
        {
            var range = new TextRange { Start = start, End = end };
            formatting.Add(range);

            return range;
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            for (int i = 0; i < PlainText.Length; i++)
            {
                var c = PlainText[i];
                foreach (var range in formatting)
                {
                    if (range.Covers(i) && range.Capitalize)
                    {
                        c = char.ToUpper(c);
                    }
                }

                sb.Append(c);
            }

            return sb.ToString();
        }

        // Flyweight implemented here by operating on these text ranges and not the huge arrays of formatting flags for individual characters
        public class TextRange
        {
            public int Start, End;
            public bool Capitalize, Bold, Italic;

            public bool Covers(int position)
            {
                return position >= Start && position <= End;
            }
        }
    }

    public class Demo2
    {
        public void FormattedExample()
        {
            var ft = new FormattedText("This is a brave new world");
            ft.Capitalize(10, 15);
            WriteLine(ft);
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            var d = new Demo2();
            d.FormattedExample();

            var bft = new BetterFormattedText("This is a brave new world");
            bft.GetRange(10, 15).Capitalize = true;
        }
    }
}
