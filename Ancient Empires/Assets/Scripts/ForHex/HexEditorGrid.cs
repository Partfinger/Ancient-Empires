using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class HexEditorGrid : HexGrid
{
    public HexMapEditor editor;

    public void OldInitialize(MapManager manager, BinaryReader reader)
    {
        Initialize(manager);
        OldLoad(reader);
    }

    public void Save(BinaryWriter writer)
    {
        for (int i = 0; i < cells.Length; i++)
        {
            cells[i].Save(writer);
        }
    }

    public void OldLoad(BinaryReader reader)
    {
        reader.ReadInt32();
        reader.ReadInt32();
        for (int i = 0; i < cells.Length; i++)
        {
            cells[i].Load(reader);
            if (cells[i].IsBuilding)
            {
                BuildingSpawner spawner = new BuildingSpawner(0, cells[i].Index, true);
                editor.stuffEditor.playerBuildings.Add(cells[i].Index, spawner);
            }
        }

        
    
        for (int i = 0; i < chunks.Length; i++)
        {
            chunks[i].Refresh();
        }
    }
}
