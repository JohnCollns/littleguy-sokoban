using Godot;
using System;

public partial class Moveable : Node2D
{
	public Vector2I tilePos { get; set; }
	[Export] public EBlockType BlockType { get; protected set; }
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
		return CanMoveStatic(tilePos, direction, this);
	}

	// Should rename this function. 
	// This is just a static version of CanMove(), it uses any location and then sees if its move is valid. 
	public static bool CanMoveStatic(Vector2I tilePos, Vector2I direction, Moveable sourceMoveable)
	{
		if (LevelSingleton.Instance.IsTileWall(tilePos + direction))
			return false;
		
		// Recursively check if this would push a moveable, and if that moveable can move in the direction. 
		if (LevelSingleton.Instance.GetMoveableAtPosition(tilePos + direction) is Moveable moveableToPush)
		{
			GD.Print($"Querying moveable at: {moveableToPush.tilePos}");
			if (sourceMoveable == moveableToPush)
			{
				return true;
			}
			return moveableToPush.CanMove(direction);
		}
		return true;
	}

	public virtual bool DoesOccupyPosition(Vector2I position)
	{
		return position == tilePos;
	}

	public virtual void Move(Vector2I direction)
	{
		if (LevelSingleton.Instance.GetMoveableAtPosition(tilePos + direction) is Moveable moveableToPush)
		{
			if (moveableToPush.BlockType == EBlockType.Orb && BlockType != EBlockType.Bouncer)
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
		Slide(direction);
		LevelSingleton.Instance.AddMoveToTurn(this, direction);
	}

	public virtual void Slide(Vector2I Direction)
	{
		ApplyTilePos();
		Vector2 target = PredictTilePos(Direction);
		tilePos += Direction;

		tween = CreateTween();
		tween.TweenProperty(this, "position", target, Constants.MoveDuration);
		//GD.Print($"Slide, tweening from: {tilePos} => {Position}, to: {target}");
	}

	public virtual bool IsInGoal()
	{
		return LevelSingleton.Instance.IsTileGoal(tilePos);
	}

	public bool IsLittleGuy()
	{
		return Constants.Friends.Contains(BlockType);
	}

	public virtual void Destroy()
	{
		// don't know what to put
		if (Constants.Friends.Contains(BlockType))
		{
			LevelSingleton.Instance.EmitFriendDamaged();
		}

		QueueFree();
	}
}
