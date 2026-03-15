using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;


[CreateAssetMenu(fileName = "LineOrderData", menuName = "Data/LineOrderData")]
public class LineOrderData : ScriptableObject
{
    [SerializeField] public List<OrderNode> lineOrder;

    private void OnEnable()
    {
        var line = GameObject.FindGameObjectsWithTag("OrderLine");
        foreach (var spot in line)
        {
            lineOrder.Add(spot.GetComponent<OrderNode>());
        }   
        lineOrder.Sort((a,b) => String.Compare(a.name, b.name, StringComparison.Ordinal));
    }
    public List<OrderNode> GetLineOrder()
    {
        return lineOrder;
    }

    private void OnDisable()
    {
        lineOrder.Clear();
    }
}