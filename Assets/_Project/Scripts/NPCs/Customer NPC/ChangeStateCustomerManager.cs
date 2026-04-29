using System;
using UnityEngine;

public class ChangeStateCustomerManager : MonoBehaviour
{
    private bool releasedFromPool;
    public bool ReleasedFromPool { set => releasedFromPool = value; }
    private bool isOrderTaken;
    public bool IsOrderTaken { set => isOrderTaken = value; }
    private bool isLineFull;
    public bool IsLineFull { set => isLineFull = value; }
    private bool hasOrderNode;
    public bool HasOrderNode { set => hasOrderNode = value; }
    private bool orderServed;
    public bool OrderServed { set => orderServed = value; }
    private bool hasFinishedEating;
    public bool HasFinishedEating { set => hasFinishedEating = value; }
    private bool playerInRange;
    public bool PlayerInRange { get => playerInRange; set => playerInRange = value; }
    
    // Checks if the order has been taken
    public bool OrderTaken()
    {
        return isOrderTaken;
    }
    
    // Checks if the line is full
    public bool LineFull()
    {
        return isLineFull;
    }
    
    // Checks if the order has been taken
    public bool HasOrder()
    {
        return hasOrderNode;
    }
    
    // Checks if the order has been served
    public bool HasOrderServed()
    {
        return orderServed;
    }
    
    // Checks if the customer has finished eating
    public bool FinishedEating()
    {
        return hasFinishedEating;
    }

    public bool IsReleasedFromPool()
    {
        return releasedFromPool;
    }
}