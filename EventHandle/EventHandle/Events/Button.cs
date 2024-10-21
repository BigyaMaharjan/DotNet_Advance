using System;

namespace EventHandle.Events
{
    // Define delegates for multiple event types
    public delegate void ButtonClickHandler(object sender, EventArgs e);
    public delegate void ButtonDoubleClickHandler(object sender, EventArgs e);
    public delegate void ButtonMouseHoverHandler(object sender, EventArgs e);

    // Button class with multiple events
    public class Button
    {
        // Declare events using the delegates
        public event ButtonClickHandler Click;
        public event ButtonDoubleClickHandler DoubleClick;
        public event ButtonMouseHoverHandler MouseHover;

        // Simulate button click and raise Click event
        public void OnClick()
        {
            Console.WriteLine("Button clicked!");
            Click?.Invoke(this, EventArgs.Empty);
        }

        // Simulate button double-click and raise DoubleClick event
        public void OnDoubleClick()
        {
            Console.WriteLine("Button double-clicked!");
            DoubleClick?.Invoke(this, EventArgs.Empty);
        }

        // Simulate mouse hover and raise MouseHover event
        public void OnMouseHover()
        {
            Console.WriteLine("Mouse hovered over the button.");
            MouseHover?.Invoke(this, EventArgs.Empty);
        }
    }
}
