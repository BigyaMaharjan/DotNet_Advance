using System;
using EventHandle.Events;
using EventHandle.Listeners;

namespace EventHandle
{
    class Program
    {
        static void Main(string[] args)
        {
            // Create instances of Button and listeners
            Button button = new Button();
            ButtonClickListener clickListener = new ButtonClickListener();
            DoubleClickListener doubleClickListener = new DoubleClickListener();
            MouseHoverListener mouseHoverListener = new MouseHoverListener();

            // Subscribe listeners to the respective events
            button.Click += clickListener.HandleClick;
            button.DoubleClick += doubleClickListener.HandleDoubleClick;
            button.MouseHover += mouseHoverListener.HandleMouseHover;

            // Simulate various button actions
            Console.WriteLine("Simulating a click:");
            button.OnClick();

            Console.WriteLine("\nSimulating a double-click:");
            button.OnDoubleClick();

            Console.WriteLine("\nSimulating a mouse hover:");
            button.OnMouseHover();

            // Keep the console open
            Console.ReadLine();
        }
    }
}
