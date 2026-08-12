using Godot;
using System;

public partial class PlayAnimation : Node
{
	[Export] public AnimationPlayer player;

public override void _Ready()
	{
		// 1. Fallback to child node if not set in Inspector
		if (player == null)
		{
			player = GetNodeOrNull<AnimationPlayer>("AnimationPlayer");
		}

		// 2. Safety check: avoid NullReferenceException crash
		if (player == null)
		{
			GD.PrintErr("[BossTransition] ERROR: Could not find AnimationPlayer node!");
			return;
		}

		// 4. Play animation
		player.Play("panup");
	}
}
