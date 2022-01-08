using UnityEngine;

public class SingleScriptableObject<T> : ScriptableObject
where T : SingleScriptableObject<T>
{
    [SerializeField] static T _instance;        
    public static T instance
    {
        get
        {
            return _instance;
        }
    }

    public SingleScriptableObject()
    {
        if(_instance == null)
        {
            _instance = this as T;
        }
        else if(_instance !=this&&instance != null)
        {
            Debug.LogError(typeof(T) +"is singleton!!" + "複数作らないでよ～！");
        }
    }

    void OnValidate()
    {
        if(this != _instance)
        {
            Debug.LogError(this.name + "is not the one of singleton.　殺して下さい");
        }
    }



    ~SingleScriptableObject()
    {
        if(_instance == this)
        {
            _instance = null;
        }
    }
}