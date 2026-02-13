using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class GameManager : MonoBehaviour
{
    public int funds = 1000;
    public int day = 1;
    public string timeOfDay = "Morning";
public TextMeshProUGUI persistentMoneyText;
public TextMeshProUGUI persistentDayText;

public Slider ProfitGoalBar;

public float DailyGoal = 200f;
public float CurrentProfit = 0f;

public void AddProfit(float amount)
{
    CurrentProfit += amount;
    ProfitGoalBar.value = CurrentProfit / DailyGoal;

}

void UpdateMoneyUI()
{
    persistentMoneyText.text = funds.ToString();
}

void UpdateDayUI()
{
    persistentDayText.text = "Day " + day + " - " + timeOfDay;
}
}