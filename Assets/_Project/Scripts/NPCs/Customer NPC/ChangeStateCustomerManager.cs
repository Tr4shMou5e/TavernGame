using System;
using UnityEngine;

public class ChangeStateCustomerManager : MonoBehaviour
{
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
    
    public bool OrderTaken()
    {
        return isOrderTaken;
    }
    public bool LineFull()
    {
        return isLineFull;
    }
    public bool HasOrder()
    {
        return hasOrderNode;
    }
    public bool HasOrderServed()
    {
        return orderServed;
    }
    public bool FinishedEating()
    {
        return hasFinishedEating;
    }
}