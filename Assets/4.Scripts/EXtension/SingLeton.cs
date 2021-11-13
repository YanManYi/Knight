using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//�����൥��
public class SingLeton<T> : MonoBehaviour where T : SingLeton<T>
{
    private static T instance;

    public static T Instance { get { return instance; } }


    //��ΪAwake���ܻ���Ҫ�����ʹ�ã�ʹ����Ҫ����д��
    protected virtual void Awake()
    {
        if (instance != null)
            Destroy(gameObject);
        else
            instance = (T)this;

    }


    //�õ����Ժͷ��� 

    // �ж��Ƿ����ɵ���
    public static bool IsInitialized
    {
        get { return instance != null; }
    }


    //���ٵ�ʱ�������������������
    protected virtual void OnDestroy()
    {
        if (instance == this)
        {
            instance = null;
        }

    }

}