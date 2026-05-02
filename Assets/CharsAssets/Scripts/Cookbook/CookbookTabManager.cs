using UnityEngine;
using UnityEngine.UI;

public class CookbookTabManager : MonoBehaviour
{
    public Book book;
    public Button overviewBtn;
    public Button shopBtn;
    public Button recipesBtn;
    
    public GameObject overviewPanel;
    public GameObject shopPanel;
    public GameObject recipesPanel;

    void Start()
    {
        overviewBtn.onClick.AddListener(() => ShowOverview());
        shopBtn.onClick.AddListener(() => ShowShop());
        recipesBtn.onClick.AddListener(() => ShowRecipes());
    }

    void Update()
    {
        // Show panel based on current page
        if (book.currentPage >= 1 && book.currentPage <= 2)
        {
            overviewPanel.SetActive(true);
            shopPanel.SetActive(false);
            recipesPanel.SetActive(false);
        }
        else if (book.currentPage >= 3 && book.currentPage <= 4)
        {
            shopPanel.SetActive(true);
            overviewPanel.SetActive(false);
            recipesPanel.SetActive(false);
        }
        else if (book.currentPage >= 5)
        {
            recipesPanel.SetActive(true);
            overviewPanel.SetActive(false);
            shopPanel.SetActive(false);
        }
        else
        {
            // Cover or other pages
            overviewPanel.SetActive(false);
            shopPanel.SetActive(false);
            recipesPanel.SetActive(false);
        }
    }

    void ShowOverview()
    {
        book.currentPage = 1;
        overviewPanel.SetActive(true);
        shopPanel.SetActive(false);
        recipesPanel.SetActive(false);
    }

    void ShowShop()
    {
        book.currentPage = 3;
        shopPanel.SetActive(true);
        overviewPanel.SetActive(false);
        recipesPanel.SetActive(false);
    }

    void ShowRecipes()
    {
        book.currentPage = 5;
        recipesPanel.SetActive(true);
        overviewPanel.SetActive(false);
        shopPanel.SetActive(false);
    }
}