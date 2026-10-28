using Godot;
using System;
using CharacterNS;

public partial class FightGuys : Node2D
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Character char_1 = new Character(30);
		GD.Print($"Initial Health: {char_1.getHealth()}");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
