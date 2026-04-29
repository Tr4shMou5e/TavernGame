using UnityEngine;
[CreateAssetMenu(fileName = "DailyCustomerAmount", menuName = "Data/CustomerAmountData")]
public class CustomerAmountData : ScriptableObject
{
    [SerializeField] private int dayOneMaxAmount = 10;
    [SerializeField] private int dayTwoMaxAmount = 20;
    [SerializeField] private int dayThreeMaxAmount = 25;

    public int GetMaxAmount(Days day)
    {
        return day switch
        {
            Days.Day1 => dayOneMaxAmount,
            Days.Day2 => dayTwoMaxAmount,
            Days.Day3 => dayThreeMaxAmount,
            _ => 0
        };
    }
}