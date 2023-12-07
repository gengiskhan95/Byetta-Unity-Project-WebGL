using UnityEngine;

public class AdjustRectTransform : MonoBehaviour
{
    // �rnek objenin RectTransform'i
    public RectTransform rectTransform;

    // Ba�lang��ta sabit de�erler
    private Vector2 startAnchoredPosition;
    private Vector2 startAnchorMin;
    private Vector2 startAnchorMax;

    // Ekran y�kseklik geni�likten b�y�kse de�erler
    public Vector2 largeHeightAnchoredPosition = new Vector2(50f, 50f);
    public Vector2 largeHeightAnchorMin = new Vector2(0.3f, 0.3f);
    public Vector2 largeHeightAnchorMax = new Vector2(0.7f, 0.7f);

    // Ekran geni�lik y�kseklikten b�y�kse de�erler
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

        // Ba�lang�� de�erlerini sakla
        startAnchoredPosition = rectTransform.anchoredPosition;
        startAnchorMin = rectTransform.anchorMin;
        startAnchorMax = rectTransform.anchorMax;
    }

    private void Update()
    {
        // Ekran geni�li�i ve y�ksekli�ini al
        float screenWidth = Screen.width;
        float screenHeight = Screen.height;

        // E�er ekran y�ksekli�i ekran geni�li�inden b�y�kse
        if (screenHeight > screenWidth)
        {
            // Y�kseklik geni�likten b�y�kse, de�erleri b�y�k y�kseklik de�erleri olarak g�ncelle
            rectTransform.anchoredPosition = largeHeightAnchoredPosition;
            rectTransform.anchorMin = largeHeightAnchorMin;
            rectTransform.anchorMax = largeHeightAnchorMax;
        }
        else // Di�er durumda (ekran geni�li�i ekran y�ksekli�inden b�y�kse)
        {
            // Geni�lik y�kseklikten b�y�kse, de�erleri b�y�k geni�lik de�erleri olarak g�ncelle
            rectTransform.anchoredPosition = largeWidthAnchoredPosition;
            rectTransform.anchorMin = largeWidthAnchorMin;
            rectTransform.anchorMax = largeWidthAnchorMax;
        }
    }
}