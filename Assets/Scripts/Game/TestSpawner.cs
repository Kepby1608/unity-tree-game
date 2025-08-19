using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Вспомогательный класс для тестирования системы инвентаря
/// Позволяет создавать предметы по нажатию клавиш
/// </summary>
public class TestSpawner : MonoBehaviour
{
    [SerializeField] private Transform slotsRoot;     // Родительский объект для слотов инвентаря

    /// <summary>
    /// Вызывается каждый кадр для обработки ввода
    /// Добавляем защиту от множественного нажатия
    /// </summary>
    void Update()
    {
        // Получаем ссылку на клавиатуру
        var keyboard = Keyboard.current;

        // Обрабатываем нажатия клавиш с защитой от множественного срабатывания
        if (keyboard.qKey.wasPressedThisFrame)
        {
            Spawn(ItemType.Wood);
        }
        if (keyboard.wKey.wasPressedThisFrame)
        {
            Spawn(ItemType.Water);
        }
    }

    /// <summary>
    /// Создает предмет указанного типа в первом свободном слоте
    /// </summary>
    /// <param name="type">Тип создаваемого предмета</param>
    private void Spawn(ItemType type)
    {
        // Защита от спама - проверяем, не обрабатывается ли уже создание предмета
        if (IsSpawning) return;

        // Устанавливаем флаг, что начали создание предмета
        IsSpawning = true;

        try
        {
            // Ищем первый свободный слот
            foreach (Transform slotTransform in slotsRoot)
            {
                var slot = slotTransform.GetComponent<InventorySlot>();
                if (slot != null && !slot.IsOccupied)
                {
                    // Создаем предмет
                    var item = ItemFactory.CreateItem(1, type);

                    // Логируем создание предмета
                    Debug.Log($"Создан предмет: {type} уровень {item.Level}");

                    // Создаем UI представление предмета БЕЗ указания родителя
                    GameObject itemGO = InventoryUIFactory.CreateItemUI(item);

                    // Получаем компонент InventoryItem
                    InventoryItem inventoryItem = itemGO.GetComponent<InventoryItem>();
                    if (inventoryItem == null)
                    {
                        Debug.LogError("Не удалось получить компонент InventoryItem");
                        Destroy(itemGO);
                        return;
                    }

                    // Размещаем предмет в слоте
                    if (slot.TrySetItem(inventoryItem))
                    {
                        Debug.Log($"Предмет размещен в слоте {slotTransform.name}");
                    }
                    else
                    {
                        Debug.LogWarning($"Не удалось разместить предмет в слоте {slotTransform.name}");
                        Destroy(itemGO);
                    }

                    return;
                }
            }

            // Если не нашли свободный слот
            Debug.Log("Не найдено свободных слотов");
        }
        finally
        {
            // Снимаем флаг создания предмета
            IsSpawning = false;
        }
    }

    // Флаг для защиты от множественного создания предметов
    private bool IsSpawning = false;
}