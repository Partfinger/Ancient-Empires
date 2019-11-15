[System.Serializable]
public class Buff
{
    // 0 - a, 1 -d, 2 - m
    int[] stats = new int[3];

    public Buff(int a =0, int d = 0, int m = 0)
    {
        stats[0] = a;
        stats[1] = d;
        stats[2] = m;
    }

    public int QualitySum
    {
        get
        {
            return stats[0] + stats[1];
        }
    }

    public int Offence
    {
        get
        {
            return stats[0];
        }
    }

    public int Defence
    {
        get
        {
            return stats[1];
        }
    }

    public int Mobility
    {
        get
        {
            return stats[2];
        }
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

    public override string ToString()
    {
        return string.Format("{0} {1} {2}", stats[0], stats[1], stats[2]);
    }

}
