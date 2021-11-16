using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class KillDamageCanvas : MonoBehaviour
{

    Text textCom;
    Transform camera_;

   

    private void Awake()
    {
        textCom = GetComponentInChildren<Text>();

        textCom.color = new Color(0, 1, 0);


       
    }
    private void Start()
    {
        camera_ = FindObjectOfType<Camera>().transform;
 
     
       
    }


    private void LateUpdate()
    {
        transform.LookAt(camera_.position);
    }



    /// <summary>
    /// 
    /// </summary>
    /// <param name="attacker"></param>
    /// <param name="defener"></param>
    /// <param name="damage">计算后的伤害值</param>
    public void DamageActive(CharacterStats attacker, CharacterStats defener,int damage) {

        textCom.text = "-"+damage;

        if (attacker.isCritical)
        {
            textCom.color = new Color(1,0,0);
           textCom. transform.DOScale(new Vector3 (1.5f,1.5f,1.5f),1.5f);
        }

        //播放DoTween动画
        transform.position = defener.transform.position + Vector3.up ;//世界物体遮盖可以多一个深度相机显示


        //transform.DOMove(
        //    transform.position+ new Vector3 (0,-0.1f,0),0.5f).OnComplete(() => {
        //       textCom.DOFade(0.2f, 2f);
        //   transform.DOMove( transform.position + new Vector3(0, 3, 0),2 ).OnComplete(() => { Destroy(gameObject); });
        //   });
        
        //改换canvas自身y轴的移动

        transform.DOMove(
            transform.position + transform.up*-0.1f, 0.5f).OnComplete(() => {
                textCom.DOFade(0f, 1.5f);
                transform.DOMove(transform.position +transform.up * 1.5f + transform.right * (Random.value <= 0.5f ? Random.value : -Random.value), 1.5f).OnComplete(() => {
                    Destroy(gameObject);//这里的Canvas可以优化重复利用，优化的事情以后再说
                });
            });



    }


}
