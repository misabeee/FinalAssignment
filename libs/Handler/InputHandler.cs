namespace libs;

public sealed class InputHandler
{
    private static InputHandler? _instance;
    private GameEngine engine;

    public static InputHandler Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new InputHandler();
            }
            return _instance;
        }
    }

    private InputHandler()
    {
        engine = GameEngine.Instance;
    }

    public void Handle(ConsoleKeyInfo keyInfo)
    {
        GameObject focusedObject = engine.GetFocusedObject();

        if (focusedObject != null)
        {
            switch (keyInfo.Key)
            {
                case ConsoleKey.UpArrow:
                    engine.SaveState();
                    focusedObject.Move(0, -1);
                    break;
                case ConsoleKey.DownArrow:
                    engine.SaveState();
                    focusedObject.Move(0, 1);
                    break;
                case ConsoleKey.LeftArrow:
                    engine.SaveState();
                    focusedObject.Move(-1, 0);
                    break;
                case ConsoleKey.RightArrow:
                    engine.SaveState();
                    focusedObject.Move(1, 0);
                    break;
                case ConsoleKey.Z:
                    engine.Undo();
                    break;
                case ConsoleKey.Enter:
                    engine.TryLoadNextLevel();
                    break;
                case ConsoleKey.S:
                    engine.SaveMap();
                    break;
                case ConsoleKey.L:
                    engine.loadSavedGame();
                    break;
                default:
                    break;
            }
           
        }
    }
}
