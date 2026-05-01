using UnityEngine;

public class DebugRecipeSetup : MonoBehaviour
{
    [SerializeField] private MenuData menuData;

    void Start()
    {
        var menuItems = menuData.GetMenuItems();
        
        Debug.Log("===== RECIPE DEBUG =====");
        foreach (var item in menuItems)
        {
            Debug.Log($"\n{item.dishName}:");
            Debug.Log($"  Cooking Type: {item.cookingType}");
            Debug.Log($"  Processes Required: {(item.processesRequired == null ? "NULL" : item.processesRequired.Count)}");
            
            if (item.processesRequired != null)
            {
                for (int i = 0; i < item.processesRequired.Count; i++)
                {
                    var gameObj = item.processesRequired[i];
                    if (gameObj == null)
                    {
                        Debug.Log($"    [{i}] NULL GAMEOBJECT!");
                    }
                    else
                    {
                        var interactable = gameObj.GetComponent<InteractableObject>();
                        Debug.Log($"    [{i}] {gameObj.name} - InteractableObject: {(interactable == null ? "NOT FOUND" : interactable.GetType().Name)}");
                    }
                }
            }
        }
    }
}