using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Buff : MonoBehaviour
{
    public Unit unit;
    public int buff;

    public abstract void Apply();
    public abstract void Cancel();
}
