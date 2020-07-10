using System;
using System.ComponentModel.Design.Serialization;
using System.Configuration;
using System.Security.Cryptography.X509Certificates;
using static System.Console;

namespace ChainOfResponsability
{
    // Scneario, computer game with a bunch of creatures with modifiers and we can improve-boosts these creatures
    public class Creature
    {
        public string Name;
        public int Attack, Defense;

        public Creature(string name, int attack, int defense)
        {
            Name = name;
            Attack = attack;
            Defense = defense;
        }

        public override string ToString()
        {
            return $"{nameof(Name)}: {Name}, {nameof(Attack)}: {Attack}, {nameof(Defense)}: {Defense}";
        }
    }


    // This class facilitates chain of responsability design pattern,
    // we are gonna have a reference to the creature 
    public class CreatureModifier
    {
        protected Creature creature;
        // Chain of modifiers building a linked list
        protected CreatureModifier next; // linked list

        public CreatureModifier(Creature creature)
        {
            this.creature = creature;
        }

        // add a new modifier
        // why we would call Add instead of constructor
        public void Add(CreatureModifier cm)
        {
            if (next != null) next.Add(cm);
            else next = cm;
        }

        // invoked when we want to apply the modifier to the creature
        // so in this case we call next.handle and we have a modifier in a chain of modifiers and we tell them to apply
        public virtual void Handle() => next?.Handle();
    }

    public class DoubleAttackModifier : CreatureModifier
    {
        public DoubleAttackModifier(Creature creature)
            : base(creature)
        {
        }

        public override void Handle()
        {
            WriteLine($"Doubling {creature.Name}*s attack");
            creature.Attack *= 2;
            // reason why we call base class handle is implementation that is walking the implementation of linked list
            // if there is a modifier after this one we want to call that one too 
            base.Handle();
        }
    }

    public class IncreaseDefenseModifier : CreatureModifier
    {
        public IncreaseDefenseModifier(Creature creature) 
            : base(creature)
        {
        }

        public override void Handle()
        {
            WriteLine($"Increasing {creature.Name}'s defense");
            creature.Defense += 3;
            base.Handle();
        }
    }

    public class NoBonusesModifiers : CreatureModifier
    {
        public NoBonusesModifiers(Creature creature) 
            : base(creature)
        {
        }

        public override void Handle()
        {
            // doing nothing here stops the traversing chains of responsability(linked list)
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            //FirstApproach();
            EventBrokeApproach_ChainOfResponsabilityPattern();
        }

        private static void EventBrokeApproach_ChainOfResponsabilityPattern()
        {
            var game = new Game();
            var goblin = new Creature2(game, "Strong Goblin", 3, 3);
            WriteLine(goblin);

            using (new DoubleAttackModifier2(game, goblin))
            {
                WriteLine(goblin);
                using (new IncreaseDefenseModifier2(game, goblin))
                {
                    WriteLine(goblin);
                }
            }

            WriteLine(goblin);
        }

        private static void FirstApproach()
        {
            var goblin = new Creature("Goblin", 2, 2);
            WriteLine(goblin);

            // we can treat the creatureModifier as a root object for any kind of modifier we want to add on top of a creature
            var root = new CreatureModifier(goblin);

            // ex goblin cannot get buffs
            root.Add(new NoBonusesModifiers(goblin));

            WriteLine("Lets double the goblin attack");
            root.Add(new DoubleAttackModifier(goblin));

            WriteLine("Lets double the goblin defense");
            root.Add(new IncreaseDefenseModifier(goblin));

            root.Handle(); // takes care of all the modifier, debug to see how we walk the chain

            WriteLine(goblin);
        }
    }

    // part 2
    // why isn't good its because its permamentely modifes the creature and can't remove the modifiers without reimplementing the creature 
    // fields of creatures are exposed 

    // (acts like a) Mediator design pattern 
    public class Game
    {
        // the game is gonna provide a query API to provide an attacker defense API for a particular creature
        public event EventHandler<Query> Queries;

        public void PerformQuery(object sender, Query q)
        {
            Queries?.Invoke(sender, q);
        }
    }

    public class Query
    {
        public string CreatureName;

        public enum Argument
        {
            Attack, Defense
        }

        public Argument WhatToQuery; // what kind of info we want from a creature
        public int value; // store value from query

        public Query(string creatureName, Argument whatToQuery, int value)
        {
            CreatureName = creatureName;
            WhatToQuery = whatToQuery;

            // if we have a bonus on top of initial value, and then event get invoked and somebody has to adds an additional value 
            this.value = value; 
        }
    }

    public class Creature2
    {
        private Game game;
        public string Name;
        private int attack, defense;

        public Creature2(Game game, string name, int attack, int defense)
        {
            this.game = game;
            Name = name;
            this.attack = attack;
            this.defense = defense;
        }
        public int Attack
        {
            get
            {
                var q = new Query(Name, Query.Argument.Attack, attack);
                game.PerformQuery(this, q); // q.Value
                return q.value;
            }
        }

        public int Defense
        {
            get
            {
                var q = new Query(Name, Query.Argument.Defense, defense);
                game.PerformQuery(this, q); // q.Value
                return q.value;
            }
        }

        public override string ToString()
        {
            return $"{nameof(Name)}: {Name}, {nameof(Attack)}: {Attack}, {nameof(Defense)}: {Defense}";
        }
    }

    public abstract class CreatureModifier2 : IDisposable
    {
        protected Game game;
        protected Creature2 creature;

        protected CreatureModifier2(Game game, Creature2 creature)
        {
            this.game = game;
            this.creature = creature;

            game.Queries += Handle;
        }

        protected abstract void Handle(object sender, Query q);

        public void Dispose()
        {
            game.Queries -= Handle;
        }
    }

    public class DoubleAttackModifier2 : CreatureModifier2
    {
        public DoubleAttackModifier2(Game game, Creature2 creature) 
            : base(game, creature)
        {
        }

        protected override void Handle(object sender, Query q)
        {
            if (q.CreatureName == creature.Name
                && q.WhatToQuery == Query.Argument.Attack) q.value *= 2;
        }
    }

    public class IncreaseDefenseModifier2 : CreatureModifier2
    {
        public IncreaseDefenseModifier2(Game game, Creature2 creature) 
            : base(game, creature)
        {
        }

        protected override void Handle(object sender, Query q)
        {
            if (q.CreatureName == creature.Name
                && q.WhatToQuery == Query.Argument.Defense) q.value *= 3;
        }
    }

}
