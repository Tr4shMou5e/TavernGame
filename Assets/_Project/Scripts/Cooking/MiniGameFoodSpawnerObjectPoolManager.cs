using System;
using UnityEngine;
using UnityEngine.Pool;
using Random = UnityEngine.Random;

public class MiniGameFoodSpawnerObjectPoolManager : MonoBehaviour
{
    
    [SerializeField] MenuData menu;
    
    [Header("Bounds")] [Tooltip("The bounds of the spawn area")] 
    [SerializeField] private float minXPosition = -5f;
    [SerializeField] private float minYPosition = -5f;
    [SerializeField] private float maxXPosition = 5f;
    [SerializeField] private float maxYPosition = 5f;
    
    [Header("Object Pool Settings")]
    [SerializeField] private GameObject foodItemPrefab;
    [SerializeField] private Transform spawnParent;
    [SerializeField] private bool collectionCheck = true;
    [SerializeField] private int defaultPoolSize = 10;
    [SerializeField] private int maxSize = 20;
    
    private IObjectPool<GameObject> foodPool;
    private static MiniGameFoodSpawnerObjectPoolManager instance;
    public static MiniGameFoodSpawnerObjectPoolManager Instance => instance;
    
    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        InitializePool();
    }
    private void InitializePool()
    {
        foodPool = new ObjectPool<GameObject>(
            SpawnFood, 
            OnGetFood, 
            OnReleaseFood, 
            OnDestroyFoodPool,
            collectionCheck: collectionCheck,
            defaultCapacity: defaultPoolSize,
            maxSize: maxSize);
    }

    public GameObject GetFood()
    {
        return foodPool.Get();
    }

    public void ReleaseFood(GameObject food)
    {
        foodPool.Release(food);
    }
    private GameObject SpawnFood()
    {
        var food = Instantiate(foodItemPrefab, GetRandomSpawnPosition(), Quaternion.identity, spawnParent);
        food.GetComponent<SpriteRenderer>().sprite = menu.SelectRandomMenuItem().dishImage;
        
        return food;
    }
    private void OnGetFood(GameObject food)
    {
        food.SetActive(true);
    }
    private void OnReleaseFood(GameObject food)
    {
        food.SetActive(false);
    }

    private void OnDestroyFoodPool(GameObject food)
    {
        Destroy(food);
    }
    Vector3 GetRandomSpawnPosition()
    {
        var randomX = Random.Range(minXPosition, maxXPosition);
        var randomY = Random.Range(minYPosition, maxYPosition);
        return new Vector3(randomX, randomY, 0f);
    }
}