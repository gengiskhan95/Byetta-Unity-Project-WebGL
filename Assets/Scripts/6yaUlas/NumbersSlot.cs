using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class NumbersSlot : MonoBehaviour, IDropHandler
{
    [Header("Slot Settings")]
    public int id;
    [HideInInspector] public int collectData;

    private DragAndDropGame DADG;
    private DragSpecialObjects DSO;

    [Header("Particle System")]
    public ParticleSystem sparkle;

    public static NumbersSlot instance;

    private void Awake()
    {
        sparkle = GetComponentInChildren<ParticleSystem>();
        sparkle.gameObject.SetActive(false);
        instance = this;
    }

    private void Start()
    {
        DADG = FindObjectOfType<DragAndDropGame>();
        DSO = FindObjectOfType<DragSpecialObjects>();
    }

    public void OnDrop(PointerEventData eventData)
    {
        Debug.Log("Item Dropped");

        if (eventData.pointerDrag != null)
        {
            DragAndDrop draggedObject = eventData.pointerDrag.GetComponent<DragAndDrop>();

            if (draggedObject != null && draggedObject.id == id)
            {
                StartGlow();
                Invoke("StopGlow", 1f);

                collectData = draggedObject.id;
                DSO.EnableSpecialNumber();

                eventData.pointerDrag.GetComponent<RectTransform>().anchoredPosition =
                    GetComponent<RectTransform>().anchoredPosition;

                DADG.NumbersInRightPosition();
                eventData.pointerDrag.GetComponent<DragAndDrop>().enabled = false;

                // Set the alpha of the dragged object back to 1f (fully opaque)
                CanvasGroup draggedCanvasGroup = eventData.pointerDrag.GetComponent<CanvasGroup>();
                if (draggedCanvasGroup != null)
                {
                    draggedCanvasGroup.alpha = 1f;
                }
            }
            else if (draggedObject != null)
            {
                draggedObject.ResetPosition();

                // Set the alpha of the dragged object back to 1f (fully opaque)
                CanvasGroup draggedCanvasGroup = eventData.pointerDrag.GetComponent<CanvasGroup>();
                if (draggedCanvasGroup != null)
                {
                    draggedCanvasGroup.alpha = 1f;
                }
            }
        }
    }

    private void StartGlow()
    {
        sparkle.gameObject.SetActive(true);
        sparkle.Play();
    }

    private void StopGlow()
    {
        sparkle.Stop();
        sparkle.gameObject.SetActive(false);
    }
}
