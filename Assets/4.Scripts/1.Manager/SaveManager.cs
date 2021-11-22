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
            //�����ŵ�ʱ��Ҳ����һ������
            SavePlayerData();
          SceneController.Instance.TransitionToMainScene();

        }

   

    }

    /// <summary>
    /// ����player����
    /// </summary>
    public void SavePlayerData()
    {

        Save(GameManager.Instance.playerStats.characterData, GameManager.Instance.playerStats.characterData.name);

        //������������������ҲҪ����
        Save(GameManager.Instance.playerStats.attackData, GameManager.Instance.playerStats.attackData.name);

    }

    /// <summary>
    /// ����player����
    /// </summary>
    public void LoadPlayerData()
    {


        Load(GameManager.Instance.playerStats.characterData, GameManager.Instance.playerStats.characterData.name);
        //������������������ҲҪ����
        Load(GameManager.Instance.playerStats.attackData, GameManager.Instance.playerStats.attackData.name);
    }



    #region MyRegion

    //  CharacterData_SO ֱ��д�����̫���ޣ�����object��ȫ����ĸ���
    public void Save(Object data, string key)
    {
        //����״̬���Ժ͹������԰�������ַ�����ʽ
        var jsonData = JsonUtility.ToJson(data, true);

        PlayerPrefs.SetString(key, jsonData);

       


           PlayerPrefs.SetString(sceneName, SceneManager.GetActiveScene().name);

      

        if (FindObjectOfType<PlayerController>())
        {
            Vector3 player = FindObjectOfType<PlayerController>().transform.position;

            FindObjectOfType<PlayerController>().agent.isStopped = true;
            //�쳡������ִ�е��洢λ�ã�����Ĭ�ϴ������Ǹ���λ�ò��ҿ�ʼ������صı��泡��
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
            //�õ��ַ���Ȼ��д���������Ժ͹������
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
