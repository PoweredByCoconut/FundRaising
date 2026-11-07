
using Godot;
using System;

public partial class Catapult : Node2D
{
    [ExportGroup("Detection")]
    [Export] public Area2D AcornZone { get; set; }
    [Export] public Area2D RockZone { get; set; }

    [ExportGroup("Objects")]
    [Export] public Marker2D AcornLaunchPoint { get; set; }
    [Export] public Marker2D AcornTarget { get; set; }
    [Export] public Marker2D RockRespawnPoint { get; set; }
    [Export] public Node2D LogPivot { get; set; }

    [ExportGroup("Launch Settings")]
    [Export] public float FlightTime { get; set; } = 0.8f;
    [Export] public float MaxObjectSpeed { get; set; } = 10.0f;

    [ExportGroup("Catapult Animation")]
    [Export] public float RotationAmount { get; set; } = 20.0f;
    [Export] public float AnimationDuration { get; set; } = 0.4f;

    private RigidBody2D _acorn;
    private RigidBody2D _rock;

    private bool _launched = false;
    private bool _animating = false;

    private Vector2 _acornLocalPosition;
    private Vector2 _rockLocalPosition;

    private float _acornLocalRotation;
    private float _rockLocalRotation;

    private float _originalLogRotation;

    public override void _Ready()
    {
        AcornZone.BodyEntered += OnAcornEntered;
        AcornZone.BodyExited += OnAcornExited;

        RockZone.BodyEntered += OnRockEntered;
        RockZone.BodyExited += OnRockExited;
    }

    private void OnAcornEntered(Node2D body)
    {
        if (body.IsInGroup("acorn") && body is RigidBody2D)
        {
            _acorn = (RigidBody2D)body;
            GD.Print("Acorn Entered");
        }
    }

    private void OnAcornExited(Node2D body)
    {
        if (body == _acorn)
        {
            _acorn = null;
            GD.Print("Acorn Exited");
        }
    }

    private void OnRockEntered(Node2D body)
    {
        if (body.IsInGroup("rock") && body is RigidBody2D)
        {
            _rock = (RigidBody2D)body;
            GD.Print("Rock Entered");
        }
    }

    private void OnRockExited(Node2D body)
    {
        if (body == _rock && !_animating)
        {
            _rock = null;
            GD.Print("Rock Exited");
        }
    }

    public override void _PhysicsProcess(double delta)
    {
        // The rock alone activates the catapult.
        if (_launched || _animating || _rock == null)
            return;

        _launched = true;
        _animating = true;

        CallDeferred(nameof(StartCatapult));
    }

    private void StartCatapult()
    {
        if (!IsInstanceValid(_rock))
        {
            ResetCatapult();
            return;
        }

        // Freeze the rock.
        _rock.Freeze = true;
        _rock.Sleeping = true;

        _rock.LinearVelocity = Vector2.Zero;
        _rock.AngularVelocity = 0;

        // Save the rock's position relative to the pivot.
        _rockLocalPosition = LogPivot.ToLocal(_rock.GlobalPosition);
        _rockLocalRotation = _rock.GlobalRotation - LogPivot.GlobalRotation;

        // Only freeze and track the acorn if one is present.
        if (IsInstanceValid(_acorn))
        {
            _acorn.Freeze = true;
            _acorn.Sleeping = true;

            _acorn.LinearVelocity = Vector2.Zero;
            _acorn.AngularVelocity = 0;

            _acornLocalPosition = LogPivot.ToLocal(_acorn.GlobalPosition);
            _acornLocalRotation = _acorn.GlobalRotation - LogPivot.GlobalRotation;
        }

        _originalLogRotation = LogPivot.Rotation;

        // Animate the log.
        Tween tween = CreateTween();

        float targetRotation = -Mathf.DegToRad(RotationAmount);

        tween.TweenMethod(
            Callable.From<float>(UpdateCatapultPose),
            _originalLogRotation,
            _originalLogRotation + targetRotation,
            AnimationDuration
        ).SetTrans(Tween.TransitionType.Linear)
         .SetEase(Tween.EaseType.InOut);

        tween.TweenCallback(Callable.From(OnCatapultAnimationFinished));
    }

    private void UpdateCatapultPose(float rotation)
    {
        LogPivot.Rotation = rotation;

        // Always move the rock with the log.
        if (IsInstanceValid(_rock))
        {
            _rock.GlobalPosition = LogPivot.ToGlobal(_rockLocalPosition);
            _rock.GlobalRotation = LogPivot.GlobalRotation + _rockLocalRotation;
        }

        // Only move the acorn if one is present.
        if (IsInstanceValid(_acorn))
        {
            _acorn.GlobalPosition = LogPivot.ToGlobal(_acornLocalPosition);
            _acorn.GlobalRotation = LogPivot.GlobalRotation + _acornLocalRotation;
        }
    }

    private async void OnCatapultAnimationFinished()
    {
        

        // Launch the acorn if present.
        if (IsInstanceValid(_acorn))
        {
            LaunchAcorn();
        }

        // Return the log to its original position.
        await ToSignal(GetTree().CreateTimer(2), SceneTreeTimer.SignalName.Timeout);
        ReturnLogToRest();
    }

    private void LaunchAcorn()
    {
        if (AcornLaunchPoint == null || AcornTarget == null)
            return;

        // Position the acorn at the launch point.
        _acorn.Freeze = true;
        _acorn.GlobalPosition = AcornLaunchPoint.GlobalPosition;

        // Calculate launch velocity.
        float gravity = (float)ProjectSettings.GetSetting(
            "physics/2d/default_gravity"
        );

        Vector2 gravityVector = Vector2.Down * gravity;

        Vector2 displacement =
            AcornTarget.GlobalPosition - AcornLaunchPoint.GlobalPosition;

        Vector2 launchVelocity =
            (displacement - 0.5f * gravityVector *
            FlightTime * FlightTime) / FlightTime;

        // Release and launch.
        _acorn.Freeze = false;
        _acorn.Sleeping = false;

        _acorn.LinearVelocity = launchVelocity;
        _acorn.AngularVelocity = 0;

        GD.Print("Catapult launched the acorn!");
    }

    private void ResetCatapult()
    {
        _animating = false;
        _launched = false;

        // The rock is now back at its spawn point.
        // Wait for it to enter RockZone again.
        _rock = null;
    }

    private void ReturnLogToRest()
    {
        Tween tween = CreateTween();

        // Respawn the rock.
        if (IsInstanceValid(_rock))
        {
            _rock.Freeze = true;

            _rock.GlobalPosition = RockRespawnPoint.GlobalPosition;
            _rock.GlobalRotation = RockRespawnPoint.GlobalRotation;

            _rock.LinearVelocity = Vector2.Zero;
            _rock.AngularVelocity = 0;

            _rock.Freeze = false;
            _rock.Sleeping = true;
        }

        tween.TweenProperty(
            LogPivot,
            "rotation",
            _originalLogRotation,
            AnimationDuration
        ).SetTrans(Tween.TransitionType.Cubic)
         .SetEase(Tween.EaseType.Out);

        tween.TweenCallback(Callable.From(() =>
        {
            _animating = false;
            _launched = false;
            _rock = null;

            GD.Print("Catapult reset.");
        }));


    }
}
