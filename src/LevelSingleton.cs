using Godot;
using System;
using System.Collections.Generic;

[GlobalClass]
public partial class LevelSingleton : Node
{
    public static LevelSingleton Instance { get; private set; }
    private TileMapLayer TileMapLayer;
    private List<Moveable> Moveables;

    public override void _Ready()
    {
        base._Ready();
        Instance = this;
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
}
