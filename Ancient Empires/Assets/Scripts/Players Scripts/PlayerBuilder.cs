using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBuilder : MonoBehaviour
{
    int teamID = -1;
    Color color;
    string name;
    int botID;

    public void SetTeam(int id)
    {
        teamID = id;
    }

    public void SetColor(Color c)
    {
        color = c;
    }

    public void SetName(string n)
    {
        name = n;
    }

    public Player Construct()
    {
        if (botID > 0)
        {
            return new AIPlayer();
        }
        else
        {
            return new Player();
        }
    }
}
