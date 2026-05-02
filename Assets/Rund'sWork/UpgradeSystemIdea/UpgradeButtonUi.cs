using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UpgradeButtonUI : MonoBehaviour
{
    public UpgradeData data;

    public Button button;
    public TextMeshProUGUI label;

    void Start()
    {
        UpdateUI();

        button.onClick.AddListener(OnClick);
    }

    void OnClick()
    {
        if (data.unlocked) return;

        data.unlocked = true;

        UpdateUI();
        RefreshDecorations();
    }

    void UpdateUI()
    {
        button.interactable = !data.unlocked;

        label.text = data.unlocked
            ? "Owned"
            : "$" + data.cost;
    }

    void RefreshDecorations()
    {
        foreach (var deco in FindObjectsOfType<DecorationInstance>())
        {
            deco.Refresh(data);
        }
    }
}
