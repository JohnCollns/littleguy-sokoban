using Godot;
using System;

public partial class Moveable : Node2D
{
    public Vector2I tilePos { get; protected set; }
    protected Tween tween;

    public override void _Ready()
    {
        base._Ready();
        DetermineTilePos();
    }

    public override void _EnterTree()
    {
        base._EnterTree();
        LevelSingleton.Instance.Moveables.Add(this);
    }

    public override void _ExitTree()
    {
        base._ExitTree();
        LevelSingleton.Instance.Moveables.Remove(this);
    }

    public void ApplyTilePos()
    {
        Position = (tilePos * Constants.GridSize) + Constants.HalfGridOffset;
    }
    public void DetermineTilePos()
    {
        tilePos = MathsHelper.Vector2IFromVector2((Position - Constants.HalfGridOffset) / (float)Constants.GridSize);
    }
    public Vector2 PredictTilePos(Vector2I direction)
    {
        return ((tilePos + direction) * Constants.GridSize) + Constants.HalfGridOffset;
    }

    public virtual bool CanMove(Vector2I direction)
    {
        if (LevelSingleton.Instance.IsTileWall(tilePos + direction))
            return false;
        
        // Recursively check if this would push a moveable, and if that moveable can move in the direction. 
        if (LevelSingleton.Instance.GetMoveableAtPosition(tilePos + direction) is Moveable moveableToPush)
        {
            return moveableToPush.CanMove(direction);
        }
        return true;
    }

    public virtual void Move(Vector2I direction)
    {
        if (LevelSingleton.Instance.GetMoveableAtPosition(tilePos + direction) is Moveable moveableToPush)
        {
            moveableToPush.Move(direction);
        }
        Slide(direction);
    }

    public void Slide(Vector2I Direction)
    {
        ApplyTilePos();
        Vector2 target = PredictTilePos(Direction);
        tilePos += Direction;

        tween = CreateTween();
        tween.TweenProperty(this, "position", target, Constants.MoveDuration);
        //GD.Print($"Slide, tweening from: {tilePos} => {Position}, to: {target}");
    }
}
