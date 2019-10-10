using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitManager : Manager
{
    public void AddUnit(Player player, int id, HexCell cell, float orientation)
    {
        Unit unit = Object.Instantiate(player.setup.GetUnit(id), grid.transform, false);
        unit.Owner = player.ID;
        player.AddUnit(ref unit);
        unit.Location = cell;
        unit.Orientation = orientation;
    }

    public void AddBoughtUnit(Player player, int id, HexCell castle, float orientation)
    {
        Unit unit = Object.Instantiate(player.setup.GetUnit(id), grid.transform, false);
        unit.Owner = player.ID;
        player.AddUnit(ref unit);
        unit.Orientation = orientation;
        if (castle.Unit)
        {
            unit.SetLocationQuiet(castle);
            player.ui.MovementInt(ref unit);
        }
        else
        {
            unit.Location = castle;
            player.ui.SelectedUnit = unit;
        }
    }

    public void SpawnCommander(Player player, HexCell cell)
    {
        Unit commander = player.Commander;
        commander.Location = cell;
        commander.Health = 100;
        commander.gameObject.SetActive(true);
    }

    public static void RemoveUnit(ref Unit unit)
    {
        unit.Die();
    }
}
