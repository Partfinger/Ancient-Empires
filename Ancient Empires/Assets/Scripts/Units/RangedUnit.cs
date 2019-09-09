using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedUnit : Unit
{
    [SerializeField]
    int AttackDistanceMin, AttackDistanceMax;

    public override void Battle(Unit victim)
    {
        Attack(victim);
        unitData.CheckNextRank();
        if (!victim.unitData.IsDead)
        {
            victim.unitData.CheckNextRank();
        }
    }

    public override HexCell[] GetAttackCells()
    {
        throw new System.NotImplementedException();
    }
}
