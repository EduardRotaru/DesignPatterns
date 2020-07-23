using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Remoting.Channels;
using System.Text;
using System.Threading.Tasks;
using OBSERVER.Annotations;

namespace OBSERVER
{
    // events are simple for observers
    public class RxSamples
    {
        public static void Demo()
        {
            var market = new Market();
            market.PropertyChanged += (sender, eventArgs) =>
            {
                // tracking changes in a single field/property
                if (eventArgs.PropertyName == "Volatility")
                {

                }
            };
        }

        public static void Demo2() // observer
        {
            var market = new Market2();
            //market.PriceAdded += (sender, f) =>
            //{
            //    Console.WriteLine($"we got a price of {f}");
            //};
            market.prices2.ListChanged += (sender, eventArgs) =>
            {
                if (eventArgs.ListChangedType == ListChangedType.ItemAdded)
                {
                    float price = ((BindingList<float>) sender)[eventArgs.NewIndex];
                    Console.WriteLine($"Binding list got a price of {price}");
                }
            };

            market.AddPrice(123);
        }
    }

    public class Market : INotifyPropertyChanged
    {
        private float _volatility;

        //Observer design pattern just for one property
        public float Volatility
        {
            get => _volatility;
            set
            {
                if (value.Equals(_volatility)) return;
                _volatility = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class Market2 // obsercable
    {
        private List<float> prices = new List<float>();
        public BindingList<float> prices2 = new BindingList<float>();

        public void AddPrice(float price)
        {
            prices.Add(price);
            //PriceAdded?.Invoke(this,price);
        }

        // no longer needed
        //public event EventHandler<float> PriceAdded;
    }
}
