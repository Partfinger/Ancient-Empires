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
    public static string path;
    [NonSerialized]
    public string toHead, toData, toMini;
    string hash;

    public static void UpdatePath()
    {
        path = Path.Combine(Application.persistentDataPath, "Levels/");
        Debug.Log(path);
    }

    public MapManager(string name)
    {
        toHead = path + name + ".mapd";
        toData = Path.Combine(path, $"LevelsData/{name}.bin");
        toMini = Path.Combine(path, $"LevelsMinimap/{name}.png");
        string json = File.ReadAllText(toHead);
        JsonUtility.FromJsonOverwrite(json, this);
    }

    public MapManager()
    {
    }

    public static MapManager GetFirstMap()
    {
        string[] paths =
            Directory.GetFiles(path, "*.mapd");
        return new MapManager(Path.GetFileNameWithoutExtension(paths[0]));
    }

    public void GetMinimap(ref UnityEngine.UI.Image minimap)
    {
        Texture2D mini = new Texture2D(512,512);
        mini.LoadImage(File.ReadAllBytes(toMini));
        minimap.sprite = Sprite.Create(mini, new Rect(0,0, mini.width, mini.height),Vector2.zero);
    }

    public void Save()
    {
        File.WriteAllText(toHead, JsonUtility.ToJson(this));

    }

    public void Delete()
    {
        File.Delete(path + Name + ".mapd");
    }
}
