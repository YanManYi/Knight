using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fx_Fire : MonoBehaviour
{
    [HideInInspector]
    public FylingDemon parent_;
    private void Start()
    {
        Destroy(gameObject,4);
    }
    

    private void OnTriggerEnter(Collider other)
    {
        if (other .gameObject.tag=="Player")
        {
            Destroy(gameObject);

            parent_.Hit();
        }

        if (other.gameObject) Destroy(gameObject);
    }

}
