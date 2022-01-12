using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Security.Claims;
using UnityEngine;
using System.Linq;

public abstract class CustomEvent<T> : ScriptableObject
where T:IEventArg
{
    public abstract EventName eventName{get;}
    int register;
    List<IEventListener<T>> registers = new List<IEventListener<T>>();

    public ITask Notice(T arg)
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

    public bool RegisterListener(IEventListener<T> listener)
    {
        if (!registers.Contains(listener))
        {

            registers.Add(listener);
            return true;
        }
        return false;
    }

    public bool DisRegisterListener(IEventListener<T> listener)
    {
        if(registers.Contains(listener))
        {
            registers.Remove(listener);
            return true;
        }

        return false;
    }


}