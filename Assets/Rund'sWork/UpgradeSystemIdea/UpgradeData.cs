using UnityEngine;

[CreateAssetMenu(menuName = "Tavern/Decoration")]
public class UpgradeData : ScriptableObject
{
    public string decorationName;
    public GameObject prefab;   

    public float priceMultiplier;  // affects ALL menu items

    public int cost; // how much player pays to unlock

    public bool unlocked;
}
