using UnityEngine;
public abstract class Ability : ScriptableObject
{
    public static HexGrid grid;
    public Unit carrier;
    public string aName = "New Ability";
    public Sprite aSprite;
    //public AudioClip aSound;
    public float aBaseCoolDown = 1f;

    public abstract bool IsUsable { get; }

    public abstract void Initialize();
    public abstract void TriggerAbility();
}