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

        //agent.velocity.sqrMagnitude=12多一点点
        //tree 0是idle 1是walk 3是run
        anim.SetFloat("Speed", agent.velocity.sqrMagnitude/4);
      
   

    }
}
