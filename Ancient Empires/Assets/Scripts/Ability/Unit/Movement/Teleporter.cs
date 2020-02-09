using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Teleporter", menuName = "Teleporter Ability", order = 70)]
public class Teleporter : Movement
{
    public override void Canceled()
    {
        fromCell.DisableHighlight();
    }

    public override bool IsUsable(Unit unit)
    {
        return true;
    }

    public override bool Selected(Unit unit)
    {
        fromCell = unit.Location;
        fromCell.EnableHighlight(Color.blue);
        return false;
    }

    public override bool Trigger(ref Unit unit, ref HexCell destination)
    {
        if (destination)
        {
            if (fromCell == destination)
                Debug.Log("You selected the same cell");
            else
            {
                if (!destination.Unit)
                    unit.Location = destination;
            }
            unit.Owner.ui.SelectUnitAfterMove();
            Canceled();
            return true;
        }
        else
        {
            Canceled();
            return false;
        }
    }
}
