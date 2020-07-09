namespace PROXY
{
    public class Creature
    {
        // public Property<int> Agility { get; set; }
        // setter won't be executed 

        private Property<int> _agility = new Property<int>();
        public int Agility
        {
            get => _agility;
            set => _agility = value;
        }
    }
}