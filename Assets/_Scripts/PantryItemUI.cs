using UnityEngine;
using UnityEngine.EventSystems;

public class PantryItemUI : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    public FoodItems foodItem;
    public GameObject highlight;

    private bool taken = false;

    public void OnPointerClick(PointerEventData eventData)
    {
        if (taken)
        {
            Debug.Log("Already took " + foodItem.itemName);
            return;
        }

        taken = true;

        Debug.Log("Took: " + foodItem.itemName);

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
