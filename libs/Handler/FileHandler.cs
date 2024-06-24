using System.Reflection.Metadata.Ecma335;

namespace libs;

using Newtonsoft.Json;

public static class FileHandler
{
    private static string filePath;
    private readonly static string envVar = "GAME_SETUP_PATH";
    private readonly static string envVar_Saved = "GAME_SAVE_PATH";

    static FileHandler()
    {
        Initialize(false);
    }

    private static void Initialize(bool loadSavedGameBool)
    {
        if (loadSavedGameBool) {
            if(Environment.GetEnvironmentVariable(envVar_Saved) != null){
            filePath = Environment.GetEnvironmentVariable(envVar_Saved);
            Console.WriteLine("Loading saved game");
            }
        } else {
            if(Environment.GetEnvironmentVariable(envVar) != null){
            filePath = Environment.GetEnvironmentVariable(envVar);
            Console.WriteLine("Loading new game");
            };   
        }
    }

    public static dynamic ReadJson(bool SavedGame)
    {
        Initialize(SavedGame);
        if (string.IsNullOrEmpty(filePath))
        {
            throw new InvalidOperationException("JSON file path not provided in environment variable");
        }

        try
        {
            string jsonContent = File.ReadAllText(filePath);
            dynamic jsonData = JsonConvert.DeserializeObject(jsonContent);
            return jsonData;
        }
        catch (FileNotFoundException)
        {
            throw new FileNotFoundException($"JSON file not found at path: {filePath}");
        }
        catch (Exception ex)
        {
            throw new Exception($"Error reading JSON file: {ex.Message}");
        }
    }

    public static void SaveJson(List<GameObject> gameObjects, int currentLevel)
    {
        File.WriteAllText("SavedGame.json", string.Empty);
        var jsonData = new
        {
            map = new
            {
                width = 28,
                height = 10
            },
            First = new
            {
                gameObjects = gameObjects.OrderBy(obj => obj.Type == 0 ? 1 : 0).ToList()
            },
            Level = currentLevel
        };

        try
        {
            string json = JsonConvert.SerializeObject(jsonData);
            File.WriteAllText("SavedGame.json", json);
            Console.WriteLine("wrote into json file");

        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
        }

    }
}
