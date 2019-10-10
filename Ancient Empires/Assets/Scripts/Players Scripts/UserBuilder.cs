using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UserBuilder : PlayerBuilder
{

    public Text NameLabel;

    public override Player Construct()
    {
        Player p = new Player
        {
            fractionID = teamID,
            ID = ID
        };
        return p;
    }

    public override void SetName(string n)
    {
        Name = n;
        NameLabel.text = n;
    }
}
