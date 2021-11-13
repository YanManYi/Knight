using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerController : MonoBehaviour
{

    private NavMeshAgent agent;
    private Animator anim;

    private void Awake()
    {  
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
    }

    private void Start()
    {
        //TODO
        MouseManager.Instance.OnMouseClicked += MoveToTarget;
    }

    private void Update()
    {
        SwitchAnimation();
    }
    public void MoveToTarget(Vector3 targetPoint)
    {

        agent.SetDestination(targetPoint);
    }

    private void SwitchAnimation() {

        //agent.velocity.sqrMagnitude=12��һ���
        //tree 0��idle 1��walk 3��run
        anim.SetFloat("Speed", agent.velocity.sqrMagnitude/4);
      
   

    }
}
