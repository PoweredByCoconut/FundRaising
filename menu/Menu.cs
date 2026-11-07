using Godot;

public partial class Menu : Node
{
	public void LoadFightGuys()
	{
		GetTree().ChangeSceneToFile("res://games/fight-guys/Scenes/Levels/FightGuys.tscn");
	}

	public void LoadBlockDestoyer()
	{
		GetTree().ChangeSceneToFile("res://games/block-destroyer/scenes/root.tscn");
	}

	public void LoadNuttyBusiness()
	{
		GetTree().ChangeSceneToFile("res://games/nutty-business/levels/level_1.tscn");
	}

    public void LoadProjectAegis()
    {
        GetTree().ChangeSceneToFile("res://games/space-game/scenes/main_menu.tscn");
    }
}
