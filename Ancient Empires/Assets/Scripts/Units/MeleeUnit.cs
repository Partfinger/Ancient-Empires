using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeUnit : Unit
{
    public override void Battle(Unit attacking, Unit victim)
    {
        attacking.Attack(victim);
        if (victim.CheckIsDead())
            victim.Attack(attacking);
        attacking.CheckIsDead();
    }

    public override HexCell[] GetAttackCells()
    {
        List<HexCell> result = new List<HexCell>();
        for(HexDirection d = HexDirection.NE; d <= HexDirection.NW; d++)
        {
            result.Add(cell.GetNeighbor(d));
        }
        return result.ToArray();
    }
}
