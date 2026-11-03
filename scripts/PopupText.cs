using Godot;
using System;

public partial class PopupText : RichTextLabel
{
	[Export]
	private Panel _popupPanel;
	private bool _typing;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Modulate = new Color(1, 1, 1, 0);
	}

	
	public async void StartSequence(string text)
	{
		Text = text;
		_popupPanel.Size = Size + new Vector2(0, 5);
		_popupPanel.Visible = true;
		_popupPanel.Modulate = new Color(1, 1, 1, 0);
		var tween = CreateTween();

		tween.TweenProperty(
			_popupPanel,
			"modulate:a",
			1.0f,
			0.1f
		);
		
		await ToSignal(tween, Tween.SignalName.Finished);
		Modulate = Colors.White;
		
		VisibleCharacters = 0;
		
		Visible = true;

		_typing = true;

		while (_typing && VisibleCharacters < Text.Length)
		{
			VisibleCharacters++;

			await ToSignal(
				GetTree().CreateTimer(0.03),
				SceneTreeTimer.SignalName.Timeout
			);
		}
	}
		
	public async void StopSequence()
	{
		_typing = false;
		var tween = CreateTween();

		tween.TweenProperty(
			this,
			"modulate:a",
			0.0f,
			0.2f
		);
		
		await ToSignal(tween, Tween.SignalName.Finished);

		this.Visible = false;
		
		tween = CreateTween();
		
		tween.TweenProperty(
			_popupPanel,
			"modulate:a",
			0.0f,
			0.2f
		);
		
		await ToSignal(tween, Tween.SignalName.Finished);
		
		_popupPanel.Visible = false;
	}
	
}
