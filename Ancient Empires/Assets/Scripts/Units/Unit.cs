using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Unit : MonoBehaviour
{
    public Player Owner;
    public static HexGameUI gui;
    public UnitData unitData;
    public Buff buff = new Buff();
    List<HexCell> pathToTravel;
    const float travelSpeed = 7f;
    const float rotationSpeed = 180f;
    const float coldDownTime = 10f;
    float coldDown = 0f;

    [SerializeField]
    protected int unitID, health;

    protected int offenceMin, offenceMax, defence, mobility, experience, nextRankExperience, healthMax = 100, rank = 0;

    [SerializeField]
    AbilityType[] abilities;

    float orientation;
    [SerializeField]
    protected HexCell location;

    private void Awake()
    {
        offenceMin = unitData.BaseOffenceMin;
        offenceMax = unitData.BaseOffenceMax;
        defence = unitData.BaseDefence;
        mobility = unitData.BaseMobility;
        health = healthMax * 1;
        GetNextRankExperience();
        enabled = false;
    }

    private void Update()
    {
        if (coldDown > coldDownTime)
        {
            coldDown = 0f;
            enabled = false;
            return;
        }
        coldDown += Time.deltaTime;
    }

    public void SetLocation(ref HexCell l)
    {
        location = l;
        transform.localPosition = l.Position;
    }

    public AbilityType GetOnlyMove()
    {
        return abilities[1];
    }

    public int ID
    {
        get
        {
            return unitID;
        }
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
            location.Unit = this;
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

    public int Rank
    {
        get
        {
            return rank;
        }
        set
        {
            rank = value;
            RecountStats();
        }
    }

    public int Offence
    {
        get
        {
            return Random.Range(offenceMin, offenceMax) + buff.Offence;
        }
    }

    public int Defence
    {
        get
        {
            return defence + buff.Defence;
        }
    }

    public int Speed
    {
        get
        {
            return mobility + buff.Mobility;
        }
    }

    public int Health
    {
        get
        {
            return health;
        }
        set
        {
            health = value;
        }
    }

    public int QualitySum
    {
        get { return offenceMin + offenceMax + defence; }
    }

    public AbilityType[] GetAbilities()
    {
        return abilities;
    }

    public void ValidateLocation()
    {
        transform.localPosition = location.Position;
    }

    public void SetLocationQuiet(HexCell value)
    {
        location = value;
        transform.localPosition = value.Position;
    }

    void TravelLok(HexCell value)
    {
        location = value;
        location.Unit = this;
        transform.localPosition = value.Position;
    }

    public void Travel(List<HexCell> path)
    {
        if (location.Unit != this)
            TravelLok(path[path.Count - 1]);
        else
            Location = path[path.Count - 1];
        pathToTravel = path;
        gui.WaitForEndOfTravel();
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
        gui.SelectUnitAfterMove();
    }

    public virtual void Attack(Unit victim)
    {
        int hit = (Offence - victim.Defence) * health / 100;

        if (hit < 0)
        {
            hit = 0;
        }
        else if (hit > victim.health)
        {
            hit = victim.health;
        }
        victim.Hit(hit);
        AddExp(hit * victim.QualitySum);
    }

    protected void RecountStats()
    {
        int bonus = rank << 1;

        offenceMin = unitData.BaseOffenceMin + bonus;
        offenceMax = unitData.BaseOffenceMax + bonus;
        defence = unitData.BaseDefence + bonus;
        mobility = unitData.BaseMobility + ( 10 * rank ) / 6;

        GetNextRankExperience();

        if (rank < 11 && (rank & 1) == 0)
            name = unitData.Name(rank >> 1);
    }

    public void CheckNextRank()
    {
        while (experience >= nextRankExperience)
        {
            rank++;
            experience -= nextRankExperience;
            RecountStats();
        }
    }

    void GetNextRankExperience()
    {
        nextRankExperience = QualitySum * 66;
    }

    public void AddExp(int exp)
    {
        experience += exp;
    }

    public virtual void Die()
    {
        location.Unit = null;
        Owner.RemoveUnit(this);
        Destroy(gameObject);
    }

    /// <summary>
    /// Only clear map!!!!!
    /// </summary>
    public void Remove()
    {
        Destroy(gameObject);
    }

    public bool IsDead
    {
        get
        {
            return health == 0;
        }
    }

    public void Hit(int hit)
    {
        health -= hit;
    }

    public virtual bool IsValidDestination(HexCell cell)
    {
        return !cell.IsUnderwater && !cell.Unit;
    }

    public void Save(BinaryWriter writer)
    {
        writer.Write(unitID);
        writer.Write(health);
        writer.Write(experience);
        writer.Write(rank);

        location.coordinates.Save(writer);
        writer.Write(orientation);
    }

    public void Load(BinaryReader reader, HexGrid grid)
    {
        health = reader.ReadInt32();
        experience = reader.ReadInt32();
        rank = reader.ReadInt32();

        Location = grid.GetCell(HexCoordinates.Load(reader));
        Orientation = reader.ReadSingle();

        RecountStats();
    }
}
