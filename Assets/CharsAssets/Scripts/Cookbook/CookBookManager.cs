using UnityEngine;
using UnityEngine.UI;

public class BookController : MonoBehaviour
{
    [Header("Book References")]
    [SerializeField] private Book     book;
    [SerializeField] private AutoFlip autoFlip;

    [Header("Page Content Panels")]
    [Tooltip("One entry per page spread (currentPage value: 0,2,4,6...)")]
    [SerializeField] private GameObject[] pageSpreadContents;
   
    [Header("Tabs")]
    [SerializeField] private Image tabOverview;
    [SerializeField] private Image tabRecipes;
    [SerializeField] private Image tabSettings;

    [Header("Tab Colors")]
    [SerializeField] private Color activeColor   = new Color(0.95f, 0.87f, 0.70f);
    [SerializeField] private Color inactiveColor = new Color(0.55f, 0.40f, 0.25f);

    private const int PAGE_OVERVIEW = 2;
    private const int PAGE_RECIPES  = 4;   
    private const int PAGE_SETTINGS = 22;  

    void Start()
    {
        book.OnFlip.AddListener(OnPageFlipped);
        RefreshDisplay();
    }

    void OnPageFlipped()
    {
        RefreshDisplay();
    }

    public void GoToOverview() => JumpToPage(PAGE_OVERVIEW);
    public void GoToRecipes()  => JumpToPage(PAGE_RECIPES);
    public void GoToSettings() => JumpToPage(PAGE_SETTINGS);

    void JumpToPage(int targetPage)
    {
        int current = book.currentPage;

        while (book.currentPage < targetPage)
            autoFlip.FlipRightPage();

        while (book.currentPage > targetPage)
            autoFlip.FlipLeftPage();

        RefreshDisplay();
    }

    void RefreshDisplay()
    {
        int spreadIndex = book.currentPage / 2;

        for (int i = 0; i < pageSpreadContents.Length; i++)
            if (pageSpreadContents[i] != null)
                pageSpreadContents[i].SetActive(i == spreadIndex);

        UpdateTabs();
    }

    void UpdateTabs()
    {
        int p = book.currentPage;
        bool isOverview = p < PAGE_RECIPES;
        bool isSettings = p >= PAGE_SETTINGS;
        bool isRecipes  = !isOverview && !isSettings;

        tabOverview.color = isOverview ? activeColor : inactiveColor;
        tabRecipes.color  = isRecipes  ? activeColor : inactiveColor;
        tabSettings.color = isSettings ? activeColor : inactiveColor;
    }
}