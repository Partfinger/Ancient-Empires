using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingSpawner : MonoBehaviour
{
    public int buildingID, owner, number, status, indexCell;

    public static HexFeatureManager featureManager;

    public void SpawnBuilding(int buildID, int ownerID, int num, int stat, int index  )
    {
        if (Partie.players.Count < ownerID + 1)
        {
            return;
        }
        else
        {
            if (buildID > 1)
            {
                featureManager.AddMoreBuilding(index, num, stat);
            }
        }
    }
}
