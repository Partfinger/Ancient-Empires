using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogMenu : MonoBehaviour
{
    [SerializeField]
    Text question;

    static string[] questions = new string[]
    {
        "There is already a map with the given name. Change her?",
        "Do you want to delete this map?"
    };

    [SerializeField]
    SaveLoadMenu Menu;

    public void Show(int q)
    {
        question.text = questions[q];
        gameObject.SetActive(true);
    }

    public void Yes()
    {
        Menu.method();
        gameObject.SetActive(false);
    }

    public void No()
    {
        gameObject.SetActive(false);
    }
}
