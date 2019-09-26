using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapManager : ScriptableObject
{
    public bool IsSpecific;
    public string Name;
    public string Desc;
    public int EditorVers;
    public int maxPlayers;
    public List<Vector2Int> spawnPositions;
    public Image miniMap;
    public string Path;
}
