using UnityEngine;
using UnityEngine.UI;

public class KeyboardInputHandler : MonoBehaviour
{
    [SerializeField] private Button cookbookButton;

    private void Update()
    {
        // Press 1 to open cookbook
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            if (cookbookButton != null)
            {
                cookbookButton.onClick.Invoke();
                Debug.Log("Cookbook opened!");
            }
        }
    }
}