using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Player : MonoBehaviour
{
    new string name = "Root";
    public string Name
    {
        get
        {
            return name;
        }
    }
}
