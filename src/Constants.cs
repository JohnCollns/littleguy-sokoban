using Godot;
using System;
using System.Collections.Generic;

public enum EBlockType
{
    None,
    Player,
    Crate,
    
    Shrimp,
    WizardGood,
    WizardBad,
    Skeleton,
    Happy,
    Nerd,
    
    Powder,
    Tobacco,
    Marvel,
    Milk,
    
    Orb,
    Bouncer,
}

public static class Constants
{
    public static readonly int SpriteResolution = 256;
    public static readonly float SpriteScale = 0.0390625f; // 10 units between - 256
    //public static readonly float SpriteScale = 0.01953125f; // 10 units between - 512
    
    // If using scaling
    // public static float GridUnits_Float => SpriteResolution * SpriteScale; 
    // public static int GridUnits_Int => (int)(SpriteResolution * SpriteScale); 
    // public static Vector2 HalfGridOffset = new Vector2(GridUnits_Float * 0.5f, GridUnits_Float * 0.5f);
    
    // No scaling
    public static int GridSize => SpriteResolution; 
    public static readonly Vector2 HalfGridOffset = new Vector2(GridSize * 0.5f, GridSize * 0.5f); 
    
    // ANIMATION
    public static readonly float MoveDuration = 0.08f;
    
    // COLLISION
    public static readonly int WallTileMapID = 0;
    public static readonly int HedgeTileMapID = 14;
    public static HashSet<int> BlockingTileMapIDs = new HashSet<int>()
    {
        WallTileMapID, HedgeTileMapID
    };

    public static readonly int WallTerrainID = 0;
    public static readonly int GrassTerrainID = 1;  // USELESS
    public static readonly int GoalTerrainID = 2;
    public static readonly int WizardTerrainID = 3;
    
    // GAMEPLAY
    public static HashSet<EBlockType> Friends = new HashSet<EBlockType>()
    {
        EBlockType.Shrimp,
        EBlockType.WizardGood,
        EBlockType.WizardBad,
        EBlockType.Skeleton,
        EBlockType.Happy,
        EBlockType.Nerd,
    };

    public static Vector2 TileCoordToSpace(Vector2I tileCoord)
    {
        return new Vector2(tileCoord.X * SpriteResolution, tileCoord.Y * SpriteResolution);
    }

    public static readonly Vector2I[] RandomDirections = new Vector2I[]
    {
        new Vector2I(1, 0),
        new Vector2I(-1, 0),
        new Vector2I(0, 1),
        new Vector2I(0, -1),
    };

    public static readonly float ChanceToStayStill = 0.7f;
    public static Vector2I GetRandomDirection()
    {
        var rng = new Random();
        return RandomDirections[rng.Next(RandomDirections.Length - 1)];
    }

    public static Vector2I GetRandomDirectionOrStayStill()
    {
        var rng = new Random();
        if (rng.NextDouble() < ChanceToStayStill)
        {
            return Vector2I.Zero;
        }
        return GetRandomDirection();
    }

    public static readonly string BossLevelString = "res://content/scenes/boss_test.tscn";
    public static readonly string PreBossLevelString = "res://content/scenes/boss_transition.tscn";
}
