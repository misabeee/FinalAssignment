namespace libs;

public class Floor : GameObject
{
    public Floor() : base()
    {
        Type = GameObjectType.Floor;
        CharRepresentation = '.';
    }

    public override GameObject Clone()
    {
        return new Floor
        {
            PosX = this.PosX,
            PosY = this.PosY,
            Color = this.Color,
            CharRepresentation = this.CharRepresentation,
            Type = this.Type
        };
    }
}