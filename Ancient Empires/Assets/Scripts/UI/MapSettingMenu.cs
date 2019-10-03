using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapSettingMenu : MonoBehaviour
{
    public HexMapEditor editor;
    [SerializeField]
    Text name, desc;
    [SerializeField]
    Slider slider;

    public void Apply()
    {
        editor.ActiveMap.Name = name.text;
        editor.ActiveMap.Desc = desc.text;
        editor.ActiveMap.MaxPlayers = (int)slider.value;
        Close();
    }

    public void UpdateText()
    {
        name.text = editor.ActiveMap.Name;
        desc.text = editor.ActiveMap.Desc;
        slider.value = editor.ActiveMap.MaxPlayers;
    }

    public void Open()
    {
        gameObject.SetActive(true);
        HexMapCamera.Locked = true;
        UpdateText();
    }

    public void Close()
    {
        gameObject.SetActive(false);
        HexMapCamera.Locked = false;
    }

    public void Cancel()
    {
        UpdateText();
        Close();
    }
}
