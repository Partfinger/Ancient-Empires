using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Melee Attack Ability", menuName = "Melee Attack Ability", order = 55)]
public class MeleeAttack : Ability
{
    public override bool IsUsable(ref HexCell l)
    {
        for (HexDirection d = HexDirection.NE; d<= HexDirection.NW; d++)
        {
            HexCell n;
            if ( (n = l.GetNeighbor(d)) && n.Unit )
            {
                return true;
            }
        }
        return false;
    }

    public override void TriggerAbility(ref Unit unit, ref HexCell cell)
    {
        Unit victim = cell.Unit;
        unit.Attack(victim);
        if (!victim.IsDead)
        {
            victim.Attack(unit);
            if (!unit.IsDead)
                unit.CheckNextRank();
            else
                unit.Die();
            victim.CheckNextRank();
        }
        else
        {
            victim.Die();
            unit.CheckNextRank();
        }
    }

    public override void Canceled()
    {
        throw new System.NotImplementedException();
    }

    public override string ToString()
    {
        throw new System.NotImplementedException();
    }
}
