using Godot;
using System;

public partial class Moveable_Orb : Moveable
{
    [Export] private Texture2D[] OrbTextures;
    private Sprite2D Sprite2D;
    public Vector2I direction { get; protected set; }

    public override void _EnterTree()
    {
        base._EnterTree();
        Sprite2D = GetNode<Sprite2D>("Sprite2D");
        var rng = new Random();
        Sprite2D.Texture = OrbTextures[rng.Next(OrbTextures.Length - 1)];
        direction = new Vector2I(0, 1);
        
        LevelSingleton.Instance.StartTurn += OnTurnStart;
    }

    private void OnTurnStart()
    {
        if (LevelSingleton.Instance.IsTileWizard(tilePos))
        {
            WizardManager.Instance.TakeDamage();
        }
        
        if (CanMove(direction))
        {
            Move(direction);
            return;
        }
        
        // how am I dealing with the reflector/player/wizard?
    }
}
