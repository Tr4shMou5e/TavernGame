using UnityEngine;
using UnityEngine.UI;

public class SectionManager : MonoBehaviour
{
    public Book book;
    public Button nextButton;
    public Button prevButton;
    public Button overviewTabButton;
    public Button shopTabButton;
    public Button recipesTabButton;

    private bool isAnimating = false;

    void Start()
    {
        nextButton.onClick.AddListener(OnNextClicked);
        prevButton.onClick.AddListener(OnPrevClicked);
        overviewTabButton.onClick.AddListener(OnOverviewClicked);
        shopTabButton.onClick.AddListener(OnShopClicked);
        recipesTabButton.onClick.AddListener(OnRecipesClicked);
    }

    void OnNextClicked()
    {
        if (isAnimating) return;
        
        if (book.currentPage + 2 < book.TotalPageCount)
        {
            isAnimating = true;
            book.currentPage += 2;
            Invoke("ResetAnimFlag", 0.3f);
            Debug.Log("Next: Page " + book.currentPage);
        }
    }

    void OnPrevClicked()
    {
        if (isAnimating) return;
        
        if (book.currentPage - 2 >= 0)
        {
            isAnimating = true;
            book.currentPage -= 2;
            Invoke("ResetAnimFlag", 0.3f);
            Debug.Log("Prev: Page " + book.currentPage);
        }
    }

    void OnOverviewClicked()
    {
        if (isAnimating) return;
        isAnimating = true;
        book.currentPage = 1;
        Invoke("ResetAnimFlag", 0.3f);
        Debug.Log("Overview: Page 1");
    }

    void OnShopClicked()
    {
        if (isAnimating) return;
        isAnimating = true;
        book.currentPage = 3;
        Invoke("ResetAnimFlag", 0.3f);
        Debug.Log("Shop: Page 3");
    }

    void OnRecipesClicked()
    {
        if (isAnimating) return;
        isAnimating = true;
        book.currentPage = 5;
        Invoke("ResetAnimFlag", 0.3f);
        Debug.Log("Recipes: Page 5");
    }

    void ResetAnimFlag()
    {
        isAnimating = false;
    }
}