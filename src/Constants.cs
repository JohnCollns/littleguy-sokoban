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
}
