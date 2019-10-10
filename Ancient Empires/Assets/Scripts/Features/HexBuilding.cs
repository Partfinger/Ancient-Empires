using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class HexBuilding : MonoBehaviour
{
    [SerializeField]
    protected bool isCaption;

    int owner = 0;

    [SerializeField]
    public Material material;

    public HexCell cell;

    public int Owner
    {
        get
        {
            return owner;
        }
        set
        {
            owner = value;
        }
    }

    public bool IsCaption { get { return IsCaption; } set { isCaption = true; } }

    protected abstract void ApplyBuff();

    public void AppointCell(ref HexCell c)
    {
        cell = c;
        ApplyBuff();
    }
}
