using CharacterNS;
using Godot;
using System;

public partial class PlayerCharacter : CharacterBody2D
{
	[Signal]
	public delegate void PlayerActionEventHandler(char action);
	public ProgressBar _HealthBar;
	public Label _AtkPwr;
	public Label _Defense;
	public Label _LastAction;
	private AnimatedSprite2D _Sprite;
	public Character player;
	public override void _Ready()
	{
		base._Ready();
		_HealthBar = GetNode<ProgressBar>("HealthBar");
		_AtkPwr = GetNode<Label>("AtkPwr");
		_Defense = GetNode<Label>("Defense");
		_LastAction = GetNode<Label>("LastAction");
		_Sprite = GetNode<AnimatedSprite2D>("Sprite");

		player = new Character(30);
		_HealthBar.Value = player.getHealth();
		_HealthBar.MaxValue = player.getHealth();
		_AtkPwr.Text = $"Attack Power: {player.getAtkPwr()}";
		_Defense.Text = $"Defensive Power: {player.getDefense()}";
		_LastAction.Text = "";
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

		_AtkPwr.Text = $"Attack Power: {player.getAtkPwr()}";
		_Defense.Text = $"Defensive Power: {player.getDefense()}";
	}

	async void OnXPressed()
	{
		EmitSignal(SignalName.PlayerAction, 'A');
		await ToSignal(GetTree().CreateTimer(3f), "timeout");
	}

	async void OnCPressed()
	{
		EmitSignal(SignalName.PlayerAction, 'D');
		await ToSignal(GetTree().CreateTimer(3f), "timeout");
	}

	async void OnZPressed()
	{
		EmitSignal(SignalName.PlayerAction, 'H');
		await ToSignal(GetTree().CreateTimer(3f), "timeout");
	}
}
