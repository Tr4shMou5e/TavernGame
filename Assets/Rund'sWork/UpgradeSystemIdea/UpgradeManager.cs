using UnityEngine;
using System.Collections.Generic;

public class UpgradeManager : MonoBehaviour
{
    public static UpgradeManager Instance;

    public List<UpgradeData> allDecorations;

    private void Awake()
    {
        Instance = this;

        foreach (var deco in allDecorations)
        {
            deco.unlocked = false;
        }
    }

    public float GetTotalPriceMultiplier()
    {
        float total = 1f;

        foreach (var deco in allDecorations)
        {
            if (deco.unlocked)
                total += deco.priceMultiplier;
        }

        return total;
    }
}
