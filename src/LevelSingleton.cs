using Godot;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Godot.Collections;

//using Godot.Collections;

public struct MoveRecord
{
    public Moveable Moveable;
    public Vector2I Direction;

    public MoveRecord(Moveable moveable_, Vector2I direction_)
    {
        Moveable = moveable_;
        Direction = direction_;
    }
}

public partial class LevelSingleton : Node
{
    public static LevelSingleton Instance { get; private set; }
    private TileMapLayer TileMapLayer;
    public List<Moveable> Moveables = new List<Moveable>();
    private List<List<MoveRecord>> MoveHistory = new List<List<MoveRecord>>();
    public LevelInfo LevelInfo;

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
        MoveHistory.Clear();
        TileMapLayer = (TileMapLayer)GetTree().GetFirstNodeInGroup("LevelTileMap");
        GD.Print($"OnLevelChanged(), TileMapLayer found: {TileMapLayer != null}");
    }
    
    [Signal]
    public delegate void StartTurnEventHandler();

    public int GetTileMapTerrainIDAtPos(Vector2I pos)
    {
        if (TileMapLayer.GetCellTileData(pos) is TileData arg)
        {
            return arg.Terrain;
        }
        return -1;
    }
    
    public bool IsTileWall(Vector2I pos)
    {
        if (TileMapLayer == null)
            return false;
        
        int posSourceID = TileMapLayer.GetCellSourceId(pos);
        int posTerrainID = GetTileMapTerrainIDAtPos(pos);
        return Constants.BlockingTileMapIDs.Contains(posSourceID) || Constants.WallTerrainID == posTerrainID;
    }

    public bool IsTileGoal(Vector2I pos)
    {
        return GetTileMapTerrainIDAtPos(pos) == Constants.GoalTerrainID;
    }

    public bool IsTileWizard(Vector2I pos)
    {
        return GetTileMapTerrainIDAtPos(pos) == Constants.WizardTerrainID;
    }

    public bool IsTileBlockType(Vector2I pos, EBlockType type)
    {
        if (GetMoveableAtPosition(pos) is Moveable moveable)
        {
            return moveable.BlockType == type;
        }
        return false;
    }

    public Moveable GetMoveableAtPosition(Vector2I pos)
    {
        // Is there a more computationally efficient way of doing this? 
        foreach (Moveable moveable in Moveables)
        {
            if (moveable.DoesOccupyPosition(pos))
            {
                return moveable;
            }
        }
        return null;
    }

    public void StartNewTurn()
    {
        EmitSignal(SignalName.StartTurn);
        MoveHistory.Add(new List<MoveRecord>());
        TestForVictory();
    }

    public void AddMoveToTurn(Moveable moveable, Vector2I direction)
    {
        MoveRecord moveRecord = new MoveRecord(moveable, direction);
        MoveHistory.Last().Add(moveRecord);
    }

    public void UndoPreviousTurn()
    {
        if (MoveHistory.Count == 0)
            return;

        var lastMove = MoveHistory.Last();
        MoveHistory.RemoveAt(MoveHistory.Count - 1);
        foreach (MoveRecord moveRecord in lastMove)
        {
            moveRecord.Moveable.Slide(-moveRecord.Direction);
        }
    }

    private void TestForVictory()
    {
        foreach (Moveable moveable in Moveables)
        {
            if (moveable.IsLittleGuy())
            {
                if (!moveable.IsInGoal())
                {
                    return;
                }
            }
        }
        Victory();
    }

    private void Victory()
    {
        GD.Print($"Victory on level: {LevelInfo.LevelName}");
        GD.Print($"About to try loading level: {LevelInfo.NextLevel}");
        GetTree().ChangeSceneToFile(LevelInfo.NextLevel);
    }
}
