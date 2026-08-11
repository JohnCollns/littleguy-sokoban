using Godot;
using System;

public partial class LevelInfo : Node
{
    [Export] public string LevelName;
    [Export] public string NextLevel;

    public override void _EnterTree()
    {
        LevelSingleton.Instance.LevelInfo = this;
    }
}
