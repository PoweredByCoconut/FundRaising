using Godot;
using System;
using CharacterNS;

public partial class EnemyCharacter : CharacterBody2D
{
	[Signal]
	public delegate void PlayerActionEventHandler(char action);
	public ProgressBar _HealthBar;
	public Label _AtkPwr;
	public Label _Defense;
	private Label _LastAction;
	public Character player;
	public override void _Ready()
	{
		base._Ready();
		_HealthBar = GetNode<ProgressBar>("HealthBar");
		_AtkPwr = GetNode<Label>("AtkPwr");
		_Defense = GetNode<Label>("Defense");
		_LastAction = GetNode<Label>("LastAction");

		player = new Character(30);
		_HealthBar.Value = player.getHealth();
		_HealthBar.MaxValue = player.getHealth();
		_AtkPwr.Text = $"Attack Power: {player.getAtkPwr()}";
		_Defense.Text = $"Defensive Power: {player.getDefense()}";
		_LastAction.Text = "";
	}

	public void MakeChoice(Character opponent)
	{
		int hp = this.player.getHealth();
		int atkPwr = this.player.getAtkPwr();
		int defense = this.player.getDefense();

		int o_hp = opponent.getHealth();
		int o_atkPwr = opponent.getAtkPwr();
		int o_defense = opponent.getDefense();

		if ((atkPwr - o_defense) >= o_hp
			|| (hp == 30) || o_hp > 15)
		{
			activateChoice('A');
			_LastAction.Text = "Attacked";
		}
		else if ((o_atkPwr > 5 && defense < 5)
			|| defense < 5)
		{
			activateChoice('D');
			_LastAction.Text = "Defended";
		}
		else
		{
			activateChoice('H');
			_LastAction.Text = "Healed";
		}
	}

	private void activateChoice(char choice)
	{
		EmitSignal(SignalName.PlayerAction, choice);
	}
}
