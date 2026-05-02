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

    void Start()
    {
        if (nextButton != null)
            nextButton.onClick.AddListener(() => FlipNext());
        else
            Debug.LogError("Next Button not assigned!");

        if (prevButton != null)
            prevButton.onClick.AddListener(() => FlipPrev());
        else
            Debug.LogError("Prev Button not assigned!");

        if (overviewTabButton != null)
            overviewTabButton.onClick.AddListener(() => GoToPage(1));
        else
            Debug.LogError("Overview Tab not assigned!");

        if (shopTabButton != null)
            shopTabButton.onClick.AddListener(() => GoToPage(3));
        else
            Debug.LogError("Shop Tab not assigned!");

        if (recipesTabButton != null)
            recipesTabButton.onClick.AddListener(() => GoToPage(5));
        else
            Debug.LogError("Recipes Tab not assigned!");
    }

    void FlipNext()
    {
        Debug.Log("FlipNext called. Current page: " + book.currentPage);
        if (book.currentPage + 2 < book.TotalPageCount)
        {
            book.currentPage += 2;
            Debug.Log("Flipped to page: " + book.currentPage);
        }
        else
        {
            Debug.Log("At end of book!");
        }
    }

    void FlipPrev()
    {
        Debug.Log("FlipPrev called. Current page: " + book.currentPage);
        if (book.currentPage - 2 >= 0)
        {
            book.currentPage -= 2;
            Debug.Log("Flipped to page: " + book.currentPage);
        }
        else
        {
            Debug.Log("At start of book!");
        }
    }

    void GoToPage(int page)
    {
        Debug.Log("Going to page: " + page);
        book.currentPage = page;
    }
}