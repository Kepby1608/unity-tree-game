using UnityEngine;
using UnityEngine.EventSystems;

public class InventoryItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private Transform originalParent;
    private Canvas canvas;
    private CanvasGroup canvasGroup;
    private RectTransform rectTransform;

    void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        rectTransform = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();
    }

    void OnEnable()
    {
        if (rectTransform == null)
            rectTransform = GetComponent<RectTransform>();

        if (canvasGroup == null)
            canvasGroup = GetComponent<CanvasGroup>();

        if (canvas == null)
            canvas = GetComponentInParent<Canvas>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        Debug.Log($"[InventoryItem] ������ ��������������: {gameObject.name}");

        if (canvasGroup == null)
            canvasGroup = GetComponent<CanvasGroup>();
        if (rectTransform == null)
            rectTransform = GetComponent<RectTransform>();
        if (canvas == null)
            canvas = GetComponentInParent<Canvas>();

        originalParent = transform.parent;
        transform.SetParent(canvas.transform);
        canvasGroup.blocksRaycasts = false;
        canvasGroup.alpha = 0.8f;
    }


    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Debug.Log($"[InventoryItem] ����� ��������������: {gameObject.name}");

        GameObject target = eventData.pointerEnter;

        if (canvasGroup == null)
        {
            Debug.LogError($"canvasGroup is null on {gameObject.name} during OnEndDrag!");
            return;
        }

        if (target != null)
        {
            // �������� ����� ���� � ������� ��� ��� ���������
            InventorySlot slot = target.GetComponent<InventorySlot>();
            if (slot == null)
                slot = target.GetComponentInParent<InventorySlot>();

            if (slot != null)
            {
                Debug.Log($"[InventoryItem] ������ �� ���� {slot.name}, ������: {slot.IsOccupied}");
                if (!slot.TrySetItem(gameObject))
                {
                    Debug.Log($"[InventoryItem] TrySetItem ������ false � ���������� �������");
                    ReturnToOriginalSlot();
                }
            }
            else
            {
                Debug.Log($"[InventoryItem] �� ������ {target.name}, �� ��� �������� �� �������� ������");
                ReturnToOriginalSlot();
            }
        }
        else
        {
            Debug.Log("[InventoryItem] ������ �� ��� ��������");
            ReturnToOriginalSlot();
        }


        canvasGroup.blocksRaycasts = true;
        canvasGroup.alpha = 1f;
    }


    private void ReturnToOriginalSlot()
    {
        transform.SetParent(originalParent);
        transform.localPosition = Vector3.zero;
    }
}