using CharacterNS;
using Godot;
using System;

public partial class PlayerCharacter : CharacterBody2D
{
  [Signal]
  public delegate void PlayerActionEventHandler(char action);
	public ProgressBar _HealthBar;
  public Character player;
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
	EmitSignal(SignalName.PlayerAction, 'A');
  }

  private void OnCPressed()
  {
	EmitSignal(SignalName.PlayerAction, 'D');
  }

  private void OnZPressed()
  {
	EmitSignal(SignalName.PlayerAction, 'H');
  }
}
