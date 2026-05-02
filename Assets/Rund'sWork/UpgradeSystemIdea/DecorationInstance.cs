using System.Collections.Generic;
using UnityEngine;

public class DecorationInstance : MonoBehaviour
{
    [SerializeField] private List<GameObject> decorations;
    public void Refresh(UpgradeData upgrade)
    {
        Debug.Log("Refreshing Decoration: " + upgrade.decorationName);
        foreach(var g in decorations)
        {
            Debug.Log("Refreshing Decoration: " + g.name);
            if(g.name == upgrade.decorationName)
                g.SetActive(true);
        }
        upgrade.unlocked = true;
    }
}
