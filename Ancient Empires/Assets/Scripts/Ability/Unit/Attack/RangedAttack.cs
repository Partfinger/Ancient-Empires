using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Ranged Attack Ability", menuName = "Ranged Attack Ability", order = 62)]
public class RangeAttack : UnitAbility
{

    [SerializeField] byte minRange, maxRange;

    public override bool IsUsable(Unit unit)
    {
        HexCell location = unit.Location;
        int centerX = location.coordinates.X;
        int centerZ = location.coordinates.Z;

        for (int r = 0, z = centerZ - brushSize; z <= centerZ; z++, r++)
        {
            for (int x = centerX - r; x <= centerX + brushSize; x++)
            {
                EditCell(hexGrid.GetCell(new HexCoordinates(x, z)));
            }
        }
        for (int r = 0, z = centerZ + brushSize; z > centerZ; z--, r++)
        {
            for (int x = centerX - brushSize; x <= centerX + r; x++)
            {
                EditCell(hexGrid.GetCell(new HexCoordinates(x, z)));
            }
        }
    }

    public override bool TriggerAbility(Unit unit, HexCell cell)
    {
        /*
        Unit victim = cell.Unit;
        //unit.Attack(victim);
        unit.CheckNextRank();
        if (!victim.IsDead)
            victim.CheckNextRank();
        else
            victim.Die();
        unit.enabled = true;*/
        return false;
    }
    /*
    public override string ToString()
    {
        return "This unit can attack enemy on distance";
    }*/
}
