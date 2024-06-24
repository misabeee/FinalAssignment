namespace libs;

// public class NPC : GameObject {
//     public NPC () : base() {
//         this.Type = GameObjectType.NPC;
//         this.CharRepresentation = '☺';
//         this.Color = ConsoleColor.Yellow;
//     }
// }

public class NPC : GameObject {
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
}