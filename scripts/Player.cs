using Godot;
using System;

public partial class Player : CharacterBody2D
{
	public const float Speed = 250.0f;
	public const float JumpVelocity = -350.0f;

	private AnimatedSprite2D _sprite;

	public override void _Ready()
	{
		_sprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		_sprite.Play("idle");
	}

	public override void _PhysicsProcess(double delta)
	{
		Vector2 velocity = Velocity;

		// Add gravity.
		if (!IsOnFloor())
		{
			velocity += GetGravity() * (float)delta;
		}

		// Handle Jump.
		if (Input.IsActionJustPressed("ui_accept") && IsOnFloor())
		{
			velocity.Y = JumpVelocity;
		}

		// Movement
		Vector2 direction = Input.GetVector("ui_left", "ui_right", "ui_up", "ui_down");

		if (direction != Vector2.Zero)
		{
			velocity.X = direction.X * Speed;

			// Face movement direction
			if (direction.X < 0)
				_sprite.FlipH = true;
			else if (direction.X > 0)
				_sprite.FlipH = false;
		}
		else
		{
			velocity.X = Mathf.MoveToward(Velocity.X, 0, Speed);
		}

		// Animation Logic
		if (!IsOnFloor())
		{
			if (velocity.Y < 1)
				_sprite.Play("jump");
			else
				_sprite.Play("fall");
		}
		else if (direction != Vector2.Zero)
		{
			_sprite.Play("run");
		}
		else
		{
			_sprite.Play("idle");
		}

		Velocity = velocity;
		

		for (int i = 0; i < GetSlideCollisionCount(); i++)
		{
			KinematicCollision2D collision = GetSlideCollision(i);

			if (collision.GetCollider() is RigidBody2D body)
			{
				float pushForce = 120.0f;

				body.ApplyCentralForce(
					new Vector2(Mathf.Sign(Velocity.X) * pushForce, 0)
				);
			}
		}
		MoveAndSlide();
	}
}
