using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class TransitionPoint : MonoBehaviour
{

    //��������
    public enum TransitionType
    {
        SameScene, DifferentScene
    }


    [Header("Transition Info")]
    public TransitionType transitionType;
    public string differentSceneName;


    [Space]
    //DestinationĿ�ĵ�   Transition���ͣ�ת��
    public TransitionDestination.DestinationTag destinationTag;


    //���Դ�����
    private bool canTrans;


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && canTrans)
        {

            //TODO:����,ɱ��֮ǰ����
            GetComponentInChildren<Text>().DOKill();
            SceneController.Instance.TransitionToDestination(this);

        }
    }




    /// <summary>
    /// ע�ͣ��ս����ֵĻ���ͻȻ��ʧ�Ķ���������Ӧ����������ν�ƶ�һ�¾Ϳ���
    /// </summary>
    /// <param name="other"></param>
    /// 
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            
            
            if (transitionType == TransitionType.SameScene)
                GetComponentInChildren<Text>().DOText("��E��ͬ��������", 3.5f);
            if (transitionType == TransitionType.DifferentScene)
                GetComponentInChildren<Text>().DOText("��E���糡�����Ͳ���������", 3.5f);
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
