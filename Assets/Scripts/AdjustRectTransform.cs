using UnityEngine;

public class AdjustRectTransform : MonoBehaviour
{
    // Örnek objenin RectTransform'i
    public RectTransform rectTransform;

    // Baþlangýçta sabit deðerler
    private Vector2 startAnchoredPosition;
    private Vector2 startAnchorMin;
    private Vector2 startAnchorMax;

    // Ekran yükseklik geniþlikten büyükse deðerler
    public Vector2 largeHeightAnchoredPosition = new Vector2(50f, 50f);
    public Vector2 largeHeightAnchorMin = new Vector2(0.3f, 0.3f);
    public Vector2 largeHeightAnchorMax = new Vector2(0.7f, 0.7f);

    // Ekran geniþlik yükseklikten büyükse deðerler
    public Vector2 largeWidthAnchoredPosition = new Vector2(50f, 50f);
    public Vector2 largeWidthAnchorMin = new Vector2(0.2f, 0.2f);
    public Vector2 largeWidthAnchorMax = new Vector2(0.8f, 0.8f);

    private void Start()
    {
        rectTransform = GetComponent<RectTransform>() ?? transform.parent.GetComponent<RectTransform>();

        if (rectTransform == null)
        {
            Debug.LogError("RectTransform not found in the object or its parent.");
            return;
        }

        // Baþlangýç deðerlerini sakla
        startAnchoredPosition = rectTransform.anchoredPosition;
        startAnchorMin = rectTransform.anchorMin;
        startAnchorMax = rectTransform.anchorMax;
    }

    private void Update()
    {
        // Ekran geniþliði ve yüksekliðini al
        float screenWidth = Screen.width;
        float screenHeight = Screen.height;

        // Eðer ekran yüksekliði ekran geniþliðinden büyükse
        if (screenHeight > screenWidth)
        {
            // Yükseklik geniþlikten büyükse, deðerleri büyük yükseklik deðerleri olarak güncelle
            rectTransform.anchoredPosition = largeHeightAnchoredPosition;
            rectTransform.anchorMin = largeHeightAnchorMin;
            rectTransform.anchorMax = largeHeightAnchorMax;
        }
        else // Diðer durumda (ekran geniþliði ekran yüksekliðinden büyükse)
        {
            // Geniþlik yükseklikten büyükse, deðerleri büyük geniþlik deðerleri olarak güncelle
            rectTransform.anchoredPosition = largeWidthAnchoredPosition;
            rectTransform.anchorMin = largeWidthAnchorMin;
            rectTransform.anchorMax = largeWidthAnchorMax;
        }
    }
}