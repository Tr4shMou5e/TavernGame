using UnityEngine;
using UnityEngine.EventSystems;

public class PantryItemUI : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    public string itemName;

    public GameObject highlight;
    public GameObject icon;

    private bool taken = false;

    public void OnPointerClick(PointerEventData eventData)
    {
        if (taken)
        {
            Debug.Log("Already took " + itemName);
            return;
        }

        taken = true;

        Debug.Log("Took: " + itemName);

        // TODO: Add to inventory later

    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!taken)
            highlight.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        highlight.SetActive(false);
    }
}
