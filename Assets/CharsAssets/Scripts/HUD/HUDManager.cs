using UnityEngine;
using UnityEngine.SceneManagement;

public class HUDManager : MonoBehaviour
{
    public void OpenCookbook()
    {
        SceneManager.LoadScene("CookBook");
    }
}