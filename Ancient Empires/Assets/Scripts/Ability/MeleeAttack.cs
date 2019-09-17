using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Melee Attack Ability", menuName = "Melee Attack Ability", order = 55)]
public class MeleeAttack : Ability
{
    public override bool IsUsable()
    {
        HexCell n;
        for (HexDirection d = HexDirection.NE; d<= HexDirection.NW; d++)
        {
            if ( (n = gui.currentCell.GetNeighbor(d)) && n.Unit )
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
            gui.AbilityCompleted();
        }
    }

    public override string ToString()
    {
        return "This unit can attack enemy in melee battle";
    }
}
