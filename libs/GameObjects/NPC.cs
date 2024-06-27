namespace libs;

public class NPC : GameObject
{
    public string Identifier { get; private set; }

    public NPC(int x, int y, string identifier) : base()
    {
        PosX = x;
        PosY = y;
        Identifier = identifier;
        Type = GameObjectType.NPC;
        CharRepresentation = '☺';
        Color = ConsoleColor.Yellow;
    }

    public override GameObject Clone()
    {
        return new NPC(PosX, PosY, Identifier)
        {
            Color = this.Color,
            CharRepresentation = this.CharRepresentation,
            Type = this.Type
        };
    }
}