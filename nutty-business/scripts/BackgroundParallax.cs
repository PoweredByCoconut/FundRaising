using Godot;
using System;

public partial class BackgroundParallax : Node2D
{
	[Export(PropertyHint.Range, "0,1")]
	public float ParallaxStrength { get; set; } = 1.0f;

	private Vector2 _startPosition;

	public override void _Ready()
	{
		_startPosition = GlobalPosition;
	}

	public override void _Process(double delta)
	{
		var camera = GetViewport().GetCamera2D();
		Vector2 cameraScreenCenter = camera.GetScreenCenterPosition();
		if (camera == null)
			return;

		GlobalPosition = new Vector2(_startPosition.X + (_startPosition.X - cameraScreenCenter.X) * -ParallaxStrength,
			_startPosition.Y
		);
	}
}
