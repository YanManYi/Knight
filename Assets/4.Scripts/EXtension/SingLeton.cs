using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//泛型类单例
public class SingLeton<T> : MonoBehaviour where T : SingLeton<T>
{
    private static T instance;

    public static T Instance { get { return instance; } }


    //因为Awake可能还需要被添加使用，使用需要被重写的
    protected virtual void Awake()
    {
        if (instance != null)
            Destroy(gameObject);
        else
            instance = (T)this;

    }


    //好的属性和方法 

    // 判断是否生成单例
    public static bool IsInitialized
    {
        get { return instance != null; }
    }


    //销毁的时候清空这个单例对象变量
    protected virtual void OnDestroy()
    {
        if (instance == this)
        {
            instance = null;
        }

    }

}