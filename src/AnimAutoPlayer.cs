using Godot;
using System;

public partial class AnimAutoPlayer : Node
{
	[Export] public Godot.AnimationPlayer AnimationPlayer;
	[Export] public StringName AnimToPlay;
	[Export] public PackedScene SceneToPlay;
	[Export] public bool IsRandomStart;

	public override async void _Ready()
	{
		base._Ready();

		if (IsRandomStart)
		{
			float randomDelay = (float)GD.RandRange(0.0, 3);
			await ToSignal(GetTree().CreateTimer(randomDelay), SceneTreeTimer.SignalName.Timeout);
		}

		AnimationPlayer.Play(AnimToPlay);
		AnimationPlayer.AnimationFinished += OnAnimationComplete;
	}

	private void OnAnimationComplete(StringName animName = null)
	{
		if (animName != null)
		{
			GetTree().ChangeSceneToPacked(SceneToPlay);
		}
	}
}
