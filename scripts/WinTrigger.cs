using Godot;
using System;

using Godot;

public partial class WinTrigger : Area2D
{
	public override void _Ready()
	{
		BodyEntered += OnBodyEntered;
	}
	private void OnBodyEntered(Node2D body)
	{
		if (body.IsInGroup("acorn"))
		{
			GD.Print("You Win!");
		}
	}
}
