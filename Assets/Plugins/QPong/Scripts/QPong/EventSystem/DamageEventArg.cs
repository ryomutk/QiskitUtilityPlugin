public class DamageEventArg:IEventArg
{
    public DamageEventArg(int damage,Actor subject)
    {
        this.damage=damage;
        this.subject=subject;
    }
    public int damage{get;}
    public Actor subject{get;} 
}

public enum Actor
{
    player,
    enemy
}