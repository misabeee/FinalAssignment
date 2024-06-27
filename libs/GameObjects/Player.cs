namespace libs;

public class Player : GameObject
{
    private static Player instance = null;
    public Map map = GameEngine.Instance.GetMap();

    private Player() : base()
    {
        Type = GameObjectType.Player;
        CharRepresentation = '☻';
        Color = ConsoleColor.DarkYellow;
    }

    public static Player Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new Player();
            }
            return instance;
        }

        set { }
    }

    public override void Move(int dx, int dy)
    {
        int goToX = PosX + dx;
        int goToY = PosY + dy;

        GameObject? nextObject = map.Get(goToY, goToX);

        if (nextObject != null && nextObject.Type == GameObjectType.NPC)
        {
            string npcIdentifier = $"NPC_{goToX}_{goToY}"; // Get Position of Player
            string interactionText = GameEngine.Instance.GetInteractionText(npcIdentifier);

            Console.WriteLine("");
            Console.WriteLine("----------------------------");
            Console.WriteLine(interactionText);
            Console.WriteLine("----------------------------");

            while (true)
            {
                Console.WriteLine("Press Enter to continue...");
                if (Console.ReadKey(true).Key == ConsoleKey.Enter)
                {
                    return;
                }
            }
        }

        if (nextObject.Type == GameObjectType.Obstacle ||
            nextObject.Type == GameObjectType.Target)
        {
            return;
        }

        if (nextObject.Type == GameObjectType.Box)
        {
            GameObject? nextNextObject = map.Get(goToY + dy, goToX + dx);

            if (nextNextObject.Type == GameObjectType.Obstacle ||
                nextNextObject.Type == GameObjectType.Box)
            {
                return;
            }

            nextObject.Move(dx, dy);
            if (nextNextObject.Type == GameObjectType.Target)
            {
                nextObject.Color = ConsoleColor.Red;
            }
            else
            {
                nextObject.Color = ConsoleColor.Yellow;
            }
        }

        this.SetPrevPosY(this.PosY);
        this.SetPrevPosX(this.PosX);
        this.PosX += dx;
        this.PosY += dy;
    }

    private void DisplayInteraction(string interactionText)
    {
        Console.WriteLine("");
        Console.WriteLine("----------------------------");
        Console.WriteLine(interactionText);
        Console.WriteLine("----------------------------");

        while (true)
        {
            Console.WriteLine("Press Enter to continue...");
            if (Console.ReadKey(true).Key == ConsoleKey.Enter)
            {
                return;
            }
        }
    }

    public override GameObject Clone()
    {
        return new Player
        {
            PosX = this.PosX,
            PosY = this.PosY,
            Color = this.Color,
            CharRepresentation = this.CharRepresentation,
            Type = this.Type
        };
    }
}
