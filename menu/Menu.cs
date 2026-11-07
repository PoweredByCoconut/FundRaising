using Godot;
using System;

public partial class Menu : Node
{
	public void LoadTestGame()
	{
		GetTree().ChangeSceneToFile("res://games/test-game/root.tscn");
	}
}
