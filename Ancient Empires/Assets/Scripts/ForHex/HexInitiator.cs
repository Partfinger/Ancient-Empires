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
    public RectTransform UnitMarketContent;
    public RectTransform Market;
    public UnitMarketLogic marketLogic;

    private void Start()
    {
        if (PartieBridge.manager == null)
        {
            MapManager.UpdatePath();
            manager.map = new MapManager(manager.map.Name);
        }
        else
            manager.map = PartieBridge.manager;
        if (PartieBridge.players.Count == 0)
        {
            for (int i = 0; i < manager.map.MaxPlayers; i++)
            {
                PlayerSpawner p = new PlayerSpawner();
                p.ID = i + 1;
                PartieBridge.players.Add(p);
            }
        }
        manager.MakePlayers(PartieBridge.players);
        using (BinaryReader reader = new BinaryReader(File.OpenRead(manager.map.toData)))
        {
            grid.Initialize(manager.map, reader);
            Load(reader);
        }
        ui.Player = manager.players[0];
        UnitMarketLogic.ui = ui;
        int length = unitManager.unitsArray.Length;
        if (length < 4)
        {
            Market.sizeDelta = new Vector2(95 * length + 15, 170);
        }
        else
        {
            Market.sizeDelta = new Vector2(395, 170);
        }
        for (int i = 0; i < length; i++)
        {
            UnitMarketLogic unitMarket = Instantiate(marketLogic, UnitMarketContent.transform);
            unitMarket.SetUnit(unitManager.unitsArray.GetUnit(i));
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
            building = cell.Building;
            if (spawner.owner > 0)
                building.Owner = manager.players[spawner.owner - 1];
            building.IsCaption = spawner.isCaptured;
        }
    }

    public void LoadUnitSpawner(BinaryReader reader)
    {
        int unitCount = reader.ReadInt32();
        for (int i = 0; i < unitCount; i++)
        {
            UnitSpawner newUnitSpawner = new UnitSpawner(reader);
            newUnitSpawner.owner -= 1;
            if (newUnitSpawner.owner < manager.players.Count && manager.players[newUnitSpawner.owner])
            {
                unitManager.AddUnit(newUnitSpawner);
            }
        }
    }
}
