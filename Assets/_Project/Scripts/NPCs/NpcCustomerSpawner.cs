using System;
using UnityEngine;
using System.Collections;
using Random = UnityEngine.Random;
public class NpcCustomerSpawner : MonoBehaviour
{
    [SerializeField] float spawnTime = 5f;
    [SerializeField] CustomerAmountData maxCustomers;
    
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
        SpawnCustomer();
    }

    void SpawnCustomer()
    {
        if (Time.time > timeSinceLastSpawn && maxAmount > 0)
        {
            customerPool.GetCustomer();
            timeSinceLastSpawn = Time.time + spawnTime;
            maxAmount--;
        }
    }
    
}

public enum Days
{
    Day1,
    Day2,
    Day3
}