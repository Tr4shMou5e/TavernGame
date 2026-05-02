using UnityEngine;

public class BookDebugger : MonoBehaviour
{
    public Book book;
    
    private int lastCurrentPage = -1;
    private int framesSinceLastChange = 0;

    void Update()
    {
        framesSinceLastChange++;

        // Monitor page changes
        if (book.currentPage != lastCurrentPage)
        {
            Debug.Log($"[PAGE CHANGE] {lastCurrentPage} → {book.currentPage}");
            lastCurrentPage = book.currentPage;
            framesSinceLastChange = 0;
        }

        // Every 30 frames, log the state
        if (framesSinceLastChange % 30 == 0)
        {
            Debug.Log($"[STATE] Current Page: {book.currentPage} | Total Pages: {book.TotalPageCount} | Interactable: {book.interactable}");
        }
    }
}