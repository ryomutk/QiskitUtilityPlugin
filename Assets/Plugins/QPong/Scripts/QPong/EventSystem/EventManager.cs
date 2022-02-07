using UnityEngine;
using System.Linq;

public class EventManager:Singleton<EventManager>
{
    [SerializeField] CustomEvent[] eventsInGame;

    public ITask Notice(EventName name,IEventArg arg)
    {
        return eventsInGame.First(x=>x.eventName == name).Notice(arg);
    }


    public bool Register(EventName name,IEventListener listener)
    {
        return eventsInGame.First(x=>x.eventName == name).RegisterListener(listener); 
    }


    public bool DisRegister(EventName name,IEventListener listener)
    {
        return eventsInGame.First(x=>x.eventName==name).DisRegisterListener(listener);
    }
}