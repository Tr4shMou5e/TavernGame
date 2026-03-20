using UnityEngine;

public class CatchGame : MonoBehaviour
{
    public RectTransform Shaker;
    private RectTransform canvasRect;

    [SerializeField] private RectTransform leftBoundary;
    [SerializeField] private RectTransform rightBoundary;
    void Start()
    {
        canvasRect = GetComponentInParent<Canvas>().GetComponent<RectTransform>();
    }

   
    void Update()
    {
        Vector2 mousePos;

        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRect,Input.mousePosition, null,out mousePos );
        float leftX = leftBoundary.anchoredPosition.x;
        float rightX = rightBoundary.anchoredPosition.x;
        float clampedX = Mathf.Clamp(mousePos.x, leftX, rightX);
        Shaker.anchoredPosition = new Vector2(clampedX, Shaker.anchoredPosition.y);
    }
}
