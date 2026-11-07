using Godot;
using System;

public partial class Sign : Area2D
{
	[Export]
	private PopupText _popupText;
	[Export]
	private String _displayText;
	private bool _active = false;

	private Vector2 _originalPosition;
	private Vector2 _originalScale;

	public override void _Ready()
	{
		_originalPosition = Position;
		_originalScale = Scale;

		BodyEntered += OnBodyEntered;
		BodyExited += OnBodyExited;

		_popupText.setText(_displayText);
	}
	
	
	private void OnBodyEntered(Node2D body)
	{
		if (body.IsInGroup("player") && !_active)
		{
			_active = true;
			ActivateSign();
		}
	}

	private void OnBodyExited(Node2D body)
	{
		if (body.IsInGroup("player") && _active)
		{
			_active = false;
			DeactivateSign();
			
		}
	}
	
	public async void ActivateSign()
	{
		var tween = CreateTween();

		tween.SetParallel();

		tween.TweenProperty(
			this,
			"scale",
			_originalScale * 1.15f,
			0.15f
		);

		tween.TweenProperty(
			this,
			"position",
			_originalPosition + Vector2.Up * 6,
			0.15f
		);

		await ToSignal(tween, Tween.SignalName.Finished);

		await ToSignal(
			GetTree().CreateTimer(0.3),
			SceneTreeTimer.SignalName.Timeout
		);
		if (!_active)
		return;

		_popupText.StartSequence();
	}

	//	ShowHint();
	public async void DeactivateSign()
	{
		_popupText.StopSequence();
		var tween = CreateTween();

		tween.SetParallel();

		tween.TweenProperty(
			this,
			"scale",
			_originalScale,
			0.15f
		);

		tween.TweenProperty(
			this,
			"position",
			_originalPosition,
			0.15f
		);

		await ToSignal(tween, Tween.SignalName.Finished);
	}
}
