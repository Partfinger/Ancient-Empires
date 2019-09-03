using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buff
{
    // 0 - a, 1 -d, 2 - m
    public int[] stats = new int[3];

    public Buff(int a =0, int d = 0, int m = 0)
    {
        stats[0] = a;
        stats[1] = d;
        stats[2] = m;
    }

    public void Revert()
    {
        stats[0] = 0;
        stats[1] = 0;
        stats[2] = 0;
    }

    public void AddDef(int d)
    {
        stats[1] += d;
    }

    public static Buff operator +(Buff b1, Buff b2)
    {
        return new Buff(b1.stats[0] + b2.stats[0], b1.stats[1] + b2.stats[1], b1.stats[2] + b2.stats[2]);
    }

    public static Buff operator -(Buff b1, Buff b2)
    {
        return new Buff(b1.stats[0] - b2.stats[0], b1.stats[1] - b2.stats[1], b1.stats[2] - b2.stats[2]);
    }

    public new string ToString()
    {
        return string.Format("{0} {1} {2}", stats[0], stats[1], stats[2]);
    }

    public static Buff Castle()
    {
        return new Buff(0, 25);
    }

    public static Buff AttackHeights(ref Unit unit)
    {
        return new Buff(-(unit.Offence / 4));
    }

    public static Buff River()
    {
        return new Buff();
    }
}
