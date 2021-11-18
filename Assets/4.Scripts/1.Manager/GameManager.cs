using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : SingLeton<GameManager>
{


    public CharacterStats playerStats;

    //观察者列表,接口列表
    List<IEndGameObserver> endGameObservers = new List<IEndGameObserver>();

    //观察者模式流程,理一下思路：

    //写一个接口IEndGameObserver，它负责抽象先写一个函数EndNotify(),供使用了接口的对象（这里都是EnemyController使用）
    //然后再GameManager里声明接口列表，GameManager类里要有可以添加(订阅)和移除接口列表里的对象，供其他类使用（其他这里都是EnemyController使用，因为它继承了接口）
    //最后满足条件的情况调用（广播）每一个添加进列表的对象的接口中Notify方法
    public void RigisterPlayer(CharacterStats characterStats)
    {
        playerStats = characterStats;
    }


    /// <summary>
    /// 调用的对象加入进观察者列表，俗称"订阅"
    /// </summary>
    /// <param name="observer"></param>
    public void AddObserver(IEndGameObserver observer)
    {
        endGameObservers.Add(observer);

    }
    /// <summary>
    /// 调用的对象移除出观察者列表
    /// </summary>
    /// <param name="observer"></param>
    public void RemoveObserver(IEndGameObserver observer)
    {
        endGameObservers.Remove (observer);

    }


    /// <summary>
    /// 通知观察者列表的每一个观察者，俗称"广播"
    /// </summary>
    public void NotifyObserver()
    {

        foreach (var observer in endGameObservers)
        {
            observer.EndNotify();
        }
    }


}
