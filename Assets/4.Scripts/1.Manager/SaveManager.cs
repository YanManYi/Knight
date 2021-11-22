using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveManager : SingLeton<SaveManager>
{
    string sceneName = "Null";
    public string SceneName { get => PlayerPrefs.GetString(sceneName); }





    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(this);
    }



    private void Update()
    { 
       
      
        
       
        if (Input.GetKeyDown(KeyCode.Escape)&&GameManager.Instance.playerStats.CurrentHealth>0&&SceneManager.GetActiveScene().name!= "Level.Main")
        {
            //传送门的时候也保存一次数据
            SavePlayerData();
          SceneController.Instance.TransitionToMainScene();

        }

   

    }

    /// <summary>
    /// 保存player数据
    /// </summary>
    public void SavePlayerData()
    {

        Save(GameManager.Instance.playerStats.characterData, GameManager.Instance.playerStats.characterData.name);

        //攻击力有升级，所以也要保存
        Save(GameManager.Instance.playerStats.attackData, GameManager.Instance.playerStats.attackData.name);

    }

    /// <summary>
    /// 加载player数据
    /// </summary>
    public void LoadPlayerData()
    {


        Load(GameManager.Instance.playerStats.characterData, GameManager.Instance.playerStats.characterData.name);
        //攻击力有升级，所以也要加载
        Load(GameManager.Instance.playerStats.attackData, GameManager.Instance.playerStats.attackData.name);
    }



    #region MyRegion

    //  CharacterData_SO 直接写这个类太局限，所以object是全部类的父类
    public void Save(Object data, string key)
    {
        //人物状态属性和攻击属性把它变成字符串形式
        var jsonData = JsonUtility.ToJson(data, true);

        PlayerPrefs.SetString(key, jsonData);

       


           PlayerPrefs.SetString(sceneName, SceneManager.GetActiveScene().name);

      

        if (FindObjectOfType<PlayerController>())
        {
            Vector3 player = FindObjectOfType<PlayerController>().transform.position;

            FindObjectOfType<PlayerController>().agent.isStopped = true;
            //异场景不让执行到存储位置，而是默认传送门那个点位置并且开始保存加载的保存场景
            PlayerPrefs.SetFloat("X", player.x);
            PlayerPrefs.SetFloat("Y", player.y);
            PlayerPrefs.SetFloat("Z", player.z);
        }


        PlayerPrefs.Save();
    }





    public void Load(Object data, string key)
    {

        if (PlayerPrefs.HasKey(key))
        {
            //拿到字符串然后写进人物属性和攻击面板
            JsonUtility.FromJsonOverwrite(PlayerPrefs.GetString(key), data);

            if (SceneManager.GetActiveScene().name == PlayerPrefs.GetString(sceneName))
            {

                Vector3 player = FindObjectOfType<PlayerController>().transform.position;

                player.x = PlayerPrefs.GetFloat("X");
                player.y = PlayerPrefs.GetFloat("Y");
                player.z = PlayerPrefs.GetFloat("Z");

                FindObjectOfType<PlayerController>().transform.position = player;
                FindObjectOfType<PlayerController>().agent.destination = player;

            }
        }

    }


    #endregion




}
