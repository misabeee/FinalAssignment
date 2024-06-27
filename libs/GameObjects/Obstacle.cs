namespace libs;

public class Obstacle : GameObject
{
    public Obstacle() : base()
    {
        this.Type = GameObjectType.Obstacle;
        this.CharRepresentation = '█';
        this.Color = ConsoleColor.Cyan;
    }

    public override GameObject Clone()
    {
        return new Obstacle
        {
            PosX = this.PosX,
            PosY = this.PosY,
            Color = this.Color,
            CharRepresentation = this.CharRepresentation,
            Type = this.Type
        };
    }
}