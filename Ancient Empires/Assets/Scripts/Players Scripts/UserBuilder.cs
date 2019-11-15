using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UserBuilder : PlayerBuilder
{

    public Text NameLabel;

    public override Player Construct()
    {
        return null;
    }

    public override void SetName(string n)
    {
        Name = n;
        NameLabel.text = n;
    }
}
