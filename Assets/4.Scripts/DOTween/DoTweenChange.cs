using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class DoTweenChange : MonoBehaviour
{
    private void Awake()
    {
    transform.parent.    transform.position = FindObjectOfType<PlayerController>().transform.position;
        transform.LookAt(FindObjectOfType<PlayerController>().transform.position);
    }
    private void Start()
    {

       
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Sequence se = DOTween.Sequence();
            se.Append(transform.DOLocalMove(new Vector3(0, -30, 0), 1f));
            se.Append(transform.DOLocalMove(new Vector3(0, 60, 0), 2));
        }
    }
}
