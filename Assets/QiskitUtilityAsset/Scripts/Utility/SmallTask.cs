public class SmallTask<T>:ITask<T>
where T:class
{
    public T result{get;set;}
    public bool ready{get{return result != null;}}
}

public class SmallTask:ITask
{
    public bool ready{get;set;}

    public SmallTask()
    {
        ready = false;
    }
}