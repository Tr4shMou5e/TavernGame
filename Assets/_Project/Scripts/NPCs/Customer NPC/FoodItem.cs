using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[Serializable]
public class FoodItem
{
    public string dishName;
    public GameObject dishPrefab;
    public Sprite dishImage;
    public string id;
    public float price;
    public int score;
    [FormerlySerializedAs("processes")]
    public List<GameObject> processesRequired;
    
    // Track which processes this recipe needs (e.g., needs cutting board before pot)
    [Tooltip("Check if this recipe requires cutting board first")]
    public bool requiresCuttingBoard = false;
    
    // Updated enum - Pan removed
    public enum CookingType { Pot, Baking, None }
    public CookingType cookingType = CookingType.None;
    
    public enum ProcessType
    {
        Catch,
        Fill,
        Shake
    }

    public List<IngredientsItems> ingredients;
    public List<ProcessType> processes;

    /// <summary>
    /// Automatically populates processesRequired based on cookingType and requiresCuttingBoard.
    /// Call this after setting the cookingType.
    /// </summary>
    public void SetupProcessesRequired()
    {
        processesRequired.Clear();

        // Add Cutting Board first if required
        if (requiresCuttingBoard)
        {
            var cuttingBoard = GameObject.Find("SM_Prop_Chopping_Board_01");
            if (cuttingBoard != null)
            {
                processesRequired.Add(cuttingBoard);
                Debug.Log($"{dishName} - Added Cutting Board to processes");
            }
            else
            {
                Debug.LogWarning($"{dishName} - Cutting Board not found in scene!");
            }
        }

        // Then add cooking process
        switch (cookingType)
        {
            case CookingType.Pot:
                // Add the Cauldron GameObject (SM_Prop_Cauldron_01)
                var cauldron = GameObject.Find("SM_Prop_Cauldron_01");
                if (cauldron != null)
                {
                    processesRequired.Add(cauldron);
                    Debug.Log($"{dishName} - Added Cauldron to processes");
                }
                else
                {
                    Debug.LogWarning($"{dishName} - Cauldron not found in scene!");
                }
                break;

            case CookingType.Baking:
                // SM_Prop_Stove_02 is the Oven for Baking
                var oven = GameObject.Find("SM_Prop_Stove_02");
                if (oven != null)
                {
                    processesRequired.Add(oven);
                    Debug.Log($"{dishName} - Added Oven (Stove) to processes");
                }
                else
                {
                    Debug.LogWarning($"{dishName} - Oven not found in scene!");
                }
                break;

            case CookingType.None:
            default:
                // No cooking required
                Debug.Log($"{dishName} - No cooking required");
                break;
        }

        Debug.Log($"{dishName} setup complete. Processes required: {processesRequired.Count}");
    }
}