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
      
        SceneController.Instance.TransitionToFirstLevel();

    }

    void ContinueGame() {

        //¶ÁÈ¡½ø¶È
       
        SceneController.Instance.TransitionToSaveLoadGame();
    }

    void QuitGame()
    {
        Application.Quit(); 
    }



}
