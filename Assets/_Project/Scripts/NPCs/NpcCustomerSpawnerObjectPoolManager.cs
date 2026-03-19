using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Pool;

[RequireComponent(typeof(NpcCustomerSpawner))]
public class NpcCustomerSpawnerObjectPoolManager : MonoBehaviour
{ 
    private IObjectPool<GameObject> customerPool;
    public readonly List<GameObject> activeCustomers = new();
    private readonly Dictionary<GameObject, int> Counts = new();
    
    private static NpcCustomerSpawnerObjectPoolManager instance;
    public static NpcCustomerSpawnerObjectPoolManager Instance => instance;
    
    [SerializeField] private GameObject customerPrefab;
    [SerializeField] private Transform customerParent;
    [SerializeField] private bool collectionCheck = true;
    [SerializeField] private int defaultPoolSize = 10;
    [SerializeField] private int maxSize = 50;
    [SerializeField] private int maxCustomerInstances = 30;
    [SerializeField] private float spawnRadius = 5f;
    [SerializeField] private GameObject centerPoint;
    [SerializeField] private List<SkinnedMeshRenderer> costumes;
    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
    }
    
    private void Start()
    {
        InitializePool();
    }

    private void InitializePool()
    {
        customerPool = new ObjectPool<GameObject>(
            SpawnCustomer, 
            OnGetCustomer, 
            OnReleaseCustomer, 
            OnDestroyCustomerPool,
            collectionCheck: collectionCheck,
            defaultCapacity: defaultPoolSize,
            maxSize: maxSize);
    }

    public bool CanSpawnCustomer(GameObject customer)
    {
        return !Counts.TryGetValue(customer, out var count) || count < maxCustomerInstances;
    }
    public GameObject GetCustomer()
    {
        var customer = customerPool.Get();
        customer.GetComponent<ChangeStateCustomerManager>().ReleasedFromPool = true;
        return customer;
    }
    public void ReleaseCustomer(GameObject customer)
    {
        customer.GetComponent<ChangeStateCustomerManager>().ReleasedFromPool = false;
        customerPool.Release(customer);
    }
    private GameObject SpawnCustomer()
    {
        var customer = Instantiate(customerPrefab, GetRandomPosition(), Quaternion.identity, customerParent);
        costumes = customer.GetComponentsInChildren<SkinnedMeshRenderer>(true).ToList();
        var currentActiveCostume = customer.GetComponentInChildren<SkinnedMeshRenderer>();
        currentActiveCostume.gameObject.SetActive(false);
        
        var randomCostume = costumes[Random.Range(0, costumes.Count)];
        randomCostume.gameObject.SetActive(true);
        customer.SetActive(false);
        SaveLoadSystem.Instance.RegisterAIEntity(customer.GetComponent<AIEntitiy>());
        return customer;
    }
    private void OnGetCustomer(GameObject customer)
    {
        customer.SetActive(true);
        customer.transform.position = GetRandomPosition();
        activeCustomers.Add(customer);
    }
    private void OnReleaseCustomer(GameObject customer)
    {
        customer.SetActive(false);
        activeCustomers.Remove(customer);
    }

    private void OnDestroyCustomerPool(GameObject customer)
    {
        Destroy(customer);
    }

    Vector3 GetRandomPosition()
    {
        var randomSpawnPoint = centerPoint.transform.position + Random.insideUnitSphere * spawnRadius;
        NavMesh.SamplePosition(randomSpawnPoint, out var hit, spawnRadius, 1);
        var finalPosition = hit.position;
        return finalPosition;
    }
}