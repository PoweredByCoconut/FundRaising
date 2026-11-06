using CharacterNS;
using Godot;
using System;

public partial class GameController : Node2D
{
	private PlayerCharacter _Player;
	private EnemyCharacter _Enemy;
	private Label _GameResult;
	private int PlayerHealth;
	private int EnemyHealth;

	private bool PlayersTurn = true;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		base._Ready();
		_Player = GetNode<PlayerCharacter>("PlayerCharacter");
		_Enemy = GetNode<EnemyCharacter>("EnemyCharacter");
		_GameResult = GetNode<Label>("GameResult");

		_Enemy.PlayerAction += OnEnemyAction;
		_Player.PlayerAction += OnPlayerAction;
		_GameResult.Text = "";

		GameTurn('A');
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	private void GameTurn(char action)
	{
		if (_GameResult.Text != "") return;
		// Point at whoever's turn it is
		Character currentTurn;
		Character defender;
		if (PlayersTurn)
		{
			currentTurn = _Player.player;
			defender = _Enemy.player;
		}
		else
		{
			currentTurn = _Enemy.player;
			defender = _Player.player;
		}
		switch (action)
		{
			case 'A':
				currentTurn.attack(defender);
				break;
			case 'D':
				currentTurn.defend();
				break;
			case 'H':
				currentTurn.heal();
				break;
			default:
				break;
		}
		if (defender.getHealth() <= 0)
		{
			if (defender == _Enemy.player)
			{
				_GameResult.Text = "You Win!";
			}
			else if (defender == _Player.player)
			{
				_GameResult.Text = "You Lose!";
			}
			else
			{
				// should never run if this runs there is a bug
				GD.Print("What the crap?!");
			}
		}
		_Player._HealthBar.Value = _Player.player.getHealth();
		_Player._AtkPwr.Text = $"Attack Power: {_Player.player.getAtkPwr()}";
		_Player._Defense.Text = $"Defensive Power: {_Player.player.getDefense()}";
		_Enemy._HealthBar.Value = _Enemy.player.getHealth();
		_Enemy._AtkPwr.Text = $"Attack Power: {_Enemy.player.getAtkPwr()}";
		_Enemy._Defense.Text = $"Defensive Power: {_Enemy.player.getDefense()}";
		// switch the turn
		PlayersTurn = !PlayersTurn;
		if (!PlayersTurn)
		{
			promptEnemyToAttack(_Player.player);
		}
	}

	private void promptEnemyToAttack(Character opponent)
	{
		_Enemy.MakeChoice(opponent);
	}

	public void OnEnemyAction(char action)
	{
		if (!PlayersTurn) GameTurn(action);
	}
	public void OnPlayerAction(char action)
	{
		if (PlayersTurn) GameTurn(action);
	}
}
