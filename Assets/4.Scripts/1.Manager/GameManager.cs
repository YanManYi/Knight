using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : SingLeton<GameManager>
{


    public CharacterStats playerStats;

    //�۲����б�,�ӿ��б�
    List<IEndGameObserver> endGameObservers = new List<IEndGameObserver>();

    //�۲���ģʽ����,��һ��˼·��

    //дһ���ӿ�IEndGameObserver�������������дһ������EndNotify(),��ʹ���˽ӿڵĶ������ﶼ��EnemyControllerʹ�ã�
    //Ȼ����GameManager�������ӿ��б�GameManager����Ҫ�п������(����)���Ƴ��ӿ��б���Ķ��󣬹�������ʹ�ã��������ﶼ��EnemyControllerʹ�ã���Ϊ���̳��˽ӿڣ�
    //�������������������ã��㲥��ÿһ����ӽ��б�Ķ���Ľӿ���Notify����
    public void RigisterPlayer(CharacterStats characterStats)
    {
        playerStats = characterStats;
    }


    /// <summary>
    /// ���õĶ��������۲����б��׳�"����"
    /// </summary>
    /// <param name="observer"></param>
    public void AddObserver(IEndGameObserver observer)
    {
        endGameObservers.Add(observer);

    }
    /// <summary>
    /// ���õĶ����Ƴ����۲����б�
    /// </summary>
    /// <param name="observer"></param>
    public void RemoveObserver(IEndGameObserver observer)
    {
        endGameObservers.Remove (observer);

    }


    /// <summary>
    /// ֪ͨ�۲����б��ÿһ���۲��ߣ��׳�"�㲥"
    /// </summary>
    public void NotifyObserver()
    {

        foreach (var observer in endGameObservers)
        {
            observer.EndNotify();
        }
    }


}
