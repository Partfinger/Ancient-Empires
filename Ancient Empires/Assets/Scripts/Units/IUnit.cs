using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IUnit
{
    int Health { get; set; }
    int Offence { get; }
    int Defence { get; }

    void Move();

    void SetRank();
    
}
