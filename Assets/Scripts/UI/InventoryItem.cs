using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// ����� �������������� ���������� ����������� �������� � UI ���������
/// ��������� ���������� �������������� (drag and drop)
/// </summary>
[RequireComponent(typeof(CanvasGroup), typeof(Image))]
public class InventoryItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField] private Image _icon;                 // ��������� Image ��� ����������� �������

    public Item RuntimeItem { get; private set; }         // ������ �� ������ ��������
    public Transform OriginalParent { get; private set; } // �������� ������������ transform

    private Canvas _rootCanvas;                           // �������� Canvas ��� ����������� ����������������
    private CanvasGroup _canvasGroup;                     // CanvasGroup ��� ���������� ������������� � ����������� �����
    private RectTransform _rect;                          // RectTransform ��� ����������������
    private Vector3 _dragStartPosition;                   // ��������� ������� ����� ���������������

    /// <summary>
    /// ����� Awake ���������� ��� �������� �������
    /// �������������� ����������� ����������
    /// </summary>
    private void Awake()
    {
        // �������� ����������� ����������
        _canvasGroup = GetComponent<CanvasGroup>();
        _rect = GetComponent<RectTransform>();

        // ���� ������ �� ������ �� ����������� � ����������, �������� ����� �
        if (_icon == null) _icon = GetComponent<Image>();

        // ���� �������� Canvas � ������������ ��������
        _rootCanvas = GetComponentInParent<Canvas>();
    }

    /// <summary>
    /// �������������� ������� �������
    /// </summary>
    /// <param name="item">������ ��������</param>
    public void Init(Item item)
    {
        RuntimeItem = item; // ��������� ������ �� ������ ��������
        if (_icon != null)
        {
            _icon.sprite = item.Sprite;           // ������������� ������
            _icon.preserveAspect = true;          // ��������� ��������� �������
        }
    }

    /// <summary>
    /// ���������� ������ ��������������
    /// </summary>
    public void OnBeginDrag(PointerEventData eventData)
    {
        // ���������� �������� ������� � ��������
        _dragStartPosition = _rect.position;
        OriginalParent = transform.parent;

        // ���������� ��� � ��� ���� ������ �� �������� Canvas
        if (_rootCanvas == null) _rootCanvas = GetComponentInParent<Canvas>();

        // ���������� ������ �� ������� ������� �������� Canvas
        transform.SetParent(_rootCanvas.transform, true);
        transform.SetAsLastSibling(); // �������� ������ ������ ���������

        // ����������� ���������� ������� ��������������
        _canvasGroup.blocksRaycasts = false; // ��������� ���������� ����� (����� ����� ���� ���������� ����� ��� ���������)
        _canvasGroup.alpha = 0.8f;           // ������ ������� ��������������
    }

    /// <summary>
    /// ���������� �������� ��������������
    /// </summary>
    public void OnDrag(PointerEventData eventData)
    {
        // ��������� ������� �������� � ������������ � �������� �������/�������
        // ��������� ������� ������� ��� ����������� ����������������
        _rect.anchoredPosition += eventData.delta / _rootCanvas.scaleFactor;
    }

    /// <summary>
    /// ���������� ��������� ��������������
    /// </summary>
    public void OnEndDrag(PointerEventData eventData)
    {
        // ��������� �� ��� �� ������ ��������� �� ����� ��������������
        if (this == null || gameObject == null) return;

        // ��������������� ���������� ���������
        _canvasGroup.blocksRaycasts = true;
        _canvasGroup.alpha = 1f;

        // �������� ���������� ������� ����
        bool handled = false;
        if (eventData.pointerEnter != null)
        {
            // ���� ��������� InventorySlot � ������� ������� ��� ��� ���������
            InventorySlot slot = eventData.pointerEnter.GetComponent<InventorySlot>() ??
                                eventData.pointerEnter.GetComponentInParent<InventorySlot>();

            // ���� ����� ����, �������� ��������� � ���� �������
            if (slot != null)
            {
                handled = slot.TrySetItem(this);
            }
        }

        // ���� �� ������� ���������� (�� ������ � ���� ��� ���� �����), ���������� �� �����
        if (!handled)
        {
            ReturnToOriginalPosition();
        }
    }

    /// <summary>
    /// ���������� ������� � �������� �������
    /// </summary>
    public void ReturnToOriginalPosition()
    {
        if (OriginalParent != null)
        {
            // ���������� � ��������� �������� � ���������� �������
            transform.SetParent(OriginalParent, false);
            transform.localPosition = Vector3.zero;
        }
        else
        {
            // ���� �������� �� ��������, ���������� � �������� ������� �������
            transform.position = _dragStartPosition;
        }
    }
}