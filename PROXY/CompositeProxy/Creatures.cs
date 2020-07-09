using System.Collections.Generic;

namespace PROXY
{
    // we want something enumerable that stores data about creature in a specific way 
    public class Creatures
    {
        private readonly int _size;
        private byte[] age;
        private int[] x, y;

        // Allocated the data, nicely aligned 
        public Creatures(int size)
        {
            this._size = size;
            age = new byte[size];
            x = new int[size];
            x = new int[size];
        }

        // using a struct because we store index of the creature to access and a reference to the overall set of the creatures
        public struct CreatureProxy
        {
            private readonly Creatures _creatures;
            private readonly int _index;

            // reference to all creatures
            // reference to the index of the creature
            public CreatureProxy(Creatures creatures, int index)
            {
                _creatures = creatures;
                _index = index;
            }

            // give out a reference that refer to the age or coordinates
            // whenever they access this proxy they can start working with Age, X, Y
            // all these really are  references into the arrays that we have defined, they are not real arrays just a reference to the arrays in the class
            // this struct is just a placeholder, something that allows to you access the particular field, all that is changeable in this struct is index
            public ref byte Age => ref _creatures.age[_index];
            public ref int X => ref _creatures.x[_index];
            public ref int Y => ref _creatures.y[_index];
        }

        public IEnumerator<CreatureProxy> GetEnumerator()
        {
            for (int pos = 0; pos < _size; ++pos)
            {
                yield return new CreatureProxy(this, pos);
            }
        }
    }
}