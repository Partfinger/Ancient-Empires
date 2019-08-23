using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedUnit : Unit
{
    [SerializeField]
    int AttackDistanceMin, AttackDistanceMax;

    public override void Battle(Unit attacking, Unit victim)
    {
        attacking.Attack(victim);
        victim.CheckIsDead();
    }

    public override HexCell[] GetAttackCells()
    {
        throw new System.NotImplementedException();
    }
}
