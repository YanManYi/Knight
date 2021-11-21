using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Playables;


public class MainMenuCanvas : MonoBehaviour
{
    Button newGameBtn, continueBtn, quitBtn;

    PlayableDirector playableDirector;
    private void Awake()
    {

        newGameBtn = transform.DG_FindChild("NewGameBtn", transform).GetComponent<Button>();
        continueBtn = transform.DG_FindChild("ContinueGameBtn", transform).GetComponent<Button>();
        quitBtn = transform.DG_FindChild("QuitGamrBtn", transform).GetComponent<Button>();

        newGameBtn.onClick.AddListener(()=> { PlayTimeline(); });
       continueBtn.onClick.AddListener(()=> { ContinueGame(); });

        quitBtn.onClick.AddListener(()=> { QuitGame(); });

        playableDirector = FindObjectOfType<PlayableDirector>();
    }



     void PlayTimeline()
    {
        playableDirector.Play();
    }


    public void TimelineOverEvent() { NewGame(); }

    void NewGame()
    {
        PlayerPrefs.DeleteAll();
        Debug.Log("重新");
        SceneController.Instance.TransitionToFirstLevel();

    }

    void ContinueGame() {

        //读取进度
        Debug.Log("继续");
        SceneController.Instance.TransitionToSaveLoadGame();
    }

    void QuitGame()
    {
        Application.Quit(); Debug.Log("离开");
    }



}
