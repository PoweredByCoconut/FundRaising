using CharacterNS;
using Godot;
using System;

public partial class PlayerCharacter : CharacterBody2D
{
	private ProgressBar _HealthBar;
  public override void _Ready()
  {
	base._Ready();
	_HealthBar = GetNode<ProgressBar>("HealthBar");

	  Character player = new Character(30);
	  _HealthBar.Value = player.getHealth();
  }

}
