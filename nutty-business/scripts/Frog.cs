using Godot;
using System;


using Godot;
using System;
using System.Collections.Generic;

public partial class Frog : AnimatableBody2D
{
    private enum FrogState
    {
        Waiting,
        Hopping,
        Pausing,
        WaitingForRider
    }

    [ExportGroup("Path")]
    [Export] public Marker2D StartBank { get; set; }
    [Export] public Marker2D FarBank { get; set; }
    [Export] public Godot.Collections.Array<Marker2D> LilyPads { get; set; } = new();

    [ExportGroup("Movement")]
    [Export] public float HopDuration { get; set; } = 0.4f;
    [Export] public float HopHeight { get; set; } = 45.0f;
    [Export] public float BankWaitTime { get; set; } = 3.0f;
    [Export] public float HopPauseDuration { get; set; } = 0.2f;
    [Export] public float DismountDelay { get; set; } = 0.5f;

    [ExportGroup("Detection")]
    [Export] public Area2D HeadArea { get; set; }

    private readonly List<Vector2> _route = new();

    private FrogState _state = FrogState.Waiting;

    private int _routeIndex = 0;
    private int _direction = 1;

    private float _hopTimer;
    private float _waitTimer;
    private float _pauseTimer;
    private float _riderWaitTimer;

    private bool _dismountPending = false;

    private AnimatedSprite2D _sprite;
    private Vector2 _hopStart;
    private Vector2 _hopEnd;

    private bool _riderOnHead = false;
    private bool _forcedTrip = false;
    private bool _returningToOrigin = false;

    private int _tripOriginIndex;
    private int _tripDestinationIndex;

    public override void _Ready()
    {
        // Build the route from the starting bank,
        // through the lily pads, to the opposite bank.

        _route.Add(StartBank.GlobalPosition);

        foreach (Marker2D pad in LilyPads)
        {
            _route.Add(pad.GlobalPosition);
        }

        _route.Add(FarBank.GlobalPosition);

        _sprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
        _sprite.Play("idle");

        // Place the frog at its starting position.
        GlobalPosition = _route[0];

        _waitTimer = BankWaitTime;

        // Connect head detection.
        HeadArea.BodyEntered += OnHeadBodyEntered;
        HeadArea.BodyExited += OnHeadBodyExited;
    }

    public override void _PhysicsProcess(double delta)
    {
        switch (_state)
        {
            case FrogState.Waiting:
                UpdateWaiting(delta);
                break;

            case FrogState.Hopping:
                UpdateHopping(delta);
                break;

            case FrogState.Pausing:
                UpdatePausing(delta);
                break;

            case FrogState.WaitingForRider:
                UpdateRiderWaiting(delta);
                break;
        }
    }

    // ------------------------------------------------
    // NORMAL WAITING
    // ------------------------------------------------

    private void UpdateWaiting(double delta)
    {
        _waitTimer -= (float)delta;

        if (_waitTimer <= 0)
        {
            // Travel toward the opposite bank.
            _direction = _routeIndex == 0 ? 1 : -1;

            BeginHop();
        }
    }

    private void UpdatePausing(double delta)
    {
        _pauseTimer -= (float)delta;

        if (_pauseTimer <= 0)
        {
            BeginHop();
        }
    }

    // ------------------------------------------------
    // HOPPING
    // ------------------------------------------------

    private void BeginHop()
    {
        int nextIndex = _routeIndex + _direction;

        if (nextIndex < 0 || nextIndex >= _route.Count)
            return;

        _hopStart = GlobalPosition;
        _hopEnd = _route[nextIndex];

        // Face the direction of travel.
        _sprite.FlipH = _hopEnd.X >  _hopStart.X;

        _hopTimer = 0;

        _sprite.Play("jump_up");

        _state = FrogState.Hopping;
    }

    private void UpdateHopping(double delta)
    {
        _hopTimer += (float)delta;

        float t = Mathf.Clamp(
            _hopTimer / HopDuration, 0.0f, 1.0f
        );

        // Horizontal movement
        Vector2 position = _hopStart.Lerp(_hopEnd, t);

        // Parabolic jumping arc
        float arc = Mathf.Sin(t * Mathf.Pi) * HopHeight;

        position.Y -= arc;

        GlobalPosition = position;

        // Switch animation halfway through the jump.
        if (t < 0.5f)
        {
            if (_sprite.Animation != "jump_up")
            {
                _sprite.Play("jump_up");
            }

            SetFallingLayer(false);
        }
        else
        {
            if (_sprite.Animation != "jump_down")
            {
                _sprite.Play("jump_down");
            }

            SetFallingLayer(true);
        }

        // Finish hop
        if (t >= 1.0f)
        {
            GlobalPosition = _hopEnd;

            _routeIndex += _direction;

            HandleReachedWaypoint();
        }
    }

    // ------------------------------------------------
    // REACHING A WAYPOINT OR BANK
    // ------------------------------------------------

    private void HandleReachedWaypoint()
    {
        bool reachedBank =
            _routeIndex == 0 ||
            _routeIndex == _route.Count - 1;

        if (!reachedBank)
        {
            // Briefly pause on each lily pad.
            _state = FrogState.Pausing;
            _pauseTimer = HopPauseDuration;

            _sprite.Play("idle");

            return;
        }

        if (_forcedTrip)
        {
            if (_returningToOrigin)
            {
                FinishForcedTrip();
            }
            else
            {
                // Wait for the player to dismount.
                _state = FrogState.WaitingForRider;

                _riderWaitTimer = BankWaitTime * 2.0f;

                _sprite.Play("idle");
            }

            return;
        }
        SetFallingLayer(false);
        // Normal behavior: wait at the bank.
        _state = FrogState.Waiting;
        _waitTimer = BankWaitTime;
        _sprite.Play("idle");
    }

    // ------------------------------------------------
    // PLAYER DETECTION
    // ------------------------------------------------

    private void OnHeadBodyEntered(Node2D body)
    {
        if (!body.IsInGroup("player"))
            return;

        _riderOnHead = true;

        // Only the first boarding triggers a special trip.
        if (!_forcedTrip)
        {
            BeginForcedTrip();
        }
    }

    private void OnHeadBodyExited(Node2D body)
    {
        if (!body.IsInGroup("player"))
            return;

        _riderOnHead = false;

        // Only start the delayed return if we're
        // waiting at the opposite bank.
        if (_state == FrogState.WaitingForRider)
        {
            _dismountPending = true;
            _riderWaitTimer = DismountDelay;
        }
    }

    // ------------------------------------------------
    // PLAYER-TRIGGERED CROSSING
    // ------------------------------------------------

    private void BeginForcedTrip()
    {
        _forcedTrip = true;
        _returningToOrigin = false;

        // Determine which bank the frog is currently
        // closest to. That will be its return destination.
        float distanceToStart =
            GlobalPosition.DistanceTo(_route[0]);

        float distanceToFar =
            GlobalPosition.DistanceTo(_route[_route.Count - 1]);

        if (distanceToStart <= distanceToFar)
        {
            _tripOriginIndex = 0;
            _tripDestinationIndex = _route.Count - 1;
        }
        else
        {
            _tripOriginIndex = _route.Count - 1;
            _tripDestinationIndex = 0;
        }

        // Find the nearest waypoint so the frog can
        // redirect itself if the player boards mid-hop.
        _routeIndex = FindNearestWaypoint();

        _direction = Math.Sign(
            _tripDestinationIndex - _routeIndex
        );

        // Start crossing immediately.
        if (_routeIndex == _tripDestinationIndex)
        {
            HandleReachedWaypoint();
        }
        else
        {
            BeginHop();
        }
    }

    private void UpdateRiderWaiting(double delta)
    {
        if (_riderOnHead)
        {
            _dismountPending = false;
            _riderWaitTimer = BankWaitTime * 2.0f;
            return;
        }

        _riderWaitTimer -= (float)delta;

        if (_riderWaitTimer <= 0)
        {
            _dismountPending = false;
            BeginReturnTrip();
        }
    }

    // ------------------------------------------------
    // RETURNING AFTER THE PLAYER DISMOUNTS
    // ------------------------------------------------

    private void BeginReturnTrip()
    {
        _returningToOrigin = true;

        _direction = Math.Sign(
            _tripOriginIndex - _routeIndex
        );

        if (_routeIndex == _tripOriginIndex)
        {
            FinishForcedTrip();
            return;
        }

        // Start hopping back immediately.
        BeginHop();
    }

    private void FinishForcedTrip()
    {
        _forcedTrip = false;
        _returningToOrigin = false;

        _state = FrogState.Waiting;
        _waitTimer = BankWaitTime;
    }

    // ------------------------------------------------
    // HELPERS
    // ------------------------------------------------

    private int FindNearestWaypoint()
    {
        int nearest = 0;
        float shortestDistance = float.MaxValue;

        for (int i = 0; i < _route.Count; i++)
        {
            float distance =
                GlobalPosition.DistanceTo(_route[i]);

            if (distance < shortestDistance)
            {
                shortestDistance = distance;
                nearest = i;
            }
        }

        return nearest;
    }

    private void SetFallingLayer(bool falling)
    {
        // Move the frog behind the player while falling.
        ZIndex = falling ? -1 : 1;
    }
}
