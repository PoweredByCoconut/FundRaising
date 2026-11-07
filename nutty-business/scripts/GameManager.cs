using Godot;

public partial class GameManager : Node
{
    public int CurrentLevel = 1;

    public int HighestUnlockedLevel = 1;

    public int MaxUnlockedLevel = 12;

    private SceneTransition _canvas;

    public bool Paused = false;

    public static GameManager Instance { get; private set; }

    public override void _Ready()
    {
        Instance = this;
    }
    public void OnLevelLoaded(int levelNumber)
    {
        CurrentLevel = levelNumber;
        _canvas = GetNode<SceneTransition>("/root/Game/CanvasLayer");

        _canvas.FadeIn();
    }


    public void CompleteLevel()
    {
        bool lastLevel = CurrentLevel == MaxUnlockedLevel;
        _canvas = GetNode<SceneTransition>("/root/Game/CanvasLayer");
        if (CurrentLevel == HighestUnlockedLevel && !lastLevel) {
            HighestUnlockedLevel++;
            _canvas.UnlockLevel(HighestUnlockedLevel);
            GD.Print("Unlocked level " + HighestUnlockedLevel);
        }
        _canvas.ShowMenu(lastLevel);
    }
}