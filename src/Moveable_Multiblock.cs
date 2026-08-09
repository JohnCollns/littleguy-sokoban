using Godot;
using System;

public partial class Moveable_Multiblock : Moveable
{
    [Export] private Vector2[] ExtraCoords;
    private Vector2I[] ExtraCoords_Internal;

    public override void _Ready()
    {
        base._Ready();
        ExtraCoords_Internal = new Vector2I[ExtraCoords.Length];
        for (int i=0; i < ExtraCoords.Length; i++)
        {
            ExtraCoords_Internal[i] = MathsHelper.Vector2IFromVector2(ExtraCoords[i]);
        }
    }

    public override bool CanMove(Vector2I direction)
    {
        bool bResult = base.CanMove(direction);
        foreach (Vector2I coord in ExtraCoords_Internal)
        {
            bResult &= CanMoveStatic(tilePos + coord, direction);
        }
        return bResult;
    }

    public override bool DoesOccupyPosition(Vector2I position)
    {
        bool bResult = base.DoesOccupyPosition(position);
        foreach (Vector2I coord in ExtraCoords_Internal)
        {
            bResult |= (tilePos + coord) == position;
        }
        return bResult;
    }
}
