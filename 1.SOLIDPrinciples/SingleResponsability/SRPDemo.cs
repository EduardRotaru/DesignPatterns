using System;
using System.Collections.Generic;
using System.IO;

namespace _1.SOLIDPrinciples.SingleResponsability
{
    public class Journal
    {
        private readonly List<string> entries = new List<string>();

        private static int count = 0;

        public int AddEntry(string text)
        {
            entries.Add($" {++count}: {text}");
            return count; //memento
        }

        public void RemoveEntry(int index)
        {
            entries.RemoveAt(index);
        }

        public override string ToString()
        {
            return string.Join(Environment.NewLine, entries);
        }

        public void Save(string filename)
        {
            File.WriteAllText(filename, ToString());
        }

        public static Journal Load(string filename)
        {
            return new Journal();
        }

        public void Load(Uri uri) { }

        // Adding too much responsability to the Journal class, not only keeping the entries but managing them
        // So we move management functions to another class to keep single responsability for a class.
    }

    public class Persistance
    {
        public void SaveToFile(Journal j, string filename, bool overwrite = false)
        {
            if(overwrite || !File.Exists(filename))
                File.WriteAllText(filename, j.ToString());
        }
    }
}
