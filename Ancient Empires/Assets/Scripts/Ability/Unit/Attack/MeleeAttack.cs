using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Melee Attack Ability", menuName = "Melee Attack Ability", order = 61)]
public class MeleeAttack : AttackAbility
{
    public override bool IsUsable(Unit unit)
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

    public override bool Selected(Unit unit)
    {
        HexCell location = unit.Location;
        HexCell n;
        for (HexDirection d = HexDirection.NE; d <= HexDirection.NW; d++)
        {
            if ((n = location.GetNeighbor(d)) && n.Unit && !n.Unit.Owner.IsAlly(unit.Owner))
            {
                n.EnableHighlight(Color.red);
                attackSpace.Add(n.Index);
                n.WasChecked = true;
            }
        }
        return false;
    }

    public override void StrikeBack(Unit attaker, Unit victim, AttackType attackType)
    {
        if (attackType == 0)
        {
            Strike(victim, attaker);
        }
    }
}
