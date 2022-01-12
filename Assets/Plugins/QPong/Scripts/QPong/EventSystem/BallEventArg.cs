public class BallEventArg:IEventArg
{
    public BallAction action{get;}
    public BallEventArg(BallAction action)
    {
        this.action = action;
    }
}

public enum BallAction
{
    none,
    bounce,
    dead,
    respawn
}