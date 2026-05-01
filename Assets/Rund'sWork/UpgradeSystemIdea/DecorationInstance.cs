using UnityEngine;

public class DecorationInstance : MonoBehaviour
{
    public UpgradeData data;

    void Start()
    {
        
        gameObject.SetActive(data.unlocked);
    }

    public void Refresh()
    {
        gameObject.SetActive(data.unlocked);
    }
}
