using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharatersStats))]
public class CharaterCombat : MonoBehaviour
{
    CharatersStats myStats;

    private void Start()
    {
        myStats = GetComponent<CharatersStats>();
    }

    public void Attack(CharatersStats targetStats)
    {
        targetStats.TakeDamage(myStats.damage.GetValue());
    }
}
