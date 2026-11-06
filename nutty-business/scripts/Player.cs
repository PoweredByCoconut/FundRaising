using Godot;
using System;

public partial class Player : CharacterBody2D
{
	public const float Speed = 250.0f;
	public const float JumpVelocity = -350.0f;
    private float _coyoteTime = 0.15f;
	private float _coyoteTimer;
    private float _jumpBufferTime = 0.15f;
    private float _jumpBufferTimer;

    private Vector2 _respawnPosition;

    private float _respawnSaveInterval = 1.0f;
    private float _respawnSaveTimer;

    private bool _isRespawning = false;

    private AnimatedSprite2D _sprite;

    public override void _Ready()
    {
        _sprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
        _sprite.Play("idle");

        _respawnPosition = GlobalPosition;
    }

    public override void _PhysicsProcess(double delta)
    {
        

        if (GameManager.Instance.Paused)
        {
            Velocity = Vector2.Zero;
            _sprite.Play("idle");
            return;
        }


        if (_isRespawning)
        {
            Velocity = Vector2.Zero;
            return;
        }

        Vector2 velocity = Velocity;
        if (!_isRespawning && IsOnFloor())
        {
            _respawnSaveTimer += (float)delta;

            if (_respawnSaveTimer >= _respawnSaveInterval)
            {
                _respawnPosition = GlobalPosition;
                _respawnSaveTimer = 0;
            }
        }

        if (IsOnFloor())
        {
            _coyoteTimer = _coyoteTime;
        }
        else
        {
            _coyoteTimer -= (float)delta;
        }

        // Add gravity.
        if (!IsOnFloor())
		{
			velocity += GetGravity() * (float)delta;
		}

        // Handle Jump.
        if (Input.IsActionJustPressed("ui_accept"))
        {
            _jumpBufferTimer = _jumpBufferTime;
        }
        else
        {
            _jumpBufferTimer -= (float)delta;
        }
        if (_jumpBufferTimer > 0 && _coyoteTimer > 0)
        {
            velocity.Y = JumpVelocity;

            _jumpBufferTimer = 0;
            _coyoteTimer = 0;
        }

        // Movement
        float direction = Input.GetAxis("ui_left", "ui_right");

        if (direction != 0)
		{
			velocity.X = direction * Speed;

			// Face movement direction
			if (direction < 0)
				_sprite.FlipH = true;
			else if (direction > 0)
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
        else if (Mathf.Abs(velocity.X) > 1)
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

    public async void Respawn()
    {
        if (_isRespawning)
            return;

        _isRespawning = true;

        GlobalPosition = _respawnPosition;
        Velocity = Vector2.Zero;

        _sprite.Play("respawn");

        await ToSignal(
            GetTree().CreateTimer(0.3),
            SceneTreeTimer.SignalName.Timeout
        );

        _isRespawning = false;
    }

}
