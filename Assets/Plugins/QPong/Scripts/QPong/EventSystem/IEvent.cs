using System;
public interface IEvent
{
    ITask Notice(IEventArg arg);
}