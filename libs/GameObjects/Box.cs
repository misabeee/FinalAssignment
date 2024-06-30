namespace libs;

public class Box : GameObject
{
    private GameObjectFactory gameObjectFactory;

    public Map map = GameEngine.Instance.GetMap();
    public Box() : base()
    {
        this.gameObjectFactory = (GameEngine.Instance.gameObjectFactory as GameObjectFactory);
        Type = GameObjectType.Player;
        CharRepresentation = '○';

        if (GameEngine.Instance.currentLevel == 1)
        {
            CharRepresentation = '★';
        }
        else if (GameEngine.Instance.currentLevel == 2)
        {
            CharRepresentation = '♫';
        }

        Color = ConsoleColor.DarkGreen;
    }

    public override void Move(int dx, int dy)
    {
        this.SetPrevPosY(this.PosY);
        this.SetPrevPosX(this.PosX);
        this.PosX += dx;
        this.PosY += dy;
    }

    public override GameObject Clone()
    {
        return new Box
        {
            PosX = this.PosX,
            PosY = this.PosY,
            Color = this.Color,
            CharRepresentation = this.CharRepresentation,
            Type = this.Type
        };
    }
}