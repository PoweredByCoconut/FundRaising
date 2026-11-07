using CharacterNS;
using Godot;
using System;
using System.Threading.Tasks;

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

	private async Task GameTurn(char action)
	{
		if (_GameResult.Text != "") return;
		// Point at whoever's turn it is
		Character currentTurn;
		Character defender;
		Label actionMarker;
		if (PlayersTurn)
		{ 
			actionMarker = _Player._LastAction;
			currentTurn = _Player.player;
			defender = _Enemy.player;
		}
		else
		{
			actionMarker = _Enemy._LastAction;
			currentTurn = _Enemy.player;
			defender = _Player.player;
		}
		switch (action)
		{
			case 'A':
				currentTurn.attack(defender);
				actionMarker.Text = "Attacked";
				break;
			case 'D':
				currentTurn.defend();
				actionMarker.Text = "Defended";
				break;
			case 'H':
				currentTurn.heal();
				actionMarker.Text = "Healed";
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
		GD.Print("Start");
		await ToSignal(GetTree().CreateTimer(3f), "timeout");
		if (!PlayersTurn)
		{
			promptEnemyToAttack(_Player.player);
		}
		GD.Print("Stop");
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
	async void wait()
	{
		GD.Print("Start");
		await ToSignal(GetTree().CreateTimer(3f), "timeout");
		GD.Print("Stop");
	}
}
