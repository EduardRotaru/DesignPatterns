using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Text;
using System.Threading.Tasks;
using static System.Console;

namespace MEDIATOR
{
    // EventBroker is a component which just in case like a chatroom it allows the exchange of messages between different components without them being aware that they are present in the system.
    // Is gonna glue all the hierarchy
    public class EventBroker : IObservable<PlayerEvent>
    {
        // is a cheat from reactive extensions infrastructure so I don't have to implement this myself
        private Subject<PlayerEvent> subscriptions = new Subject<PlayerEvent>();

        public IDisposable Subscribe(IObserver<PlayerEvent> observer)
        {
            return subscriptions.Subscribe(observer);
        }

        public void Publish(PlayerEvent pe)
        {
            subscriptions.OnNext(pe);
        }
    }

    public class Actor
    {
        protected EventBroker broker;

        public Actor(EventBroker broker)
        {
            this.broker = broker;
        }
    }

    public class FootballPlayer : Actor
    {
        public string Name { get; set; }
        public int GoalsScored { get; set; }

        public void Score()
        {
            GoalsScored++;
            broker.Publish(new PlayerScoredEvent {Name = Name, GoalsScored = GoalsScored});
        }

        public void AssaultReferree()
        {
            broker.Publish(new PlayerSentOfFEvent {Name = Name, Reason = "violence"});
        }

        public FootballPlayer(EventBroker broker, string name) : base(broker)
        {
            Name = name;
            broker.OfType<PlayerScoredEvent>()
                .Where(ps => !ps.Name.Equals(Name))
                .Subscribe(
                    ps => WriteLine($"{Name}: Nicely done, {ps.Name}! Its your{ps.GoalsScored} goal.")
                );

            broker.OfType<PlayerSentOfFEvent>()
                .Where(ps => !ps.Name.Equals(Name))
                .Subscribe(ps => WriteLine($"{Name}: see you in the lockers, {ps.Name}"));
        }
    }

    public class FootballCoach : Actor
    {
        public FootballCoach(EventBroker broker) : base(broker)
        {
            broker.OfType<PlayerScoredEvent>()
                .Subscribe(pe =>
                {
                    if (pe.GoalsScored < 3)
                        WriteLine($"Coach: well done , {pe.Name}!");
                });
            broker.OfType<PlayerSentOfFEvent>()
                .Subscribe(pe =>
                {
                    if(pe.Reason == "Violence")
                        WriteLine($"Coach: how could you , {pe.Name}!");

                });
        }
    }

    public class PlayerEvent
    {
        public string Name { get; set; }
    }

    public class PlayerScoredEvent : PlayerEvent
    {
        public int GoalsScored { get; set; }
    }

    public class PlayerSentOfFEvent : PlayerEvent
    {
        public string Reason { get; set; }
    }
}

