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

    /// <summary>
    /// 判断是否生成单例,因为退出编辑模式的时候单例丢失报空的编辑器错误，使用这个以此判断
    /// </summary>
    public static bool IsInitialized
    {
        get { return instance==null ?false:true; }
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