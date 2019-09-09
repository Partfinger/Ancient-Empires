using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeUnit : Unit
{
    public override void Battle(Unit victim)
    {
        Attack(victim);
        if (!victim.unitData.IsDead)
        {
            victim.Attack(this);
            if (!unitData.IsDead)
                unitData.CheckNextRank();
            victim.unitData.CheckNextRank();
        }
        else
        {
            unitData.CheckNextRank();
        }
    }

    public override HexCell[] GetAttackCells()
    {
        List<HexCell> result = new List<HexCell>();
        for(HexDirection d = HexDirection.NE; d <= HexDirection.NW; d++)
        {
            result.Add(Location.GetNeighbor(d));
        }
        return result.ToArray();
    }
}
