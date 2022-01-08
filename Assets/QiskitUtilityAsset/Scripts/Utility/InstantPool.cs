using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

//クラス単位での管理が不便な場合に使うObjPool。
//同じクラスのプールを(prefab毎とかで)いくつでも作成できる代わりに、
//singletonでのインターフェイスは確保できない(作成時に返される参照でしかアクセスできない。) 
public class InstantPool<T>
where T : Component
{
    T objPrefab = null;
    Transform parent;
    List<T> objList;
    Transform transform;

    public InstantPool(Transform trans = null) : base()
    {
        transform = trans;
        objList = new List<T>();
    }

    public void ForeachObject(Action<T> process)
    {
        for(int i = 0;i < objList.Count;i++)
        {
            var obj = objList[i];
            process(obj);
        }
    }

    public bool CreatePool(T obj, int num, bool makeParent)
    {
        if (num >= 0)
        {
            if (makeParent)
            {
                parent = new GameObject(obj.name + "Pool").transform;
                parent.localPosition = Vector3.zero;
            }
            else
            {
                parent = transform;
            }

            if (transform != null)
            {
                parent.SetParent(transform);
                parent.transform.localScale = Vector3.one;
            }

            objPrefab = obj;

            for (int i = 0; i < num; i++)
            {
                CreateObj();
            }

        }
        return false;
    }
    /*
            public Task CreatePoolAsync(T obj, int num)
            {

                Task task = Task.Run(
                    () =>
                    {
                        parent = new GameObject(obj.name + "Pool").transform;

                        if (transform != null)
                        {
                            parent.SetParent(transform);
                            parent.transform.localScale = Vector3.one;
                        }

                        objPrefab = obj;
                        _state = ModuleState.working;

                        for (int i = 0; i < num; i++)
                        {
                            CreateObj();
                        }

                        _state = ModuleState.ready;
                    }
                );
                return task;
            }
    */

    public T GetObj(bool activate = true)
    {
        T returnObj = null;
        foreach (T obj in objList)
        {
            if (!obj.gameObject.activeSelf)
            {
                returnObj = obj;
                break;
            }
        }

        if (returnObj == null)
        {
            returnObj = CreateObj();
        }

        if (activate)
        {

            returnObj.gameObject.SetActive(true);
        }

        return returnObj;
    }

    T CreateObj()
    {

        var clone = MonoBehaviour.Instantiate(objPrefab, parent);
        clone.name += objList.Count;
        clone.transform.localPosition = Vector2.zero;
        objList.Add(clone);
        clone.gameObject.SetActive(false);
        return clone;
    }

    public void DisableAll()
    {
        foreach (var obj in objList)
        {
            obj.gameObject.SetActive(false);
        }
    }
}

