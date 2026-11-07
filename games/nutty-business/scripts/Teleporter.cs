
using Godot;

public partial class Teleporter : Area2D
{
    [ExportGroup("Objects")]
    [Export] public Node2D Player { get; set; }
    [Export] public RigidBody2D Acorn { get; set; }

    [ExportGroup("Destinations")]
    [Export] public Marker2D PlayerDestination { get; set; }
    [Export] public Marker2D AcornDestination { get; set; }

    [ExportGroup("Settings")]
    [Export] public bool TeleportPlayer { get; set; } = true;
    [Export] public bool TeleportAcorn { get; set; } = true;



    public override void _Ready()
    {
        BodyEntered += OnBodyEntered;
    }

    private void OnBodyEntered(Node2D body)
    {
        if (body.IsInGroup("player") && TeleportPlayer)
        {
            TeleportPlayerObject();
        }
        if (body.IsInGroup("acorn") && TeleportAcorn)
        {
            TeleportAcornObject();
        }
    }



    public void TeleportPlayerObject()
    {
        if (Player == null || PlayerDestination == null)
        {
            GD.PushWarning("Missing player or player destination.");
            return;
        }

        Player.GlobalPosition = PlayerDestination.GlobalPosition;

        if (Player is CharacterBody2D playerBody)
        {
            playerBody.Velocity = Vector2.Zero;
        }
    }

    public void TeleportAcornObject()
    {
        if (Acorn == null || AcornDestination == null)
        {
            GD.PushWarning("Missing acorn or acorn destination.");
            return;
        }

        // Defer the operation until physics queries finish.
        CallDeferred(nameof(DeferredTeleportAcorn));
    }

    private void DeferredTeleportAcorn()
    {
        if (!IsInstanceValid(Acorn) || !IsInstanceValid(AcornDestination))
            return;

        
        // Freeze physics while repositioning.
        Acorn.Freeze = true;


        GD.Print("Teleporting acorn to " + AcornDestination.GlobalPosition);

        Transform2D transform = Acorn.GlobalTransform;
        transform.Origin = AcornDestination.GlobalPosition;

        Acorn.GlobalTransform = transform;

        // Clear existing momentum.
        Acorn.LinearVelocity = Vector2.Zero;
        Acorn.AngularVelocity = 0.0f;

        // Clear any accumulated forces.
        Acorn.Sleeping = true;

        // Restore physics.
        Acorn.Freeze = false;
        GD.Print("Teleported acorn to " + GlobalPosition);
    }
}
