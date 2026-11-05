using CharacterNS;
using Godot;
using System;

public partial class GameController : Node2D
{
	private PlayerCharacter _Player;
	private EnemyCharacter _Enemy;
	private int PlayerHealth;
	private int EnemyHealth;

	private bool PlayersTurn = true;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		base._Ready();
		_Player = GetNode<PlayerCharacter>("PlayerCharacter");
		_Enemy = GetNode<EnemyCharacter>("EnemyCharacter");

		_Enemy.AttackPlayer += OnEnemyAttackPlayer;
		_Player.PlayerAction += OnPlayerAction;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	private void GameTurn(char action)
	{
		// Point at whoever's turn it is
		Character currentTurn;
		Character defender;
		if (PlayersTurn) {
			currentTurn = _Player.player;
			defender = _Enemy.player;
		} else {
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
				GD.Print("Player Wins");
			} else if (defender == _Player.player)
			{
				GD.Print("Player Loses");
			} else
			{
				// should never run if this runs there is a bug
				GD.Print("What the crap?!");
			}
		}
	}

	public void OnEnemyAttackPlayer()
	{
		_Enemy.player.attack(_Player.player);
		_Player._HealthBar.Value = _Player.player.getHealth();
	}
	public void OnPlayerAction(char action)
	{
		if (PlayersTurn) GameTurn(action);
	}
}
