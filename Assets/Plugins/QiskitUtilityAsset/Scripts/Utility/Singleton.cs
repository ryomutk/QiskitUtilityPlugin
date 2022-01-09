using UnityEngine;

public class Singleton<T> : MonoBehaviour
where T : Singleton<T>
{
    public static T instance;

    //DontDestroyOnLoad
    [SerializeField] bool DDOL = true;

    protected virtual void Awake()
    {
        if (DDOL)
        {
            DontDestroyOnLoad(this);
        }
        
        CreateSingleton();
    }

    void CreateSingleton()
    {
        if (instance == null)
        {
            instance = this as T;
        }
        else
        {
            Debug.LogError(this.name + "is Singleton!!" + "Destroying...");
            Destroy(this.gameObject);
        }
    }
}