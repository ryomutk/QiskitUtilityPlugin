public interface ITask
{
    bool ready { get; }
}

public interface ITask<T>:ITask
{
    T result{get;}
}