using System.IO;

public class UnitSpawner
{
    public int unitID, owner, health, experience, rank, indexCell;

    public UnitSpawner(int unitID, int owner, int health, int experience, int rank, int indexCell)
    {
        this.unitID = unitID;
        this.owner = owner;
        this.health = health;
        this.experience = experience;
        this.rank = rank;
        this.indexCell = indexCell;
    }

    public void Save(BinaryWriter writer)
    {
        writer.Write(indexCell);
        writer.Write(owner);
        writer.Write(unitID);
        writer.Write(health);
        writer.Write(rank);
        writer.Write(experience);
    }
}
