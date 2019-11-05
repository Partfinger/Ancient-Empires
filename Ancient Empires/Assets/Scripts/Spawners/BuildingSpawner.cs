using System.IO;

public class BuildingSpawner
{
    public int owner, indexCell;
    public bool isCaptured;

    public BuildingSpawner(int ow, int index, bool cpt)
    {
        owner = ow;
        indexCell = index;
        isCaptured = cpt;
    }

    public BuildingSpawner(BinaryReader reader)
    {
        owner = reader.ReadInt32();
        indexCell = reader.ReadInt32();
        isCaptured = reader.ReadBoolean();
    }

    public void Save(BinaryWriter writer)
    {
        writer.Write(owner);
        writer.Write(indexCell);
        writer.Write(isCaptured);
    }
}
