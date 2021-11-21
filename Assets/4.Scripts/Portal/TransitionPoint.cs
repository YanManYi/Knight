using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class TransitionPoint : MonoBehaviour
{

    //传送类型
    public enum TransitionType
    {
        SameScene, DifferentScene
    }


    [Header("Transition Info")]
    public TransitionType transitionType;
    public string differentSceneName;


    [Space]
    //Destination目的地   Transition传送，转换
    public TransitionDestination.DestinationTag destinationTag;


    //可以传送了
    private bool canTrans;


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && canTrans)
        {

            //TODO:传送,杀死之前动画
            GetComponentInChildren<Text>().DOKill();
            SceneController.Instance.TransitionToDestination(this);

        }
    }




    /// <summary>
    /// 注释：空降出现的或者突然消失的都不触发对应触发器，所谓移动一下就可以
    /// </summary>
    /// <param name="other"></param>
    /// 
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            
            
            if (transitionType == TransitionType.SameScene)
                GetComponentInChildren<Text>().DOText("按E键同场景传送", 3.5f);
            if (transitionType == TransitionType.DifferentScene)
                GetComponentInChildren<Text>().DOText("按E键跨场景传送并保存数据", 3.5f);
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player") canTrans = true;
      
       
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player")) { canTrans = false; GetComponentInChildren<Text>().text = null; GetComponentInChildren<Text>().DOKill(); } 
    }

}
