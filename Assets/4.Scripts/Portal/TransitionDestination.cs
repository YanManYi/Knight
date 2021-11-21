using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransitionDestination : MonoBehaviour
{

    //Destination目的地   Transition传送，转换
    public enum DestinationTag 
    {    
        ENTER,A,B,C
    }


    public DestinationTag destinationTag;
}
