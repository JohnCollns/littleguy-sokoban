using Godot;
using System;

public partial class AnimAutoPlayer : Node
{
    [Export] public Godot.AnimationPlayer AnimationPlayer;
    [Export] public StringName AnimToPlay;
    [Export] public string SceneToPlay;

    public override void _Ready()
    {
        base._Ready();
        AnimationPlayer.Play(AnimToPlay);
        AnimationPlayer.AnimationFinished += OnAnimationComplete;
    }

    private void OnAnimationComplete(StringName animName)
    {
        GetTree().ChangeSceneToFile(SceneToPlay);
    }
}
