using System;
using System.Windows;
using  static System.Console;

namespace OBSERVER
{
    // c# being a mananged languages doesn't have memory leaks but the actual fact we
    // can get something similar to a memory leak using events and observer design pattern

    // weak event design pattern when I make visual controls in wpf 

    public class WeakEventManager
    {
        public static void Demo()
        {
            var btn = new Button();
            var window = new Window(btn);
            var windowRef = new WeakReference(window);
            btn.Fire();

            WriteLine("Setting window to null");
            window = null; // we should have get garbage collector fired here 
            // it doesn't happen because its subscribed to an event and the button isn't dead

            FireGC();
            WriteLine($"Is the window alive after GC? {windowRef.IsAlive}");
        }

        // we still have window that is not finalized because is subscribed to an event 
        // object is being kept and cannot be garbage collected simply because it subscribes to an event
        // on an object that is still alive
        private static void FireGC()
        {
            WriteLine("Starting GC");
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();
            WriteLine("GC is done");
        }
    }

    public class Button
    {
        public event EventHandler Clicked;

        public void Fire()
        {
            Clicked?.Invoke(this, EventArgs.Empty);
        }
    }

    public class Window
    {
        public Window(Button button)
        {
            button.Clicked += ButtonOnClicked; // subscribe to the events
        }

        private void ButtonOnClicked(object sender, EventArgs eventArgs)
        {
            WriteLine("Button clicked (Window handle)");
        }

        // it won't get null because we have subscribed to an event and the button is not dead
        // 

        // Destructor for when window is finalized
        ~Window()
        {
            WriteLine("Window finalized");
        }
    }

    public class Window2
    {
        public Window2(Button button)
        {
            WeakEventManager<Button, EventArgs>
                .AddHandler(button, "Clicked", ButtonOnClicked);
        }

        private void ButtonOnClicked(object sender, EventArgs eventArgs)
        {
            WriteLine("Button clicked (Window2 handler)");
        }

        ~Window2()
        {
            WriteLine("Window2 finalized");
        }
    }
}
