using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Customer", menuName = "Customer Name")]
public class CustomerName : ScriptableObject
{
    public List<string> names;
    public string selectedName;
    public string GetRandomName()
    {
        if(names.Count == 0)
            return "Customer";
        selectedName = names[Random.Range(0, names.Count)];
        return selectedName;
    }
}