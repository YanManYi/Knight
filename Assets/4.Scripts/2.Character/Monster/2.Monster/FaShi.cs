using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FaShi : EnemyController
{
  public   GameObject[] MonsterPrefabs;
    

    /// <summary>
    /// ����event  skill02�ٻ�����
    /// </summary>

    public void FaShi_Instantiate()
    {
        if (attackTarget)
        {
            GameObject temp = Instantiate(  MonsterPrefabs[(int)Random.Range(0,3)],transform.position+transform.forward,Quaternion.identity );
        }


    }

}
