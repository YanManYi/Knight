using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class KillDamageCanvas : MonoBehaviour
{

    Text textCom;
    Transform camera_;


    //��Ϊ���ö���أ���Ҫ�ظ�����Ӧ��д��OnEnable
    //private void Awake()
    //{
    //    textCom = GetComponentInChildren<Text>();
    //    textCom.color = new Color(0, 0, 1);
    // textCom.transform.position.Scale(new Vector3(1f, 1f, 1f));
    //}
    private void Start()
    {
        camera_ = FindObjectOfType<Camera>().transform;       
    }

    private void OnEnable()
    {
        textCom = GetComponentInChildren<Text>();
        textCom.color = new Color(0, 0, 1,1);
        textCom.transform.position.Scale(new Vector3(1f, 1f, 1f));

    }


    private void LateUpdate()
    {
        transform.LookAt(camera_.position);
    }



    /// <summary>
    /// DoTween��ʾDamage
    /// </summary>
    /// <param name="attacker"></param>
    /// <param name="defener"></param>
    /// <param name="damage">�������˺�ֵ</param>
    public void DamageActive(CharacterStats attacker, CharacterStats defener,int damage) {

        textCom.text = "-"+damage;

        if (attacker.isCritical)
        {
            textCom.color = new Color(1,0,0);
           textCom. transform.DOScale(new Vector3 (1.5f,1.5f,1.5f),1.5f);
        }

        //����DoTween����
        transform.position = defener.transform.position + Vector3.up ;//���������ڸǿ��Զ�һ����������ʾ



        //����canvas����y����ƶ�

        transform.DOMove( transform.position + transform.up * -0.1f, 0.5f). OnComplete      
          (

            () => 
            {
                textCom.DOFade(0f, 1.5f);
                Vector3 vec = transform.position + transform.up * 1.5f + transform.right * (Random.value <= 0.5f ? Random.value : -Random.value);

            transform.DOMove(vec, 1.5f).OnComplete
            (
                () => 
            {
               //�����DoTweenCanvas�����Ż��ظ����û���أ�Ҳ�ж����   
                gameObject.SetActive(false);
            }

            );                         
                
            }


          );



    }


}
