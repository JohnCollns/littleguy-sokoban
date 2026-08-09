using Godot;
using System;

[Tool]
public partial class Block_Base : Node2D
{
    private Sprite2D SpriteNode;
    [Export] public Texture2D SpriteTexture;
    [Export] public bool bBlocksMovement { get; protected set; }
    
    [ExportToolButton("Apply Settings")]
    public virtual Callable ApplySettingsButtonCallable => Callable.From(ApplySettings);
    
    public override void _Ready()
    {
        base._Ready();
        ApplySettings();
    }
    
    public void ApplySettings()
    {
        SpriteNode = GetNode<Sprite2D>("Sprite2D");
        SpriteNode.Texture = SpriteTexture;
        SpriteNode.Scale = Godot.Vector2.One * Constants.SpriteScale;
    }
}
