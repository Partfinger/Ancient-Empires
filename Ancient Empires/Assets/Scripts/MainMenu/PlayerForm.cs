using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerForm : MonoBehaviour
{
    public PlayerSpawner data = new PlayerSpawner();
    /// <summary>
    /// -2 залочена, -1 свободная, 0 занята, 1 изи, 2 мид, 3 хард
    /// </summary>
    sbyte Status = -1;
    public Dropdown dropDownType;
    public Button playerName;
    public Text textName;

    public void SetID(int id)
    {
        data.ID = id;
    }

    public void SetTeam(int id)
    {
        data.teamID = id;
    }

    public void SetStatus(sbyte newStatus)
    {
        Status = newStatus;
    }

    public void SetColor(int id)
    {
        data.colorID = id;
    }

    public virtual void SetName(string n)
    {
        data.Name = n;
        dropDownType.gameObject.SetActive(false);
        playerName.gameObject.SetActive(true);
        textName.text = n;
    }

    public PlayerSpawner GetSpawner()
    {
        return data;
    }
}
