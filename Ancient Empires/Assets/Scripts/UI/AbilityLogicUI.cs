using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityLogicUI : MonoBehaviour
{
    [SerializeField]
    public static HexGameUI UI;
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
        //UI.SelectedAbility(ref ability);
    }
}
