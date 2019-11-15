using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotBuilder : PlayerBuilder
{
    int botID;
    public override Player Construct()
    {
        throw new System.NotImplementedException();
    }

    public void SetBotID(int newID)
    {
        botID = newID - 2;
    }
}
