using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class HexInitiator : MonoBehaviour
{
    public HexGameGrid grid;
    public HexGameUI ui;
    public UnitManager unitManager;
    public PartieManager manager;

    private void Start()
    {

        manager.map = PartieBridge.manager;
        manager.MakePlayers(PartieBridge.players);
        if (PartieBridge.manager == null)
            grid.Initialize(manager.map);
        else
        {
            using (BinaryReader reader = new BinaryReader(File.OpenRead(manager.map.toData)))
            {
                grid.Initialize(manager.map, reader);
                Load(reader);
            }
            ui.Player = manager.players[0];
        }
        Destroy(gameObject);
    }

    public void Load(BinaryReader reader)
    {
        LoadBuildingSpawner(reader);
        LoadUnitSpawner(reader);
    }

    private void LoadBuildingSpawner(BinaryReader reader)
    {
        int count = reader.ReadInt32();
        BuildingSpawner spawner;
        HexBuilding building;
        HexCell cell;
        for (int i = 0; i < count; i++)
        {
            spawner = new BuildingSpawner(reader);
            cell = grid.GetCell(spawner.indexCell);
            if (cell.ActiveBuilding == BuildingType.village)
                continue;
            Debug.Log(spawner.indexCell);
            building = cell.Building;
            building.Owner = manager.players[spawner.owner];
            building.IsCaption = spawner.isCaptured;
        }
    }

    public void LoadUnitSpawner(BinaryReader reader)
    {
        int unitCount = reader.ReadInt32();
        for (int i = 0; i < unitCount; i++)
        {
            UnitSpawner newUnitSpawner = new UnitSpawner(reader);
            if (newUnitSpawner.owner < manager.players.Count && manager.players[newUnitSpawner.owner])
            {
                unitManager.AddUnit(newUnitSpawner);
            }
        }
    }
}
