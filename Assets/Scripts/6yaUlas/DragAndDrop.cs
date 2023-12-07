using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(RectTransform), typeof(CanvasGroup))]
public class DragAndDrop : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    [SerializeField] public int id;
    [SerializeField] public Vector3 initPos;

    private RectTransform rectTrans;
    private CanvasGroup canvasGroup;
    private Canvas myCanvas;

    private Vector2 minDragPosition;
    private Vector2 maxDragPosition;

    public static DragAndDrop instance;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        rectTrans = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();

        // myCanvas'i sadece bir kere bulup, daha sonra deðiþtirmiyoruz.
        if (myCanvas == null)
            myCanvas = FindObjectOfType<Canvas>();

        // Sürükleme sýnýrlarýný belirle
        CalculateDragBounds();

        initPos = transform.position;
    }

    private void CalculateDragBounds()
    {
        if (myCanvas == null)
            return;

        Vector2 canvasSize = myCanvas.GetComponent<RectTransform>().sizeDelta;
        Vector2 objSize = rectTrans.sizeDelta;

        minDragPosition = new Vector2(-(canvasSize.x / 2) + (objSize.x / 2), -(canvasSize.y / 2) + (objSize.y / 2));
        maxDragPosition = new Vector2((canvasSize.x / 2) - (objSize.x / 2), (canvasSize.y / 2) - (objSize.y / 2));
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        canvasGroup.alpha = 0.6f;
        canvasGroup.blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        // Sürükleme sýrasýnda objenin yeni pozisyonunu hesapla ve sýnýrlara uygula
        Vector2 newPosition = rectTrans.anchoredPosition + eventData.delta / myCanvas.scaleFactor;
        rectTrans.anchoredPosition = new Vector2(
            Mathf.Clamp(newPosition.x, minDragPosition.x, maxDragPosition.x),
            Mathf.Clamp(newPosition.y, minDragPosition.y, maxDragPosition.y)
        );
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        // Buraya gerekirse sürükleme baþlamadan önce yapýlacak iþlemler eklenebilir.
    }

    public void ResetPosition()
    {
        transform.position = initPos;
    }
}
