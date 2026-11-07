using Godot;

public partial class PauseMenu : CanvasLayer
{
    public override void _Input(InputEvent @event)
    {
        if (@event.IsActionPressed("Pause") && !@event.IsEcho())
        {
            BackToGame();
        }
    }

    public void BackToGame()
    {
        Visible = !Visible;
        GetTree().Paused = Visible;
    }

    public void GoToMenu()
    {
        GetTree().Paused = false;
        GetTree().ChangeSceneToFile("res://menu/MainMenu.tscn");
    }
}
