using System.Collections.Generic;
using UnityEngine;

public class QPongManager : Singleton<QPongManager>, IEventListener
{
    [SerializeField] Ball ball;
    GameState state = GameState.Pause;

    public Dictionary<Actor,int> lifetable = new Dictionary<Actor, int>();


    void Start()
    {
        EventManager.instance.Register(EventName.GameEvent, this);
        EventManager.instance.Register(EventName.DamageEvent,this);
        lifetable[Actor.player] = QPongConfig.instance.playerLife;
        lifetable[Actor.enemy] = QPongConfig.instance.enemyLife;
    }

    void Update()
    {
        if (state == GameState.Pause)
        {
            if (Input.GetKeyDown("space"))
            {
                UpdateState(GameState.InGame);
            }
        }
    }

    void UpdateState(GameState state)
    {
        EventManager.instance.Notice(EventName.GameEvent, new GameEventArg(state));
        this.state = state;
    }

    public ITask OnNotice(IEventArg arg)
    {
        if (arg is GameEventArg garg)
        {
            if (garg.state == GameState.InGame)
            {
                ball.Restart();
            }

        }
        else if(arg is DamageEventArg darg)
        {
            lifetable[darg.subject] -= darg.damage;

            if(lifetable.ContainsValue(0))
            {
                if(lifetable[Actor.player]!=0)
                {
                    UpdateState(GameState.YouWon);
                }
                else
                {
                    UpdateState(GameState.EnemyWon);
                }
            }
            else
            {
                UpdateState(GameState.Pause);
                ball.Pause();
            }
        }

        return SmallTask.NullTask;
    }

}
