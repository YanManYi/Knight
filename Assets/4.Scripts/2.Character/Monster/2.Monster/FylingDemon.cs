using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FylingDemon : EnemyController
{
    
  
    [Range(0,30)]
    public float Fx_fireFore;


    /// <summary>
    /// skill02克隆出火球，附带上自己的data，然后调用自己的Hit去扣血
    /// </summary>

    public void HitDemonFire()
    {
        if (attackTarget)
        {
            GameObject temp = Instantiate(Resources.Load<GameObject>("FX-2.1"));
            temp.transform.position = transform.DG_FindChild("FirePoint", transform).position;
            temp.transform.LookAt(attackTarget.transform.position + Vector3.up*1.5f);
            //攻击目标传递到子弹火球上
            temp.GetComponent<Fx_Fire>().parent_ =this;
          temp.GetComponent<Rigidbody>().AddForce(temp.transform.forward* Fx_fireFore, ForceMode.Impulse);

            
        }

    }

    public void HitDemonFire_Boss()
    {
        if (attackTarget)
        {
            GameObject temp = Instantiate(Resources.Load<GameObject>("FX-2.4"));
            temp.transform.position = transform.DG_FindChild("FirePoint", transform).position;
            temp.transform.LookAt(attackTarget.transform.position + Vector3.up * 1.5f);
            //攻击目标传递到子弹火球上
            temp.GetComponent<Fx_Fire>().parent_ = this;
            temp.GetComponent<Rigidbody>().AddForce(temp.transform.forward * Fx_fireFore, ForceMode.Impulse);


        }

    }


}
