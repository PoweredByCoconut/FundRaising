using Godot;
using System;

public partial class RespawnTrigger : Area2D
{
	[Export]
	private  bool _respawnPlayer;
	[Export]
	private bool _respawnAcorn;
    [Export]
    private float _delay;

    private Vector2 _originalAcornPosition;
	private Vector2 _originalPlayerPosition;

    [Export]
    private RigidBody2D _acorn;

	public override void _Ready()
	{
        if(_acorn == null)
		{
            _acorn = GetNode<RigidBody2D>("../Scene/Layer1/Acorn");
        }
		_originalAcornPosition = _acorn.GlobalPosition;
        BodyEntered += OnBodyEntered;
	}
	
	
	private async void OnBodyEntered(Node2D body)
	{
		if (body.IsInGroup("player"))
		{
			if (_respawnPlayer) { 
				
			}
		}
		else if(body.IsInGroup("acorn"))
        {
			if (_respawnAcorn)
			{
                GD.Print($"Respawning to {_originalAcornPosition}");
                GD.Print($"Current: {_acorn.GlobalPosition}");
                await ToSignal(GetTree().CreateTimer(_delay), SceneTreeTimer.SignalName.Timeout);
                _acorn.Freeze = true;

                Transform2D transform = _acorn.GlobalTransform;
                transform.Origin = _originalAcornPosition;

                _acorn.GlobalTransform = transform;

                _acorn.LinearVelocity = Vector2.Zero;
                _acorn.AngularVelocity = 0;

                await ToSignal(GetTree(), SceneTree.SignalName.PhysicsFrame);

                _acorn.Freeze = false;
                GD.Print($"Current Location after respawn: {_acorn.GlobalPosition}");
            }

        }
    }


}
