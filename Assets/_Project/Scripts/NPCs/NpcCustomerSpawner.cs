using System;
using UnityEngine;
using System.Collections;
using Random = UnityEngine.Random;
public class NpcCustomerSpawner : MonoBehaviour, IBind<CurrentDayData>
{
    [SerializeField] float spawnTime = 5f;
    [SerializeField] CustomerAmountData maxCustomers;
    
    [field: SerializeField] public SerializableGuid Id { get; set; } = SerializableGuid.NewGuid();
    private CurrentDayData data;
    private int maxAmount;
    private Days currentDay;
    private NpcCustomerSpawnerObjectPoolManager customerPool;
    private float timeSinceLastSpawn;
    
    private void Awake()
    {
        customerPool = NpcCustomerSpawnerObjectPoolManager.Instance;
        currentDay = Days.Day1;
        maxAmount = maxCustomers.GetMaxAmount(currentDay);
    }

    void Update()
    {
        //Updates the current day for the saving system
        data.currentDay = currentDay;
        
        SpawnCustomer();
    }

    void SpawnCustomer()
    {
        if (Time.time > timeSinceLastSpawn && maxAmount > 0)
        {
            customerPool?.GetCustomer();
            timeSinceLastSpawn = Time.time + spawnTime;
            maxAmount--;
        }
    }

    public void Bind(CurrentDayData data)
    {
        this.data = data;
        this.data.Id = Id;
        currentDay = data.currentDay;
    }
}

public enum Days
{
    Day1,
    Day2,
    Day3
}
[Serializable]
public class CurrentDayData : ISaveable
{
    [field: SerializeField] public SerializableGuid Id { get; set; }
    public Days currentDay;
}