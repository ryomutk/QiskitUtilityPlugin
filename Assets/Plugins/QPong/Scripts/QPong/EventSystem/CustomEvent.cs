using System.Collections.Generic;
using UnityEngine;
using System.Linq;


[CreateAssetMenu(menuName ="CustomEvent")]
public class CustomEvent : ScriptableObject
{
    public EventName eventName{get{return _eventName;}}
    [SerializeField] EventName _eventName;
    int register;
    List<IEventListener> registers = new List<IEventListener>();

    public ITask Notice(IEventArg arg)
    {
        List<ITask> listenings = null;
        foreach(var register in registers)
        {
            var task = register.OnNotice(arg);
            if(!task.ready)
            {
                if(listenings==null)
                {
                    listenings=new List<ITask>();
                }
                listenings.Add(task);
            }
        }

        if(listenings!=null)
        {
            return new JobTask(()=>listenings.All(x=>x.ready));
        }

        return SmallTask.NullTask;
    }

    public bool RegisterListener(IEventListener listener)
    {
        if (!registers.Contains(listener))
        {

            registers.Add(listener);
            return true;
        }
        return false;
    }

    public bool DisRegisterListener(IEventListener listener)
    {
        if(registers.Contains(listener))
        {
            registers.Remove(listener);
            return true;
        }

        return false;
    }


}