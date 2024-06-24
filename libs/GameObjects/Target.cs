namespace libs;

public class Target : GameObject {

    public Target () : base(){
        Type = GameObjectType.Player;
        CharRepresentation = 'o';
        Color = ConsoleColor.DarkGreen;
    }
}