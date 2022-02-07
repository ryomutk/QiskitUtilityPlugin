using System.Xml.Schema;
using System.Net.Mime;
using UnityEngine;

public class PauseScreen:MonoBehaviour,IEventListener
{
    [SerializeField] string showKey = "Show";
    [SerializeField] string hideKey = "Hide";
    Animator animator;

    void Start()
    {
         animator = GetComponent<Animator>();
         EventManager.instance.Register(EventName.GameEvent,this);
    }

    public ITask OnNotice(IEventArg arg)
    {
        if(arg is GameEventArg gawr)
        {
            if(gawr.state==GameState.Pause)
            {
                animator.SetTrigger("Show");
            }
            else if(gawr.state == GameState.InGame)
            {
                animator.SetTrigger("Hide");
            }
        }

        return SmallTask.NullTask;
    }
    
}