using Godot;
using System;
using CharacterNS;

public partial class EnemyCharacter : CharacterBody2D
{
  [Signal]
  public delegate void AttackPlayerEventHandler();
	public ProgressBar _HealthBar;
  public Character player;
  public override void _Ready()
  {
	  base._Ready();
	  _HealthBar = GetNode<ProgressBar>("HealthBar");

	player = new Character(30);
	  _HealthBar.Value = player.getHealth();

	  doAttack();
  }
  
  public void doAttack()
  {
	EmitSignal(SignalName.AttackPlayer, 10);
  }
}
