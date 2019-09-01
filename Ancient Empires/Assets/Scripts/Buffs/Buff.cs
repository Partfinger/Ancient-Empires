using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Buff : MonoBehaviour
{
    public int buff;

    public abstract void Apply(Unit unit);
    public abstract void Cancel(Unit unit);
}
