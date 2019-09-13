using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Castle Capture Ability", menuName = "Castle Capture Ability", order = 53)]
public class CastleCapture : Ability
{
    public override bool IsUsable(ref HexCell location)
    {
        if (location.Building)
        {
            return true;
        }
        return false;
    }

    public override void Canceled()
    {
        throw new System.NotImplementedException();
    }

    public override void Selected(ref Unit unit)
    {
        unit.Location.Building.Owner = 2;
    }

    public override string ToString()
    {
        throw new System.NotImplementedException();
    }
}
