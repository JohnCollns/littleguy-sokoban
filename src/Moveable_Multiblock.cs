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
        GD.Print($"Querying CanMove at pos: {tilePos}. ExtraCoords_Internal length: {ExtraCoords_Internal.Length}");
        bool bResult = base.CanMove(direction);
        foreach (Vector2I coord in ExtraCoords_Internal)
        {
            GD.Print($"Querying CanMove at pos: {tilePos + coord}");
            if (DoesOccupyPosition(coord))
                continue;
            bResult &= CanMoveStatic(tilePos + coord, direction, this);
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

    public override void Move(Vector2I direction)
    {
        foreach (Vector2I coord in ExtraCoords_Internal)
        {
            if (LevelSingleton.Instance.GetMoveableAtPosition(tilePos + direction + coord) is Moveable moveableToPush)
            {
                if (moveableToPush.BlockType == EBlockType.Orb)
                {
                    GD.Print($"Block of type: {BlockType} moved into orb, destroying self");
                    moveableToPush.Destroy();
                    Destroy();
                }
                if (moveableToPush != this)
                {
                    moveableToPush.Move(direction);
                }
            }
        }
        base.Move(direction);
    }

    public override bool IsInGoal()
    {
        bool bResult = base.IsInGoal();
        foreach (Vector2I coord in ExtraCoords_Internal)
        {
            bResult &= LevelSingleton.Instance.IsTileGoal(tilePos + coord);
        }
        return bResult;
    }
}
