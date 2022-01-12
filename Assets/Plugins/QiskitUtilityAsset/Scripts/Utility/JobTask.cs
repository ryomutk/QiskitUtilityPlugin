using System;

public class JobTask:ITask
{
    public bool ready{get{return endCondition();}}
    Func<bool> endCondition;
    public JobTask(Func<bool> endCondition)
    {
        this.endCondition = endCondition;
    }
}