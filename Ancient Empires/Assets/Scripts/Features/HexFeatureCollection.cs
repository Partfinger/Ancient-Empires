using UnityEngine;

[System.Serializable]
public struct HexFeatureCollection
{

    public Transform[] prefabs;

    public Transform Pick(float choice)
    {
        return prefabs[(int)(choice * prefabs.Length)];
    }

    public Transform Pick(int choice)
    {
        return prefabs[choice];
    }
}