using System.ComponentModel;
public interface IEventListener<T>
where T:IEventArg
{
    ITask OnNotice(T arg);
}