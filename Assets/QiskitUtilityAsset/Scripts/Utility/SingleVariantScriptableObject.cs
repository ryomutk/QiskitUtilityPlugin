using UnityEngine;
using System.Collections.Generic;
using System.Linq;

//同じクラスの人を一人だけ作れるタイプのScriptableObject
public abstract class SingleVariantScriptableObject<T> : ScriptableObject
where T : SingleVariantScriptableObject<T>
{
    public static List<T> variantInstances = new List<T>();

    public SingleVariantScriptableObject()
    {
        //同じクラスの人がいたら消す
        if (!variantInstances.Any(x => x.GetType() == this.GetType()))
        {
            variantInstances.Add(this as T);
        }
        else
        {
            Debug.LogError("too many" + typeof(T) + "複数作らないでよ～！");
        }
    }


    ~SingleVariantScriptableObject()
    {
        var instance = variantInstances.Find(x => x.GetType() == this.GetType());

        //保存されているInstanceがわたくせなら消す
        if (instance == this)
        {
            variantInstances.Remove(instance);
        }
    }
}