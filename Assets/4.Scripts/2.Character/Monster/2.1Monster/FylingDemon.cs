using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FylingDemon : EnemyController
{
    
  
    [Range(0,30)]
    public float Fx_fireFore;


    /// <summary>
    /// skill02��¡�����򣬸������Լ���data��Ȼ������Լ���Hitȥ��Ѫ
    /// </summary>

    public void DemonFire()
    {
        if (attackTarget)
        {
            GameObject temp = Instantiate(Resources.Load<GameObject>("FX-2.1"));
            temp.transform.position = transform.DG_FindChild("FirePoint", transform).position;
            temp.transform.LookAt(attackTarget.transform.position + Vector3.up);
            //����Ŀ�괫�ݵ��ӵ�������
            temp.GetComponent<Fx_Fire>().parent_ =this;
          temp.GetComponent<Rigidbody>().AddForce(temp.transform.forward* Fx_fireFore, ForceMode.Impulse);

            
        }

    }


}
