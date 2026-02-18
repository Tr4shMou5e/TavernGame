using UnityEngine;

public class CookingHUDController : MonoBehaviour
{
    public GameObject cookingHUD;

    public void ShowCookingHUD()
    {
        cookingHUD.SetActive(true);
    }

    public void HideCookingHUD()
    {
        cookingHUD.SetActive(false);
    }
}
