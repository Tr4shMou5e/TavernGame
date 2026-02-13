using UnityEngine;
using UnityEngine.UI;
public class ActionProgressUI : MonoBehaviour
{
    public Slider ActionBar;
    float ActionTime;
    float timer;
    bool isPerforming;

    public void StartAction(float duration)
    {
        ActionTime = duration;
        timer = 0f;
        isPerforming = true;
        ActionBar.gameObject.SetActive(true);   
    }

    void Update()
    {
        if (!isPerforming) return;
        timer += Time.deltaTime;
        ActionBar.value = timer / ActionTime;

        if (timer >= ActionTime)
        {
            isPerforming = false;
            ActionBar.gameObject.SetActive(false);
        }      
    }
}
