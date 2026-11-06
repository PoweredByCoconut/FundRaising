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
		_Sprite.FlipH = true;
	}

	public void MakeChoice(Character opponent)
	{
		int hp = this.player.getHealth();
		int atkPwr = this.player.getAtkPwr();
		int defense = this.player.getDefense();

		int o_hp = opponent.getHealth();
		int o_atkPwr = opponent.getAtkPwr();
		int o_defense = opponent.getDefense();

		var rng = new Random();
		int randomness = rng.Next(8);

		if ((atkPwr - o_defense) >= o_hp
			|| (hp == 30) || o_hp > 15 || randomness == 5
		  || (atkPwr <= 2))
		{
			activateChoice('A');
		}
		else if ((o_atkPwr > 5 && defense < 5)
			 || randomness == 7)
		{
			activateChoice('D');
		}
		else
		{
			activateChoice('H');
		}
	}

	private void activateChoice(char choice)
	{
		EmitSignal(SignalName.PlayerAction, choice);
	}
}
