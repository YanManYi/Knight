using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.AI;

public class SceneController : SingLeton<SceneController>,IEndGameObserver
{

    private GameObject player;//同场景就是位移过去
    private GameObject playerPrefab;//重新创建出来

    bool fadeIsPlay;

    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(this);
        fadeIsPlay = true;
    }

    private void Start()
    {
        playerPrefab = Resources.Load<GameObject>("Player");

      
          GameManager.Instance.endGameObservers.Add(this);

    }



    //    #region 传送门相关
    /// <summary>
    /// 传送到终点
    /// </summary>
    /// <param name="transitionPoint">这个门需要的类型</param>
    public void TransitionToDestination(TransitionPoint transitionPoint)
    {
        //当前传送门想要传送的类型
        switch (transitionPoint.transitionType)
        {
            //同场景情况
            case TransitionPoint.TransitionType.SameScene:

                StartCoroutine(Transition(SceneManager.GetActiveScene().name, transitionPoint.destinationTag));
                break;

            //不同场景情况
            case TransitionPoint.TransitionType.DifferentScene:

                StartCoroutine(Transition(transitionPoint.differentSceneName, transitionPoint.destinationTag));
                break;


            default:
                break;
        }

    }


    /// <summary>
    /// 同场景和不同场景都用同一个协程方法名
    /// </summary>
    /// <param name="sceneName">场景名字</param>
    /// <param name="destinationTag">传送门那个小点位置的类型</param>
    /// <returns></returns>
    IEnumerator Transition(string sceneName, TransitionDestination.DestinationTag destinationTag)
    {
        //TODO:数据还没加，这里保存数据
        SaveManager.Instance.SavePlayerData();
     

        //当前激活的场景和想要的场景名字，不一致，也就是异场景情况
        if (SceneManager.GetActiveScene().name != sceneName)
        {
           SceneFade fade = Instantiate(Resources.Load<GameObject>("UI/FadeCanvas")).GetComponent<SceneFade>();
            yield return StartCoroutine(fade.FadeOut(fade.fadeInDurtion));

            yield return SceneManager.LoadSceneAsync(sceneName);
            yield return Instantiate(playerPrefab, GetDestination(destinationTag).transform.position, GetDestination(destinationTag).transform.rotation);

            //空数据player读取存储数据
            SaveManager.Instance.LoadPlayerData();
            //再次保存额外用于加载，场景和位置的情况
            SaveManager.Instance.SavePlayerData();

          yield return StartCoroutine(fade.FadeIn(fade.fadeInDurtion));

            yield break;//结束跳出协程

        }
        else
        {
            //同场景的传送

            player = GameManager.Instance.playerStats.gameObject;
            NavMeshAgent agent = player.GetComponent<NavMeshAgent>();
            //NavMeshAgent之前的目标点还在需要取消一下
            agent.enabled = false;
            player.transform.SetPositionAndRotation(GetDestination(destinationTag).transform.position, GetDestination(destinationTag).transform.rotation);
            agent.enabled = true;



            yield break;
        }


    }


    /// <summary>
    /// 传送门
    /// </summary>
    /// <param name="destinationTag">那个传送门前面那个点</param>
    /// <returns></returns>
    private TransitionDestination GetDestination(TransitionDestination.DestinationTag destinationTag)
    {

        //找到全部的传送点
        var transitionDestination = FindObjectsOfType<TransitionDestination>();

        for (int i = 0; i < transitionDestination.Length; i++)
        {
            if (transitionDestination[i].destinationTag == destinationTag)
            {
                return transitionDestination[i];
            }
        }

        return null;

    }







    /// <summary>
    /// NewGameBtn调用的
    /// </summary>
    public void TransitionToFirstLevel()
    {
        StartCoroutine(LoadLevel("Level.01"));

    }

    /// <summary>
    /// ContinueGameBtn调用的
    /// </summary>
    public void TransitionToSaveLoadGame()
    {
        if(SaveManager.Instance.SceneName!="Null")//第一次没存数据之前的bug可能
        StartCoroutine(LoadLevel(SaveManager.Instance.SceneName));
    }

    /// <summary>
    /// 加载场景会产生player
    /// </summary>
    /// <param name="scene"></param>
    /// <returns></returns>
    IEnumerator LoadLevel(string scene)
    {
        

        if (scene != "")
        {
           SceneFade fade = Instantiate(Resources.Load<GameObject>("UI/FadeCanvas")).GetComponent<SceneFade>();

          yield return StartCoroutine(fade.FadeOut(fade.fadeInDurtion));

            yield return SceneManager.LoadSceneAsync(scene);
            yield return player = Instantiate(playerPrefab, GetEntrance().position, GetEntrance().rotation);

            //保存游戏,如果是newgame，那么这里保存就是清空之前的后需要保存一次

            SaveManager.Instance.SavePlayerData();

          yield return StartCoroutine(fade.FadeIn(fade.fadeInDurtion));
            yield break;

        }
    }



    /// <summary>
    /// 获取开始场景的Enter入口传送门
    /// </summary>
    /// <returns></returns>
    public Transform GetEntrance()
    {
        foreach (var item in FindObjectsOfType<TransitionDestination>())
        {
            if (item.destinationTag == TransitionDestination.DestinationTag.ENTER)
            {
                return item.transform;
            }

        }
        return null;


    }






    /// <summary>
    /// 按键Esc直接返回Main场景
    /// </summary>
    public void TransitionToMainScene() { StartCoroutine(LoadMainScene()); }


    
    /// <summary>
    /// 订阅player死亡信息,会持续调用，一直开启协程，需要多个bool变量解决
    /// </summary>
    public void EndNotify()
    {
      
        if (fadeIsPlay)
        {
            fadeIsPlay = false;
        StartCoroutine(LoadMainScene());

        }

    }


    /// <summary>
    /// 协程加载Main
    /// </summary>
    /// <returns></returns>
    IEnumerator LoadMainScene()
    {
        SceneFade fade = Instantiate(Resources.Load<GameObject>("UI/FadeCanvas")).GetComponent<SceneFade>();
        
        yield return StartCoroutine(fade.FadeOut(fade.fadeInDurtion));
        yield return SceneManager.LoadSceneAsync("Level.Main");
        yield return StartCoroutine(fade.FadeIn(fade.fadeInDurtion));
        fadeIsPlay = true;//第二次死一样返回menu
        yield break;

    }


    
 
}
