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
        AssignRandomSprite();
        direction = new Vector2I(0, 1);
        
        LevelSingleton.Instance.StartTurn += OnTurnStart;
    }

    private void OnTurnStart()
    {
        if (LevelSingleton.Instance.IsTileWizard(tilePos))
        {
            WizardManager.Instance.TakeDamage();
            Destroy();
        }

        EBlockType nextMoveBlockType = LevelSingleton.Instance.GetBlocktypeAtPosition(tilePos + direction);
        switch (nextMoveBlockType)
        {
            case EBlockType.Orb:
            {
                GD.Print($"Orb at {tilePos} hit orb.");
                if (LevelSingleton.Instance.GetMoveableAtPosition(tilePos + direction) is Moveable_Orb orb)
                {
                    orb.Destroy();
                }

                Destroy();
                break;
            }
            case EBlockType.Bouncer:
            {
                GD.Print($"Orb at {tilePos} hit bouncer, inverting direction.");
                direction *= -1;
                break;
            }
            case EBlockType.Player:
            {
                GD.Print($"Orb at {tilePos} hit player.");
                LevelSingleton.Instance.EmitPlayerDamaged();
                break;
            }
            default:
            {
                if (Constants.Friends.Contains(nextMoveBlockType))
                {
                    GD.Print($"Orb at {tilePos} hit a friend: {nextMoveBlockType}.");
                    LevelSingleton.Instance.EmitFriendDamaged();
                    if (LevelSingleton.Instance.GetMoveableAtPosition(tilePos + direction) is Moveable moveable)
                    {
                        moveable.QueueFree();
                    }
                    Destroy();
                }
                break;
            }
        }
        
        AssignRandomSprite();
        if (CanMove(direction))
        {
            Move(direction);
            return;
        }
    }

    private void Destroy()
    {
        LevelSingleton.Instance.StartTurn -= OnTurnStart;
        // what??
        QueueFree();
    }

    public override void _ExitTree()
    {
        base._ExitTree();
        LevelSingleton.Instance.StartTurn -= OnTurnStart;
    }

    private void AssignRandomSprite()
    {
        var rng = new Random();
        Sprite2D.Texture = OrbTextures[rng.Next(OrbTextures.Length - 1)];
    }
}
