using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Melee Attack Ability", menuName = "Melee Attack Ability", order = 55)]
public class MeleeAttack : UnitAbility
{
    public override bool IsUsable(ref Unit unit)
    {
        HexCell location = unit.Location;
        HexCell n;
        for (HexDirection d = HexDirection.NE; d<= HexDirection.NW; d++)
        {
            if ( (n = location.GetNeighbor(d)) && n.Unit && !n.Unit.Owner.IsAlly(unit.Owner))
            {
                return true;
            }
        }
        return false;
    }

    public override void TriggerAbility(ref Unit unit, ref HexCell cell)
    {
        Unit victim = cell.Unit;
        if (victim)
        {
            unit.Attack(victim);
            if (!victim.IsDead)
            {
                victim.Attack(unit);
                if (!unit.IsDead)
                {
                    unit.CheckNextRank();
                    unit.enabled = true;
                }
                else
                    unit.Die();
                victim.CheckNextRank();
            }
            else
            {
                victim.Die();
                unit.CheckNextRank();
                unit.enabled = true;
            }
        }
    }

    public override string ToString()
    {
        return "This unit can attack enemy in melee battle";
    }
}
