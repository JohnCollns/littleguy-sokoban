using Godot;
using System;
using Godot.Collections;

public partial class Player : Moveable
{
    protected static Dictionary<string, Vector2I> InputDirections = new Dictionary<string, Vector2I>()
    {
        { "W", new Vector2I(0, -1) },
        { "S", new Vector2I(0, 1) },
        { "D", new Vector2I(1, 0) },
        { "A", new Vector2I(-1, 0) },
    };
    
    public override void _Input(InputEvent @event)
    {
        if (@event.IsPressed())
        {
            GD.Print($"Received InputEvent: {@event.AsText()}");
            if (InputDirections.ContainsKey(@event.AsText()))
            {
                HandleMovementInput(@event);
                return;
            }

            // Undo
        }
    }
    
    protected void HandleMovementInput(InputEvent @event)
    {
        Vector2I direction = InputDirections[@event.AsText()];
        GD.Print($"Handling Movement in direction: {direction},\tCanMove: {CanMove(direction)}");
        if (CanMove(direction))
        {
            Move(direction);
        }
    }
}
