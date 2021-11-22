using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthUI : MonoBehaviour
{

    private  Image levelSlider;
    private  Text  level;
 
    private  Image healthSlider;
    private Text  health;

    private void Awake()
    {
        levelSlider = transform.DG_FindChild("LevelSlider", transform).GetComponent<Image>();
        level =levelSlider. GetComponentInChildren<Text>();

        healthSlider= transform.DG_FindChild("HealthSlider", transform).GetComponent<Image>();
        health= healthSlider. GetComponentInChildren<Text>();
    }

    private void Update()
    {
        //经验百分比
    
            float sliderPercent_Exp = (float)GameManager.Instance.playerStats.characterData.currentExp / GameManager.Instance.playerStats.characterData.baseExp;
            levelSlider.fillAmount = sliderPercent_Exp;
            level.text = GameManager.Instance.playerStats.characterData.currentLevel.ToString();


            //血量百分比
            float sliderPercent = (float)GameManager.Instance.playerStats.CurrentHealth / GameManager.Instance.playerStats.MaxHealth;
            healthSlider.fillAmount = sliderPercent;
            health.text = (GameManager.Instance.playerStats.CurrentHealth) + "" + "/" + (GameManager.Instance.playerStats.MaxHealth);

        if (healthSlider.fillAmount < 0.8f)
        {
            healthSlider.color = new Color(1, 1, 0);
            if (healthSlider.fillAmount < 0.3f)
                healthSlider.color = new Color(1, 0, 0);


        }

    }



}
