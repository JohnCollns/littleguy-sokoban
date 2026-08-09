using Godot;
using System;

public partial class Moveable : Node2D
{
    public Vector2I tilePos { get; protected set; }
    protected Tween tween;

    public override void _Ready()
    {
        base._Ready();
        
    }

    public void ApplyTilePos()
    {
        Position = (tilePos * Constants.GridUnits) + Constants.HalfGridOffset;
    }
    public Vector2 PredictTilePos(Vector2I direction)
    {
        return ((tilePos + direction) * Constants.GridUnits) + Constants.HalfGridOffset;
    }

    public virtual bool CanMove(Vector2I direction)
    {
        if (LevelSingleton.Instance.IsTileWall(tilePos + direction))
            return false;
        return true;
    }

    public virtual void Move(Vector2I Direction)
    {
        Slide(Direction);
    }

    public void Slide(Vector2I Direction)
    {
        ApplyTilePos();
        tilePos += Direction;

        Vector2 target = PredictTilePos(Direction);
        //tween = new Tween();
        tween = CreateTween();
        // tween.tween_property(self, "position", target, 0.08f);
        tween.TweenProperty(this, "position", target, Constants.MoveDuration);
    }
}
