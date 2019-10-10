using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerBuilder : MonoBehaviour
{
    protected int teamID = -1;
    protected Color color;
    protected int ID;
    protected string Name;

    public void SetID(int id)
    {
        ID = id;
    }

    public void SetTeam(int id)
    {
        teamID = id;
    }

    public void SetColor(Color c)
    {
        color = c;
    }

    public virtual void SetName(string n)
    {
        Name = n;
    }

    public void MakeCurrentPlayer()
    {
        //GetComponent<>
    }

    public abstract Player Construct();
}
