namespace libs;

public class Box : GameObject
{
    public Box() : base()
    {
        Type = GameObjectType.Player;
        CharRepresentation = '★';
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