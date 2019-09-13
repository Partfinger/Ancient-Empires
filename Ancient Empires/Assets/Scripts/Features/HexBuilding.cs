using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class HexBuilding : MonoBehaviour
{
    [SerializeField]
    protected bool isCaption;

    [SerializeField]
    protected int owner;

    public HexCell cell;

    public bool IsCaption { get { return IsCaption; } }

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

    public virtual int GetUserAction()
    {
        return 0;
    }

    protected abstract void ApplyBuff();

    public void AppointCell(ref HexCell c)
    {
        cell = c;
        ApplyBuff();
    }
}
