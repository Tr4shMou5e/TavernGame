using System;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using Cysharp.Text;
public class SpawnFoodItem : MonoBehaviour
{
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private Transform parentTransform;
    [SerializeField] private Canvas deliveryCanvas;
    [SerializeField] private MenuData menu;
    private FoodItem foodItem;
    void SpawnFoodItemObject(CustomerOrderKey key)
    {
        if (key.FoodItem == null) return;
        foodItem = menu.GetMenuItems().Find(item => item.id == key.FoodItem.id);
        if (foodItem == null)
        {
            Debug.LogWarning($"No food item found for key: {key}");
            return;
        }
        
        GameObject foodObject = Instantiate(foodItem.dishPrefab, spawnPoint.position, spawnPoint.rotation, parentTransform);
        foodObject.name = foodItem.dishName;
        
        var customerName = deliveryCanvas.GetComponentInChildren<TextMeshProUGUI>(true);
        customerName.SetTextFormat("Deliver to {0}", key.Customer.name);
        deliveryCanvas.gameObject.SetActive(true);
    }

    private void OnEnable()
    {
        InteractableObject.OnOrderDone += SpawnFoodItemObject;
        NpcSitState.OnFoodGiven += DestroyFoodItem;
    }
    private void OnDisable()
    {
        InteractableObject.OnOrderDone -= SpawnFoodItemObject;
        NpcSitState.OnFoodGiven -= DestroyFoodItem;
    }
    
    private void DestroyFoodItem(AIEntitiy entity)
    {
        //Increment balance here
        deliveryCanvas.gameObject.SetActive(false);
        Destroy(parentTransform.GetChild(0).gameObject);
    }
}