using Godot;
using System;

using Godot;

public partial class WinTrigger : Area2D
{
	public override void _Ready()
	{
		BodyEntered += OnBodyEntered;
	}
	private async void OnBodyEntered(Node2D body)
	{
		if (body.IsInGroup("acorn"))
		{
            await ToSignal(
            GetTree().CreateTimer(0.3),
            SceneTreeTimer.SignalName.Timeout
			);
            GameManager.Instance.CompleteLevel();
        }
	}
}
