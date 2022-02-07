using System.ComponentModel;
public interface IEventListener
{
    ITask OnNotice(IEventArg arg);
}