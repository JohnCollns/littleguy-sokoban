using Godot;
using System;

public partial class WizardManager : Node
{
	[Export] private PackedScene OrbScene;
	[Export] private Vector2I OrbSpawnPeriodRange = new Vector2I(4, 8);
	[Export] private int MaxHealth = 10;
	private int CurrentHealth;
	public bool IsAlive => CurrentHealth > 0;
	[Export] private Vector2[] OrbSpawningCoords;
	private Vector2I[] OrbSpawningCoords_Internal;
	private int TurnsUntilNextOrbSpawn = 2;
	[Export] public Sprite2D[] BossSprites;
	[Export] public PackedScene SceneToPlay;

	public static WizardManager Instance;
	

	public override void _Ready()
	{
		CurrentHealth = MaxHealth;
		BossHealthBar.Instance.SetWizardHealthBar(CurrentHealth);
		OrbSpawningCoords_Internal = new Vector2I[OrbSpawningCoords.Length];
		for (int i=0; i < OrbSpawningCoords.Length; i++)
		{
			OrbSpawningCoords_Internal[i] = MathsHelper.Vector2IFromVector2(OrbSpawningCoords[i]);
		}
		LevelSingleton.Instance.StartTurn += OnTurnStart;
		LevelSingleton.Instance.bIsBossLevel = true;
	}

	public override void _ExitTree()
	{
		base._ExitTree();
		LevelSingleton.Instance.bIsBossLevel = false;
	}

	private void OnTurnStart()
	{
		// This does not respect undos. 
		// Could add a signal to LevelSingleton.UndoLastTurn()

		if (!IsAlive)
			return;
		
		TurnsUntilNextOrbSpawn--;
		if (TurnsUntilNextOrbSpawn == 0)
		{
			var rand = new Random();
			SpawnOrbAtTile(OrbSpawningCoords_Internal[rand.Next(0, OrbSpawningCoords_Internal.Length)]);
			TurnsUntilNextOrbSpawn = rand.Next(OrbSpawnPeriodRange.X, OrbSpawnPeriodRange.Y);
		}
	}

	public override void _EnterTree()
	{
		base._EnterTree();
		Instance = this;
		var orbScene = ResourceLoader.Load<PackedScene>(OrbScene.GetPath());
	}
	
	

	private Moveable_Orb SpawnOrbAtTile(Vector2I pos)
	{
		Moveable_Orb orb = OrbScene.Instantiate<Moveable_Orb>();
		GetTree().CurrentScene.AddChild(orb);
		orb.Position = Constants.TileCoordToSpace(pos);
		orb.tilePos = pos;
		orb.ApplyTilePos();
		return orb;
	}

	public void TakeDamage()
	{
		CurrentHealth -= 1;
		BossHealthBar.Instance.SetWizardHealthBar(CurrentHealth);
		GD.Print($"Wizard took damage, health remaining: {CurrentHealth}");
		FlashRed();
		if (CurrentHealth <= 0)
		{
			Death();
		}
	}

	private void Death()
	{
		// TODO
		if (SceneToPlay != null)
		{
			GetTree().ChangeSceneToPacked(SceneToPlay);
		}
	}

	public void FlashRed()
	{
		if (BossSprites == null || BossSprites.Length == 0) {
			return;
		}

		Tween tween = CreateTween();
		tween.SetParallel(true);

		foreach (Sprite2D sprite in BossSprites)
		{
			if (sprite != null)
			{
				sprite.Modulate = Colors.Red;
				tween.TweenProperty(sprite, "modulate", Colors.White, 0.15f);
			}
		}
	}
}
