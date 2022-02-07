using System.Security.Cryptography.X509Certificates;
public class GameEventArg:IEventArg
{
    public GameState state{get;}
    public GameEventArg(GameState gameState)
    {
        this.state = gameState;
    }
}
