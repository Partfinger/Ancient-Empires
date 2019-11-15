using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityLogicUI : MonoBehaviour
{
    public static HexGameUI UI;
    [SerializeField]
    Ability ability;

    public Ability Ability
    {
        set
        {
            ability = value;
            image.sprite = value.Sprite;
        }
    }
    public UnityEngine.UI.Image image;

    public void OnClick()
    {
        UI.SelectedAbility = ability;
    }
}
