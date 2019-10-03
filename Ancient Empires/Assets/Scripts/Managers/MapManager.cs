using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[Serializable]
public class MapManager
{
    public bool IsSpecific;
    public string Name = "", Desc = "Map desc";
    public int EditorVer, MaxPlayers, X,Z;
    public List<Vector2Int> SpawnPositions;
    public float mapVer = 1.0f;
    static string path;
    string hash;

    public static string GetPath()
    {
        return path;
    }

    public static void UpdatePath()
    {
        path = Path.Combine(Application.persistentDataPath, "Levels/");
        Debug.Log(path);
    }

    public MapManager(string name)
    {
        string json = File.ReadAllText(path + name + ".mapd");
        JsonUtility.FromJsonOverwrite(json, this);
    }

    public static MapManager ApplyJSON(string name)
    {
        string json = File.ReadAllText(path + name + ".mapd");
        MapManager manager = new MapManager();
        JsonUtility.FromJsonOverwrite(json, manager);
        return manager;
    }

    public MapManager()
    {
    }

    public void Save()
    {
        File.WriteAllText(path + Name + ".mapd", JsonUtility.ToJson(this));
    }

    public void Delete()
    {
        File.Delete(path + Name + ".mapd");
    }
}
