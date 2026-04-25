using System;
using UnityEngine;

public struct CustomerOrderKey : IEquatable<CustomerOrderKey>
{
    public GameObject Customer;
    public FoodItem FoodItem;

    public CustomerOrderKey(GameObject customer, FoodItem foodItem)
    {
        Customer = customer;
        FoodItem = foodItem;
    }

    public bool Equals(CustomerOrderKey other)
    {
        return Customer == other.Customer && FoodItem == other.FoodItem;
    }

    public override bool Equals(object obj)
    {
        return obj is CustomerOrderKey other && Equals(other);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Customer, FoodItem);
    }
}