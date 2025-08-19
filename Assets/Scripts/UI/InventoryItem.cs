using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// Класс представляющий визуальное отображение предмета в UI инвентаря
/// Реализует интерфейсы перетаскивания (drag and drop)
/// </summary>
[RequireComponent(typeof(CanvasGroup), typeof(Image))]
public class InventoryItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField] private Image _icon;                 // Компонент Image для отображения спрайта

    public Item RuntimeItem { get; private set; }         // Ссылка на данные предмета
    public Transform OriginalParent { get; private set; } // Исходный родительский transform

    private Canvas _rootCanvas;                           // Корневой Canvas для корректного позиционирования
    private CanvasGroup _canvasGroup;                     // CanvasGroup для управления прозрачностью и блокировкой лучей
    private RectTransform _rect;                          // RectTransform для позиционирования
    private Vector3 _dragStartPosition;                   // Начальная позиция перед перетаскиванием

    /// <summary>
    /// Метод Awake вызывается при создании объекта
    /// Инициализирует необходимые компоненты
    /// </summary>
    private void Awake()
    {
        // Получаем необходимые компоненты
        _canvasGroup = GetComponent<CanvasGroup>();
        _rect = GetComponent<RectTransform>();

        // Если ссылка на иконку не установлена в инспекторе, пытаемся найти её
        if (_icon == null) _icon = GetComponent<Image>();

        // Ищем корневой Canvas в родительских объектах
        _rootCanvas = GetComponentInParent<Canvas>();
    }

    /// <summary>
    /// Инициализирует предмет данными
    /// </summary>
    /// <param name="item">Данные предмета</param>
    public void Init(Item item)
    {
        RuntimeItem = item; // Сохраняем ссылку на данные предмета
        if (_icon != null)
        {
            _icon.sprite = item.Sprite;           // Устанавливаем спрайт
            _icon.preserveAspect = true;          // Сохраняем пропорции спрайта
        }
    }

    /// <summary>
    /// Обработчик начала перетаскивания
    /// </summary>
    public void OnBeginDrag(PointerEventData eventData)
    {
        // Запоминаем исходную позицию и родителя
        _dragStartPosition = _rect.position;
        OriginalParent = transform.parent;

        // Убеждаемся что у нас есть ссылка на корневой Canvas
        if (_rootCanvas == null) _rootCanvas = GetComponentInParent<Canvas>();

        // Перемещаем объект на верхний уровень иерархии Canvas
        transform.SetParent(_rootCanvas.transform, true);
        transform.SetAsLastSibling(); // Помещаем поверх других элементов

        // Настраиваем визуальные эффекты перетаскивания
        _canvasGroup.blocksRaycasts = false; // Отключаем блокировку лучей (чтобы можно было обнаружить слоты под предметом)
        _canvasGroup.alpha = 0.8f;           // Делаем предмет полупрозрачным
    }

    /// <summary>
    /// Обработчик процесса перетаскивания
    /// </summary>
    public void OnDrag(PointerEventData eventData)
    {
        // Обновляем позицию предмета в соответствии с позицией курсора/касания
        // Учитываем масштаб канваса для корректного позиционирования
        _rect.anchoredPosition += eventData.delta / _rootCanvas.scaleFactor;
    }

    /// <summary>
    /// Обработчик окончания перетаскивания
    /// </summary>
    public void OnEndDrag(PointerEventData eventData)
    {
        // Проверяем не был ли объект уничтожен во время перетаскивания
        if (this == null || gameObject == null) return;

        // Восстанавливаем нормальные настройки
        _canvasGroup.blocksRaycasts = true;
        _canvasGroup.alpha = 1f;

        // Пытаемся определить целевой слот
        bool handled = false;
        if (eventData.pointerEnter != null)
        {
            // Ищем компонент InventorySlot в целевом объекте или его родителях
            InventorySlot slot = eventData.pointerEnter.GetComponent<InventorySlot>() ??
                                eventData.pointerEnter.GetComponentInParent<InventorySlot>();

            // Если нашли слот, пытаемся поместить в него предмет
            if (slot != null)
            {
                handled = slot.TrySetItem(this);
            }
        }

        // Если не удалось обработать (не попали в слот или слот занят), возвращаем на место
        if (!handled)
        {
            ReturnToOriginalPosition();
        }
    }

    /// <summary>
    /// Возвращает предмет в исходную позицию
    /// </summary>
    public void ReturnToOriginalPosition()
    {
        if (OriginalParent != null)
        {
            // Возвращаем к исходному родителю и сбрасываем позицию
            transform.SetParent(OriginalParent, false);
            transform.localPosition = Vector3.zero;
        }
        else
        {
            // Если родитель не сохранен, возвращаем в исходную мировую позицию
            transform.position = _dragStartPosition;
        }
    }
}