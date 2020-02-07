using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class HexBuilding : MonoBehaviour
{
    [SerializeField]
    protected bool isCaption;
    [SerializeField]
    Player owner;

    [SerializeField]
    public Material material;

    [SerializeField]
    protected PlayerAbility ability;

    public HexCell cell;

    public Player Owner
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

    public bool IsCaption
    {
        get
        {
            return isCaption;
        }
        set
        {
            isCaption = value;
        }
    }

    protected abstract void ApplyBuff();

    public void AppointCell(ref HexCell c)
    {
        cell = c;
        ApplyBuff();
    }

    public void GetAbility(Player player, ref List<Ability> list)
    {
        if (ability && ability.IsUsable(player, this))
            list.Add( ability);
    }


}
