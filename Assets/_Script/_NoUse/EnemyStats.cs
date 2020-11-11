using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : CharatersStats
{
    public override void Die()
    {
        base.Die();

        //Animtion

        Destroy(gameObject);
    }
}
