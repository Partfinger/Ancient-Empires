using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Ranged Attack Ability", menuName = "Ranged Attack Ability", order = 51)]
public class RangeAttack : Ability
{
    public override bool IsUsable()
    {
        RangedUnitData data = gui.currentCell.Unit.unitData as RangedUnitData;
        int min = data.AttackDistanceMin - 1, max = data.AttackDistanceMax;

        HexCoordinates center = gui.currentCell.coordinates;
        HexCoordinates current;
        HexCell cell;

        int centerX = center.X;
        int centerZ = center.Z;

        for (int r = 0, z = centerZ - max; z <= centerZ; z++, r++)
        {
            for (int x = centerX - r; x <= centerX + max; x++)
            {
                current = new HexCoordinates(x, z);
                cell = grid.GetCell(current);
                if (cell && center.DistanceTo(current) > min && cell.Unit)
                {
                    return true;
                }
            }
        }

        for (int r = 0, z = centerZ + max; z > centerZ; z--, r++)
        {
            for (int x = centerX - max; x <= centerX + r; x++)
            {
                current = new HexCoordinates(x, z);
                cell = grid.GetCell(current);
                if (cell && center.DistanceTo(current) > min && cell.Unit)
                {
                    return true;
                }
            }
        }

        return false;
    }

    public override void TriggerAbility(ref Unit unit, ref HexCell cell)
    {
        Unit victim = cell.Unit;
        unit.Attack(victim);
        unit.CheckNextRank();
        if (!victim.IsDead)
            victim.CheckNextRank();
        else
            victim.Die();
        gui.AbilityCompleted();
    }

    public override string ToString()
    {
        return "This unit can attack enemy on distance";
    }

    public override void Selected()
    {
        return;
    }
}
