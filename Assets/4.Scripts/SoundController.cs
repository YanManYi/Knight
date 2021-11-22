using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundController : SingLeton<SoundController>
{
    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(this);
    }
}
