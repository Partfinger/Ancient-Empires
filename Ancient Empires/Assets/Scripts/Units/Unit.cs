using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public enum UnitActions
{
    empty, finish, move, attack, mend, capture
}

public abstract class Unit : MonoBehaviour
{
    public UnitData unitData;
    public Buff buff = new Buff();
    List<HexCell> pathToTravel;
    const float travelSpeed = 4f;
    const float rotationSpeed = 180f;

    float orientation;
    [SerializeField]
    HexCell location;

    private void Awake()
    {
        unitData.AwakeUnit();
    }

    void OnEnable()
    {
        if (location)
        {
            transform.localPosition = location.Position;
        }
    }

    public HexCell Location
    {
        get
        {
            return location;
        }
        set
        {
            if (location)
            {
                location.Unit = null;
            }
            location = value;
            value.Unit = this;
            transform.localPosition = value.Position;
        }
    }

    public float Orientation
    {
        get
        {
            return orientation;
        }
        set
        {
            orientation = value;
            transform.localRotation = Quaternion.Euler(0f, value, 0f);
        }
    }

    public int Offence
    {
        get
        {
            return unitData.Offence + buff.Offence;
        }
    }

    public int Defence
    {
        get
        {
            return unitData.Defence + buff.Defence;
        }
    }

    public int Speed
    {
        get
        {
            return unitData.Mobility + buff.Mobility;
        }
    }

    public int QualitySum
    {
        get
        {
            return unitData.QualitySum + buff.QualitySum;
        }
    }

    public void ValidateLocation()
    {
        transform.localPosition = location.Position;
    }

    public void Travel(List<HexCell> path)
    {
        Location = path[path.Count - 1];
        pathToTravel = path;
        StopAllCoroutines();
        StartCoroutine(TravelPath());
    }

    IEnumerator LookAt(Vector3 point)
    {
        point.y = transform.localPosition.y;
        Quaternion fromRotation = transform.localRotation;
        Quaternion toRotation =
            Quaternion.LookRotation(point - transform.localPosition);
        float angle = Quaternion.Angle(fromRotation, toRotation);
        if (angle > 0f)
        {
            float speed = rotationSpeed / angle;

            for (
                float t = Time.deltaTime * speed;
                t < 1f;
                t += Time.deltaTime * speed
            )
            {
                transform.localRotation =
                    Quaternion.Slerp(fromRotation, toRotation, t);
                yield return null;
            }

            transform.LookAt(point);
            orientation = transform.localRotation.eulerAngles.y;
        }
    }

    IEnumerator TravelPath()
    {
        Vector3 a, b, c = pathToTravel[0].Position;
        transform.localPosition = c;
        yield return LookAt(pathToTravel[1].Position);

        float t = Time.deltaTime * travelSpeed;
        for (int i = 1; i < pathToTravel.Count; i++)
        {
            a = c;
            b = pathToTravel[i - 1].Position;
            c = (b + pathToTravel[i].Position) * 0.5f;
            for (; t < 1f; t += Time.deltaTime * travelSpeed)
            {
                transform.localPosition = Bezier.GetPoint(a, b, c, t);
                Vector3 d = Bezier.GetDerivative(a, b, c, t);
                d.y = 0f;
                transform.localRotation = Quaternion.LookRotation(d);
                yield return null;
            }
            t -= 1f;
        }

        a = c;
        b = pathToTravel[pathToTravel.Count - 1].Position;
        c = b;
        for (; t < 1f; t += Time.deltaTime * travelSpeed)
        {
            transform.localPosition = Bezier.GetPoint(a, b, c, t);
            Vector3 d = Bezier.GetDerivative(a, b, c, t);
            d.y = 0f;
            transform.localRotation = Quaternion.LookRotation(d);
            yield return null;
        }
        transform.localPosition = location.Position;
        orientation = transform.localRotation.eulerAngles.y;

        ListPool<HexCell>.Add(pathToTravel);
        pathToTravel = null;
    }

    public void GetMotionSpace()
    {

    }

    public void Move(HexCell[] path)
    {

    }

    public virtual void Attack(Unit victim)
    {
        int hit = (Offence - victim.Defence) * unitData.Health / 100;

        if (hit < 0)
        {
            hit = 0;
        }
        else if (hit > victim.unitData.Health)
        {
            hit = victim.unitData.Health;
        }
        victim.Hit(hit);
        unitData.AddExp(hit * victim.QualitySum);
    }

    

    public void Die()
    {
        location.Unit = null;
        Destroy(gameObject);
    }

    public void Hit(int hit)
    {
        unitData.Health -= hit;
    }

    public abstract void Battle(Unit victim);

    public abstract HexCell[] GetAttackCells();

    public virtual UnitActions Skill()
    {
        return 0;
    }

    public virtual bool IsValidDestination(HexCell cell)
    {
        return !cell.IsUnderwater && !cell.Unit;
    }

    public void Save(BinaryWriter writer)
    {
        location.coordinates.Save(writer);
        writer.Write(orientation);
    }

    public void Load(BinaryReader reader, HexGrid grid)
    {
        Location = grid.GetCell(HexCoordinates.Load(reader));
        Orientation = reader.ReadSingle();
    }
}
