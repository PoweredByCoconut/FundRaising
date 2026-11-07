using Godot;
using System;

public partial class SceneTransition : CanvasLayer
{
	[Export]
	private ColorRect _fade;
	[Export]
	private Control _menu;
	[Export]
	private Button _nextButton;
	[Export]
	private Label _finalText;
	public override void _Ready()
	{

	}

	public async void ChangeScene(int level)
	{
		var tween = CreateTween();
		_fade.Visible = true;
		tween.TweenProperty(
			_fade,
			"modulate:a",
			1.0f,
			0.5f);

		await ToSignal(tween, "finished");
		GD.Print("Loading scene " + level);
		String path = "res://games/nutty-business/levels/level_" + level + ".tscn";
		GetTree().ChangeSceneToFile(path);
	}

	public async void FadeIn()
	{
		var tween = CreateTween();
		tween.TweenProperty(
			_fade,
			"modulate:a",
			0.0f,
			0.5f);

		await ToSignal(tween, "finished");
		_fade.Visible = false;
		GameManager.Instance.Paused = false;
	}
	public void ShowMenu(bool finalLevel)
	{
		_menu.Visible = true;
		GameManager.Instance.Paused = true;
		if (finalLevel)
		{
			_nextButton.Visible = false;
			_finalText.Visible = true;
		}
	}

	private void OnNextLevelPressed()
	{
		ChangeScene(GameManager.Instance.CurrentLevel + 1);
	}

	private void OnNewLevelPressed(int level)
	{
		ChangeScene(level);
	}

	public void UnlockLevel(int levelNumber)
	{
		for (int i = 1; i <= levelNumber; i++)
		{
			_menu.GetChild<Button>(i, false).Disabled = false;
		}
	}
}
