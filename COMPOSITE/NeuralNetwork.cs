using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace COMPOSITE
{
    public static class ExtensionMethods
    {
        // now neurons have in common ienumerable

        public static void ConnectTo(this IEnumerable<Neuron> self,
            IEnumerable<Neuron> other)
        {
            // if they are equal we return
            if (ReferenceEquals(self, other)) return;

            // We iterate on both sides of neurons and connect them together
            foreach(var from in self)
                foreach (var to in other)
                {
                    from.Out.Add(to);
                    to.In.Add(from);
                }
        }
    }

    public static class NeuralNetwork
    {
        public static void Demo()
        {
            var neuron1 = new Neuron();
            var neuron2 = new Neuron();

            neuron1.ConnectTo(neuron2); // 1 method

            var layer1 = new NeuronLayer();
            var layer2 = new NeuronLayer();

            // 4 methods to connect every other kind of object to each other
            // Requirements now are, a neuron can connect to a layer, a layer to a neuron, a layer to a layer

            // we now can connect them each other
            neuron1.ConnectTo(layer1);
            layer1.ConnectTo(layer2);
        }
    }
    
    // Neuon is a ienumerable of neuron
    public class Neuron : IEnumerable<Neuron>
    {
        public float Value;
        // Two sets of connections, going In and going Out
        public List<Neuron> In, Out;

        // connect single neuron to other
        public void ConnectTo(Neuron other)
        {
            Out.Add(other);
            other.In.Add(this);
        }

        public IEnumerator<Neuron> GetEnumerator()
        {
            // typically here we return each of the containing elements
            // neuron is a scalar value and we want to represent it as a enumerable
            yield return this;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
    
    // A collection of neurons 
    public class NeuronLayer : Collection<Neuron> { }

    // We need even more methods for connections
    // WE need to implement the composite pattern avoid the cartesian product
    public class NeuronRing : List<Neuron> { }

    // To solve this we need an extension method
    // Extension method on what? What does it have in common between a neuron that is just a standalone class and a neuronLayer that is already occupied and cannot give them a base class
    // We treat both classes as a collection of neurons except that Neuron is gonna be a collection with a single element


}
