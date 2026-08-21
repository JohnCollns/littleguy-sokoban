using Godot;

public partial class Confetti : Node2D
{
	[Export] public PackedScene ConfettiScene;

	public override async void _Ready()
	{
		await ToSignal(GetTree().CreateTimer(2.5f), SceneTreeTimer.SignalName.Timeout);
		
		while (true)
		{
			Spawn();
			await ToSignal(GetTree().CreateTimer(GD.RandRange(1.5, 3.5)), SceneTreeTimer.SignalName.Timeout);
		}
	}

	private void Spawn()
	{
		Node2D confetti = ConfettiScene.Instantiate<Node2D>();
		
		// Spawn in the middle 90% of the screen 
		float widthBorder = GetViewportRect().Size.X * 0.1f;
		float heightBorder = GetViewportRect().Size.Y * 0.1f;

		confetti.Position = new Vector2(
			(float)GD.RandRange((GetViewportRect().Size.X - widthBorder) * -1, GetViewportRect().Size.X - widthBorder),
			(float)GD.RandRange((GetViewportRect().Size.Y - heightBorder) * -1, GetViewportRect().Size.Y - heightBorder)
		);
		
		confetti.ZIndex = -10; // put in background
		AddChild(confetti);

		AnimatedSprite2D sprite = confetti.GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		sprite.Show();
		sprite.Play("default");
		sprite.AnimationFinished += confetti.QueueFree;
	}
}
