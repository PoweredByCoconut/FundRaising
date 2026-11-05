using CharacterNS;
using Godot;
using System;

public partial class GameController : Node2D
{
	private PlayerCharacter _Player;
	private EnemyCharacter _Enemy;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		base._Ready();
		_Player = GetNode<PlayerCharacter>("PlayerCharacter");
		_Enemy = GetNode<EnemyCharacter>("EnemyCharacter");

		_Enemy.AttackPlayer += OnEnemyAttackPlayer;
		_Player.AttackEnemy += OnPlayerAttackEnemy;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void OnEnemyAttackPlayer()
	{
		GD.Print("attackedPlayer");
	}
	public void OnPlayerAttackEnemy()
	{
		GD.Print("Player Attacked");
	}
}
