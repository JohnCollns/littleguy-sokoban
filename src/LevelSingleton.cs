using Godot;
using System;
using System.Collections.Generic;

//[GlobalClass]
public partial class LevelSingleton : Node
{
    public static LevelSingleton Instance { get; private set; }
    private TileMapLayer TileMapLayer;
    public List<Moveable> Moveables = new List<Moveable>();

    public override void _EnterTree()
    {
        base._EnterTree();
        LevelSingleton.Instance = this;
    }

    public override void _Ready()
    {
        base._Ready();
        GetTree().SceneChanged += OnLevelChanged;
        OnLevelChanged();
    }

    private void OnLevelChanged()
    {
        TileMapLayer = (TileMapLayer)GetTree().GetFirstNodeInGroup("LevelTileMap");
        GD.Print($"OnLevelChanged(), TileMapLayer found: {TileMapLayer != null}");
    }

    public bool IsTileWall(Vector2I pos)
    {
        if (TileMapLayer == null)
            return false;
        int posSourceID = TileMapLayer.GetCellSourceId(pos); 
        return Constants.BlockingTileMapIDs.Contains(posSourceID);
    }

    public Moveable GetMoveableAtPosition(Vector2I pos)
    {
        // Is there a more computationally efficient way of doing this? 
        foreach (Moveable moveable in Moveables)
        {
            if (moveable.tilePos == pos)
            {
                return moveable;
            }
        }
        return null;
    }
}
