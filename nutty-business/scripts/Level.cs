using Godot;
using System;

public partial class Level : Node2D
{
	[Export]
	private int _levelNumber = 1;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		GameManager.Instance.OnLevelLoaded(_levelNumber);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
