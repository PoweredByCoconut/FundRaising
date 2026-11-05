using CharacterNS;
using Godot;
using System;

public partial class PlayerCharacter : CharacterBody2D
{
  [Signal]
  public delegate void AttackEnemyEventHandler();
	private ProgressBar _HealthBar;
  private Character player;
  public override void _Ready()
  {
	base._Ready();
	_HealthBar = GetNode<ProgressBar>("HealthBar");

	player = new Character(30);
	  _HealthBar.Value = player.getHealth();
	
  }

  public override void _Input(InputEvent @event)
  {
	if (@event is not InputEventKey keyEvent || !keyEvent.Pressed || keyEvent.Echo)
	{
	  return;
	}

	switch (keyEvent.Keycode)
	{
	  case Key.X:
		OnXPressed();
		break;
	  case Key.C:
		OnCPressed();
		break;
	  case Key.Z:
		OnZPressed();
		break;
	}
  }

  private void OnXPressed()
  {
	doAttack();
	GD.Print("Attacking");
  }

  private void OnCPressed()
  {
	this.player.defend();
	GD.Print("Defending");
  }

  private void OnZPressed()
  {
	this.player.heal();
	GD.Print("Healing");
  }

  public void doAttack()
  {
	EmitSignal(SignalName.AttackEnemy);
  }
}
