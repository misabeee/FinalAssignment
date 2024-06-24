using System.Reflection.Metadata.Ecma335;
using System.Text.Json.Nodes;

namespace libs;

using System.Security.Cryptography;
using Newtonsoft.Json;

public sealed class GameEngine
{
    private static GameEngine? _instance;
    public IGameObjectFactory gameObjectFactory;

    // Current level when we start is 1
    public int currentLevel = 1;

    public static GameEngine Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new GameEngine();
            }
            return _instance;
        }
    }

    private GameEngine()
    {
        //INIT PROPS HERE IF NEEDED
        gameObjectFactory = new GameObjectFactory();
    }

    private GameObject? _focusedObject;

    private Map map = new Map();

    private List<GameObject> gameObjects = new List<GameObject>();


    public Map GetMap()
    {
        return map;
    }

    public GameObject GetFocusedObject()
    {
        return _focusedObject;
    }

    public bool endGame()
    {
        var Targets = gameObjects.OfType<Target>();
        var Boxes = gameObjects.OfType<Box>();
        int Hits = 0;

        foreach (var Target in Targets)
        {
            foreach (var Box in Boxes)
            {
                if (Box.PosX == Target.PosX && Box.PosY == Target.PosY) {
                    Hits++;
                }   
            }
        }
        return (Hits != Targets.Count());
    }

    public void Setup(bool SavedGame)
    {
        // Added for proper display of game characters
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            dynamic gameData = FileHandler.ReadJson(SavedGame);

            // First level gets loaded at beginning
            var Level = gameData.First;
            if (!SavedGame)
            {
                switch (currentLevel)
                {
                    case (1):
                        Level = gameData.First;
                        AddGameObject(new NPC(6, 3, "Level1_NPC"));
                        break;
                    case (2):
                        Level = gameData.Second;
                        AddGameObject(new NPC(4, 3, "Level2_NPC"));
                        break;
                    default:
                        return;
                }
            }

        gameObjects.Clear(); // Clear all objects before rendering new level
        map.MapWidth = gameData.map.width;
        map.MapHeight = gameData.map.height;

        int? level_from_json = gameData.Level;
        if (level_from_json.HasValue)
        {
            currentLevel = level_from_json.Value;
        }

        foreach (var gameObject in Level.gameObjects)
        {
            AddGameObject(CreateGameObject(gameObject));
        }

        _focusedObject = gameObjects.OfType<Player>().First();

    }

    public void Render()
    {
        //Clean the map
        Console.Clear();
        Console.WriteLine("----------------------------");
        Console.WriteLine("----------------------------");
        Console.WriteLine($"          Level: {currentLevel}");
        Console.WriteLine("----------------------------");
        Console.WriteLine("----------------------------");
        Console.WriteLine("");
        map.Initialize();

        PlaceGameObjects();

        //Render the map
        for (int i = 0; i < map.MapHeight; i++)
        {
            for (int j = 0; j < map.MapWidth; j++)
            {
                DrawObject(map.Get(i, j));
            }
            Console.WriteLine();
        }

        // Checks if the current level is anything but the last
        if (endGame() == false && currentLevel != 2) 
        {
            Console.WriteLine("");
            Console.WriteLine("You completed the level!");
            Console.WriteLine("Press Enter to get to the next level!");
        }
    }

    public void SaveMap(){
    List<GameObject> savedMap = new List<GameObject>();

        for (int i = 0; i < map.MapHeight; i++)
        {
            for (int j = 0; j < map.MapWidth; j++)
            {
                GameObject currentObject = map.Get(i, j);
                if (currentObject != null && currentObject.Type != GameObjectType.Floor)
                {
                    savedMap.Add(currentObject);
                }
            }
        }
        FileHandler.SaveJson(savedMap, currentLevel);
    }

    public void loadSavedGame(){
        Setup(true);
    }

    public void Undo()
    {
        // Only step back possible
        // TO-DO: fix bug where the box will still go to prev position even if it didnt move in current move
        foreach (var gameObject in gameObjects)
        {
            gameObject.PosX = gameObject.GetPrevPosX();
            gameObject.PosY = gameObject.GetPrevPosY();
        }
    }

    // Method to create GameObject using the factory from clients
    public GameObject CreateGameObject(dynamic obj)
    {
        return gameObjectFactory.CreateGameObject(obj);
    }

    public void AddGameObject(GameObject gameObject)
    {
        gameObjects.Add(gameObject);
    }

    private void PlaceGameObjects()
    {
        gameObjects.ForEach(delegate (GameObject obj)
        {
            map.Set(obj);
        });
    }

    private void DrawObject(GameObject gameObject)
    {
        Console.ResetColor();

        if (gameObject != null)
        {
            Console.ForegroundColor = gameObject.Color;
            Console.Write(gameObject.CharRepresentation);
        }
        else
        {
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.Write(' ');
        }
    }

    // Increase currentLevel if Level is finished and next Level exists  
    public void TryLoadNextLevel()
    {
        if ((endGame() == false) && (currentLevel < 2))
        {
            currentLevel++;
            Setup(false);
        }
    }

    // Main Menu
    public void MainMenu()
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("----------------------------");
            Console.WriteLine("----------------------------");
            Console.WriteLine("    Welcome to the Game!");
            Console.WriteLine("----------------------------");
            Console.WriteLine("----------------------------");
            Console.WriteLine("");
            Console.WriteLine("    1. Start New Game");
            Console.WriteLine("    2. Load Saved Game");
            Console.WriteLine("    3. Instructions");
            Console.WriteLine("    4. Exit");
            Console.WriteLine("");
            Console.WriteLine("----------------------------");
            Console.WriteLine("");
            Console.Write("    Select an option: ");

            var choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    currentLevel = 1;
                    Setup(false);
                    return;
                case "2":
                    loadSavedGame();
                    return;
                case "3":
                    DisplayInstructions();
                    break;
                case "4":
                    Environment.Exit(0);
                    break;
                default:
                    Console.WriteLine("Invalid option, please try again.");
                    break;
            }
        }
    }

    // Game Instructions
    private void DisplayInstructions()
    {
        Console.Clear();
        Console.WriteLine("----------------------------");
        Console.WriteLine("----------------------------");
        Console.WriteLine("        How to Play:");
        Console.WriteLine("----------------------------");
        Console.WriteLine("----------------------------");
        Console.WriteLine("");
        Console.WriteLine("1. Use arrow keys to move.");
        Console.WriteLine("2. Push all the boxes onto the target spots to complete the level.");
        Console.WriteLine("3. Press 'Z' to undo your last move.");
        Console.WriteLine("4. Press 'S' to save the game.");
        Console.WriteLine("5. Press 'L' to load a saved game.");
        Console.WriteLine("");
        Console.WriteLine("----------------------------");
        Console.WriteLine("");
        Console.WriteLine("Press Enter to return to the main menu.");
        Console.ReadLine();
    }

    public string GetInteractionText(string identifier)
    {
        if (identifier == null)
        {
            Console.WriteLine("Error: Identifier cannot be null.");
            return "Error: Identifier cannot be null.";
        }

        string jsonData;
        try
        {
            jsonData = File.ReadAllText("../Interaction.json");
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error reading interaction file: " + ex.Message);
            return "Error reading interaction text.";
        }

        Dictionary<string, string> interactionData;
        try
        {
            interactionData = JsonConvert.DeserializeObject<Dictionary<string, string>>(jsonData);
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error deserializing interaction file: " + ex.Message);
            return "Error deserializing interaction text.";
        }

        if (interactionData.ContainsKey(identifier))
        {
            return interactionData[identifier];
        }
        else
        {
            Console.WriteLine($"Error: No interaction text found for identifier '{identifier}'.");
            return $"Error: No interaction text found for identifier '{identifier}'.";
        }
    }
}