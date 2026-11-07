using Godot;

public partial class Menu : Node
{
	public void LoadTestGame()
	{
		GetTree().ChangeSceneToFile("res://games/test-game/root.tscn");
	}

	public void LoadFightGuys()
	{
		GetTree().ChangeSceneToFile("res://games/fight-guys/Scenes/Levels/FightGuys.tscn");
	}
}
