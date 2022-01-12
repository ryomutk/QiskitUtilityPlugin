public class ScoreEventArg:IEventArg
{
    public int score{get;}
    public Actor subject{get;} 
}

public enum Actor
{
    player,
    enemy
}