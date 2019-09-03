public static class Buffs
{
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

    public static Buff Brigde()
    {
        return new Buff(0, 10);
    }
}
