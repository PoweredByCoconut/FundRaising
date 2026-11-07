using Godot;
using System;

public partial class PitTrap : Area2D
{
    [Export]
    private CollisionShape2D _collider;

    [Export]
    private RigidBody2D _rock;
    public override void _Ready()
    { 
        BodyEntered += OnBodyEntered;
    }


    private void OnBodyEntered(Node2D body)
    {
        if (body.IsInGroup("rock"))
        {
            _rock.SetDeferred(RigidBody2D.PropertyName.Freeze, true);

            _rock.GetChild<CollisionShape2D>(1)
                 .SetDeferred(CollisionShape2D.PropertyName.Disabled, true);

            _collider.SetDeferred(CollisionShape2D.PropertyName.Disabled, false);
        }
    }

}