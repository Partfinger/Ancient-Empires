﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitManager : Manager
{
    public UnitsArray unitsArray;

    public void AddUnit(Player player, int id, HexCell cell, float orientation = 0.0f)
    {
        Unit unit = Object.Instantiate(unitsArray.GetUnit(id), grid.transform, false);
        unit.Owner = player;
        player.AddUnit(ref unit);
        unit.Location = cell;
        unit.Orientation = orientation;
    }

    public void AddUnit(UnitSpawner spawner, float orientation = 0.0f)
    {
        Player player = manager.players[spawner.owner];
        Unit unit = Object.Instantiate(unitsArray.GetUnit(spawner.unitID), grid.transform, false);
        unit.Owner = player;
        unit.Health = spawner.health;
        unit.Rank = spawner.rank;
        unit.AddExp(spawner.experience);
        unit.Location = grid.GetCell(spawner.indexCell);
        unit.Orientation = orientation;
        player.AddUnit(ref unit);
        unit.RecountStats();
    }

    public void AddBoughtUnit(Player player, int id, HexCell castle, float orientation)
    {
        Unit unit = Object.Instantiate(unitsArray.GetUnit(id), grid.transform, false);
        unit.Owner = player;
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
