using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Castle Capture Ability", menuName = "Castle Capture Ability", order = 101)]
public class CastleCapture : Ability
{
    public override void Initialize()
    {
        carrier.Location.Building.Owner = 2;
    }

    public override bool TriggerAbility()
    {
        HexCell cell = carrier.Location;
        if (cell.IsBuilding && cell.ActiveBuilding == 1 )
        {
            return true;
        }
        return false;
    }
}
