public class SmallTask<T>:ITask<T>
where T:class
{
    public T result{get;set;}
    public bool ready{get{return result != null;}}
}

public class SmallTask:ITask
{
    static SmallTask nullTask = new SmallTask(){ready = true};
    public bool ready{get;set;}
    public static SmallTask NullTask{get{return nullTask;}}

    public SmallTask()
    {
        ready = false;
    }
}