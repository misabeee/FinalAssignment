namespace libs;
public class Target : GameObject
{
    public Target() : base()
    {
        Type = GameObjectType.Target;
        CharRepresentation = 'o';
        Color = ConsoleColor.DarkGreen;
    }

    public override GameObject Clone()
    {
        return new Target
        {
            PosX = this.PosX,
            PosY = this.PosY,
            Color = this.Color,
            CharRepresentation = this.CharRepresentation,
            Type = this.Type
        };
    }
}