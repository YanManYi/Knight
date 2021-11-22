using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.AI;

public class SceneController : SingLeton<SceneController>,IEndGameObserver
{

    private GameObject player;//ͬ��������λ�ƹ�ȥ
    private GameObject playerPrefab;//���´�������

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



    //    #region ���������
    /// <summary>
    /// ���͵��յ�
    /// </summary>
    /// <param name="transitionPoint">�������Ҫ������</param>
    public void TransitionToDestination(TransitionPoint transitionPoint)
    {
        //��ǰ��������Ҫ���͵�����
        switch (transitionPoint.transitionType)
        {
            //ͬ�������
            case TransitionPoint.TransitionType.SameScene:

                StartCoroutine(Transition(SceneManager.GetActiveScene().name, transitionPoint.destinationTag));
                break;

            //��ͬ�������
            case TransitionPoint.TransitionType.DifferentScene:

                StartCoroutine(Transition(transitionPoint.differentSceneName, transitionPoint.destinationTag));
                break;


            default:
                break;
        }

    }


    /// <summary>
    /// ͬ�����Ͳ�ͬ��������ͬһ��Э�̷�����
    /// </summary>
    /// <param name="sceneName">��������</param>
    /// <param name="destinationTag">�������Ǹ�С��λ�õ�����</param>
    /// <returns></returns>
    IEnumerator Transition(string sceneName, TransitionDestination.DestinationTag destinationTag)
    {
        //TODO:���ݻ�û�ӣ����ﱣ������
        SaveManager.Instance.SavePlayerData();
     

        //��ǰ����ĳ�������Ҫ�ĳ������֣���һ�£�Ҳ�����쳡�����
        if (SceneManager.GetActiveScene().name != sceneName)
        {
           SceneFade fade = Instantiate(Resources.Load<GameObject>("UI/FadeCanvas")).GetComponent<SceneFade>();
            yield return StartCoroutine(fade.FadeOut(fade.fadeInDurtion));

            yield return SceneManager.LoadSceneAsync(sceneName);
            yield return Instantiate(playerPrefab, GetDestination(destinationTag).transform.position, GetDestination(destinationTag).transform.rotation);

            //������player��ȡ�洢����
            SaveManager.Instance.LoadPlayerData();
            //�ٴα���������ڼ��أ�������λ�õ����
            SaveManager.Instance.SavePlayerData();

          yield return StartCoroutine(fade.FadeIn(fade.fadeInDurtion));

            yield break;//��������Э��

        }
        else
        {
            //ͬ�����Ĵ���

            player = GameManager.Instance.playerStats.gameObject;
            NavMeshAgent agent = player.GetComponent<NavMeshAgent>();
            //NavMeshAgent֮ǰ��Ŀ��㻹����Ҫȡ��һ��
            agent.enabled = false;
            player.transform.SetPositionAndRotation(GetDestination(destinationTag).transform.position, GetDestination(destinationTag).transform.rotation);
            agent.enabled = true;



            yield break;
        }


    }


    /// <summary>
    /// ������
    /// </summary>
    /// <param name="destinationTag">�Ǹ�������ǰ���Ǹ���</param>
    /// <returns></returns>
    private TransitionDestination GetDestination(TransitionDestination.DestinationTag destinationTag)
    {

        //�ҵ�ȫ���Ĵ��͵�
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
    /// NewGameBtn���õ�
    /// </summary>
    public void TransitionToFirstLevel()
    {
        StartCoroutine(LoadLevel("Level.01"));

    }

    /// <summary>
    /// ContinueGameBtn���õ�
    /// </summary>
    public void TransitionToSaveLoadGame()
    {
        if(SaveManager.Instance.SceneName!="Null")//��һ��û������֮ǰ��bug����
        StartCoroutine(LoadLevel(SaveManager.Instance.SceneName));
    }

    /// <summary>
    /// ���س��������player
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

            //������Ϸ,�����newgame����ô���ﱣ��������֮ǰ�ĺ���Ҫ����һ��

            SaveManager.Instance.SavePlayerData();

          yield return StartCoroutine(fade.FadeIn(fade.fadeInDurtion));
            yield break;

        }
    }



    /// <summary>
    /// ��ȡ��ʼ������Enter��ڴ�����
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
    /// ����Escֱ�ӷ���Main����
    /// </summary>
    public void TransitionToMainScene() { StartCoroutine(LoadMainScene()); }


    
    /// <summary>
    /// ����player������Ϣ,��������ã�һֱ����Э�̣���Ҫ���bool�������
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
    /// Э�̼���Main
    /// </summary>
    /// <returns></returns>
    IEnumerator LoadMainScene()
    {
        SceneFade fade = Instantiate(Resources.Load<GameObject>("UI/FadeCanvas")).GetComponent<SceneFade>();
        
        yield return StartCoroutine(fade.FadeOut(fade.fadeInDurtion));
        yield return SceneManager.LoadSceneAsync("Level.Main");
        yield return StartCoroutine(fade.FadeIn(fade.fadeInDurtion));
        fadeIsPlay = true;//�ڶ�����һ������menu
        yield break;

    }


    
 
}
