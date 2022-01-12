public class ScoreEvent:CustomEvent<ScoreEventArg>
{
    public override EventName eventName{get{return EventName.ScoreEvent;}}
}