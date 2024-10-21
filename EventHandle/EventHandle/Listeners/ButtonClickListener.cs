using System;

namespace EventHandle.Listeners
{
    public class ButtonClickListener
    {
        // Method to handle Click event
        public void HandleClick(object sender, EventArgs e)
        {
            Console.WriteLine("ButtonClickListener: Handling Click event.");
        }
    }
}
