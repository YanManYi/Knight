using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarUI : MonoBehaviour
{

    private GameObject healthUIPrefab;//血条预制体
    private GameObject BarPoint;//位置


    private Transform healthUI;
    private Image healthSlider;

    private Transform cameraPoint;

    CharacterStats currentStats;
    private void Awake()
    {
        healthUIPrefab = Resources.Load<GameObject>("UI/Bar Holder");
        BarPoint = transform.DG_FindChild("HealthPoint", transform).gameObject;

        //全局只有一个声音监听,可以FindObjectOfType<AudioListener>().transform
        cameraPoint = Camera.main.transform;


        currentStats = GetComponent<CharacterStats>();
        currentStats.UpdateHealthBarOnAttack += UpdateHealthMonthed;
    }

    private void OnEnable()
    {


        foreach (var canvas in FindObjectsOfType<Canvas>())
        {
            if (canvas.gameObject.name == "HealthBarCanvas")
            {
                healthUI = Instantiate(healthUIPrefab, canvas.transform).transform;

                healthSlider = healthUI.GetChild(0).GetComponent<Image>();
                healthUI.gameObject.SetActive(false);
            }
        }

    }
    private void UpdateHealthMonthed(int currentHealth, int maxHealth)
    {
        if (currentStats.CurrentHealth <= 0) Destroy(healthUI.gameObject);
        StopCoroutine("enumerator");
        healthUI.gameObject.SetActive(true);
        StartCoroutine("enumerator");

        float sliderPercent = (float)currentHealth / maxHealth;

        healthSlider.fillAmount = sliderPercent;

        if (healthSlider.fillAmount < 0.8f)
        {
            healthSlider.color = new Color(1, 1, 0);
            if (healthSlider.fillAmount < 0.3f)
                healthSlider.color = new Color(1, 0, 0);


        }

    }

    IEnumerator enumerator()
    {

        yield return new WaitForSeconds(3);

        healthUI.gameObject.SetActive(false);
        yield break;
    }


    private void LateUpdate()
    {

        if (healthUI)
        {

            healthUI.transform.LookAt(cameraPoint);
            healthUI.transform.position = BarPoint.transform.position;


        }
    }
}
