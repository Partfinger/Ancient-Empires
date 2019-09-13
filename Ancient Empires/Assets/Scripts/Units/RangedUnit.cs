using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedUnit : Unit
{
    [SerializeField]
    int AttackDistanceMin, AttackDistanceMax;
    /*
    public override void Battle(Unit victim)
    {
        Attack(victim);
        CheckNextRank();
        if (!victim.IsDead)
        {
            victim.CheckNextRank();
        }
    }*/
}
