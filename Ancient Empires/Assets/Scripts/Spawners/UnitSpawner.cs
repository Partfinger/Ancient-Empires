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

    public UnitSpawner(BinaryReader reader)
    {
        indexCell = reader.ReadInt32();
        owner = reader.ReadInt32();
        unitID = reader.ReadInt32();
        health = reader.ReadInt32();
        rank = reader.ReadInt32();
        experience = reader.ReadInt32();
    }

    public UnitSpawner()
    {

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
