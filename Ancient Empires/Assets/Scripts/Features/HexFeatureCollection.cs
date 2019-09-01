using UnityEngine;

[System.Serializable]
public struct HexFeatureCollection
{

    public HexFeature[] prefabs;

    public HexFeature Pick(float choice)
    {
        return prefabs[(int)(choice * prefabs.Length)];
    }
}