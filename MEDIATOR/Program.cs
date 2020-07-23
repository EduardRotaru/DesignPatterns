using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using static System.Console;

namespace MEDIATOR
{
    class Program
    {
        static void Main(string[] args)
        {
            EventBrokerImplementation();
        }

        static void ChatRoomImplementation()
        {
            var room = new ChatRoom();
            var john = new Person("John");
            var jane = new Person("Jane");

            room.Join(john);
            room.Join(jane);

            john.Say("hi");
            jane.Say("oh hey john");

            var simon = new Person("Simon");
            room.Join(simon);

            simon.Say("Hi everyone");
            jane.PrivateMessage("Simon", "glad you can join us!");
        }

        // the way of setting up an eventBroker/EventBus using DI container and reactive extensions
        static void EventBrokerImplementation()
        {
            var cb = new ContainerBuilder();
            cb.RegisterType<EventBroker>().SingleInstance();
            cb.RegisterType<FootballCoach>();
            cb.Register((c, p) =>
                new FootballPlayer(
                    c.Resolve<EventBroker>(),
                    p.Named<string>("name")
                    ));

            using (var c = cb.Build())
            {
                var coach = c.Resolve<FootballCoach>();
                var player1 = c.Resolve<FootballPlayer>(new NamedParameter("name", "John"));
                var player2 = c.Resolve<FootballPlayer>(new NamedParameter("name", "Chris"));

                player1.Score();
                player1.Score();
                player1.Score(); // should be ignored
                player1.AssaultReferree();
                player2.Score();
            }
        }
    }

    public class Person
    {
        public string Name;
        public ChatRoom Room;
        private List<string> ChatLog = new List<string>();

        public Person(string name)
        {
            Name = name;
        }

        public void Say(string message)
        {
            Room.Broadcast(Name, message);
        }

        public void PrivateMessage(string who, string message)
        {
            Room.Message(Name, who, message);
        }

        public void Receive(string sender, string message)
        {
            string s = $"{sender}: '{message}'";
            ChatLog.Add(s);
            WriteLine($"[{Name}'s chat session] {s}");
        }
    }

    public class ChatRoom
    {
        private List<Person> people = new List<Person>();

        public void Join(Person p)
        {
            string joinMsg = $"{p.Name} joins the chat room";
            Broadcast("room", joinMsg);

            p.Room = this;
            people.Add(p);
        }

        // we go to everyone and tell this person joined
        public void Broadcast(string source, string message)
        {
            foreach (var p in people)
            {
             if(p.Name != source) 
                 p.Receive(source, message);
            }
        }

        // chatroom is a cental component which acts as a mediator which allows people to communicate with each other without knowing their presence
        public void Message(string source, string destination, string message)
        {
            // Participants are not aware of each other and if the name is not found the message doesn't go anywhere
            people.FirstOrDefault(p => p.Name == destination)?.Receive(source, message); // ?. if name not found 
        }
    }
}
