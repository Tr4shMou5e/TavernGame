// using UnityEngine;
// using UnityEngine.UI;
//
// public class BookController : MonoBehaviour
// {
//     [Header("Book References")]
//     [SerializeField] private Book     book;
//     [SerializeField] private AutoFlip autoFlip;
//
//     [Header("Page Content Panels")]
//     [Tooltip("One entry per page spread (currentPage value: 0,2,4,6...)")]
//     [SerializeField] private GameObject[] pageSpreadContents;
//     // Assign in Inspector, one per spread:
//     // [0] = Overview (currentPage 0)
//     // [1] = FoodDivider+Recipe1 (currentPage 2)
//     // [2] = Recipe2+3  (currentPage 4)
//     // etc.
//
//     [Header("Tabs")]
//     [SerializeField] private Image tabOverview;
//     [SerializeField] private Image tabRecipes;
//     [SerializeField] private Image tabSettings;
//
//     [Header("Tab Colors")]
//     [SerializeField] private Color activeColor   = new Color(0.95f, 0.87f, 0.70f);
//     [SerializeField] private Color inactiveColor = new Color(0.55f, 0.40f, 0.25f);
//
//     // currentPage values for each section's first spread
//     private const int PAGE_OVERVIEW = 2;
//     private const int PAGE_RECIPES  = 4;   // Food divider spread
//     private const int PAGE_SETTINGS = 22;  // adjust if your total pages differ
//
//     void Start()
//     {
//         // Hook into OnFlip so we update content after every page turn
//         book.OnFlip.AddListener(OnPageFlipped);
//         RefreshDisplay();
//     }
//
//     // ── Called by Book's OnFlip event ────────────────────────────────────────
//     void OnPageFlipped()
//     {
//         RefreshDisplay();
//     }
//
//     // ── Tab buttons call these ────────────────────────────────────────────────
//     public void GoToOverview() => JumpToPage(PAGE_OVERVIEW);
//     public void GoToRecipes()  => JumpToPage(PAGE_RECIPES);
//     public void GoToSettings() => JumpToPage(PAGE_SETTINGS);
//
//     // ── Internal ──────────────────────────────────────────────────────────────
//     void JumpToPage(int targetPage)
//     {
//         int current = book.currentPage;
//
//         // Flip right (forward) or left (backward) until we reach target
//         // Each flip moves by 2 pages
//         while (book.currentPage < targetPage)
//             autoFlip.FlipRightPage();
//
//         while (book.currentPage > targetPage)
//             autoFlip.FlipLeftPage();
//
//         RefreshDisplay();
//     }
//
//     void RefreshDisplay()
//     {
//         // currentPage 0,2,4,6... → array index 0,1,2,3...
//         int spreadIndex = book.currentPage / 2;
//
//         for (int i = 0; i < pageSpreadContents.Length; i++)
//             if (pageSpreadContents[i] != null)
//                 pageSpreadContents[i].SetActive(i == spreadIndex);
//
//         UpdateTabs();
//     }
//
//     void UpdateTabs()
//     {
//         int p = book.currentPage;
//         bool isOverview = p < PAGE_RECIPES;
//         bool isSettings = p >= PAGE_SETTINGS;
//         bool isRecipes  = !isOverview && !isSettings;
//
//         tabOverview.color = isOverview ? activeColor : inactiveColor;
//         tabRecipes.color  = isRecipes  ? activeColor : inactiveColor;
//         tabSettings.color = isSettings ? activeColor : inactiveColor;
//     }
// }