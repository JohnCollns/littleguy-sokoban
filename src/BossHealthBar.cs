using Godot;
using System;

public partial class BossHealthBar : Node
{
    [Export] public Texture2D FriendTexture;
    [Export] public Texture2D WizardTexture;

    [Export] public Sprite2D[] FriendPips;
    [Export] public Sprite2D[] WizardPips;
    
    public static BossHealthBar Instance;

    public override void _EnterTree()
    {
        base._EnterTree();
        Instance = this;
    }

    public override void _ExitTree()
    {
        base._ExitTree();
        if (Instance == this)
            Instance = null;
    }

    public void SetWizardHealthBar(int Health)
    {
        SetHealthBar(WizardPips, Health);
    }

    public void SetFriendHealthBar(int Health)
    {
        SetHealthBar(FriendPips, Health);
    }

    protected void SetHealthBar(Sprite2D[] array, int Health)
    {
        for (int i = 0; i < array.Length; i++)
        {
            array[i].Visible = i < Health;
        }
    }
}
