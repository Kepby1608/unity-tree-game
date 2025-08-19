using System.Collections;
using UnityEngine;

/// <summary>
/// Класс представляющий слот инвентаря
/// Управляет размещением, хранением и объединением предметов
/// </summary>
public class InventorySlot : MonoBehaviour
{
    // Флаг для отслеживания процесса объединения
    private bool IsMerging = false;

    /// <summary>
    /// Проверяет занят ли слот
    /// Слот считается занятым, если в нем есть дочерний объект с компонентом InventoryItem
    /// </summary>
    public bool IsOccupied => transform.childCount > 0 && transform.GetChild(0).GetComponent<InventoryItem>() != null;

    /// <summary>
    /// Пытается разместить предмет в слоте
    /// </summary>
    /// <param name="newItem">Предмет для размещения</param>
    /// <returns>True если предмет успешно размещен</returns>
    public bool TrySetItem(InventoryItem newItem)
    {
        if (newItem == null)
        {
            Debug.LogWarning("Попытка разместить пустой предмет в слоте");
            return false;
        }

        // Логируем информацию о размещаемом предмете
        Debug.Log($"Попытка разместить предмет: {newItem.RuntimeItem.Type} уровня {newItem.RuntimeItem.Level}");

        // Если слот пустой - просто размещаем предмет
        if (!IsOccupied)
        {
            PlaceItem(newItem);
            Debug.Log("Предмет размещен в пустом слоте");
            return true;
        }

        // Получаем предмет уже находящийся в слоте
        InventoryItem existingItem = transform.GetChild(0).GetComponent<InventoryItem>();

        // Логируем информацию о существующем предмете
        Debug.Log($"В слоте уже есть предмет: {existingItem.RuntimeItem.Type} уровня {existingItem.RuntimeItem.Level}");

        // Проверяем можно ли объединить предметы
        if (existingItem != null && existingItem.RuntimeItem.CanMergeWith(newItem.RuntimeItem))
        {
            Debug.Log("Предметы могут быть объединены");

            // Устанавливаем флаг объединения
            IsMerging = true;

            // Объединяем предметы
            MergeItems(existingItem, newItem);

            // Сбрасываем флаг после завершения
            IsMerging = false;

            return true;
        }

        // Слот занят и слияние невозможно
        Debug.Log("Слот занят, слияние невозможно");
        return false;
    }

    /// <summary>
    /// Размещает предмет в слоте
    /// </summary>
    /// <param name="item">Предмет для размещения</param>
    private void PlaceItem(InventoryItem item)
    {
        // Не размещаем предмет, если происходит объединение
        if (IsMerging) return;

        // Очищаем предыдущий слот (если предмет перемещается из другого слота)
        if (item.transform.parent != null)
        {
            var prevSlot = item.transform.parent.GetComponent<InventorySlot>();
            if (prevSlot != null && prevSlot != this)
            {
                // Не очищаем слот, если в нем происходит объединение
                if (!prevSlot.IsMerging)
                {
                    prevSlot.ClearSlot();
                }
            }
        }

        // Устанавливаем новый родитель и позицию
        item.transform.SetParent(transform, false);
        item.transform.localPosition = Vector3.zero;
    }

    /// <summary>
    /// Объединяет два предмета
    /// </summary>
    /// <param name="existing">Предмет уже находящийся в слоте</param>
    /// <param name="newItem">Новый предмет для объединения</param>
    private void MergeItems(InventoryItem existing, InventoryItem newItem)
    {
        // Логируем процесс объединения
        Debug.Log($"Начинаем объединение: {existing.RuntimeItem.Type} уровня {existing.RuntimeItem.Level} + {newItem.RuntimeItem.Type} уровня {newItem.RuntimeItem.Level}");

        // Правильно рассчитываем новый уровень (уровень существующего предмета + 1)
        int newLevel = existing.RuntimeItem.Level + 1;
        Debug.Log($"Новый уровень после объединения: {newLevel}");

        // Получаем спрайт для нового уровня
        Sprite newSprite = ItemSpriteDatabase.Instance?.GetSprite(existing.RuntimeItem.Type, newLevel);

        if (newSprite == null)
        {
            Debug.LogError($"Не найден спрайт для {existing.RuntimeItem.Type} уровня {newLevel}");
            return;
        }

        // Создаем объединенный предмет
        Item mergedItem = ItemFactory.CreateItem(newLevel, existing.RuntimeItem.Type, newSprite);

        // Логируем создание нового предмета
        Debug.Log($"Создан объединенный предмет: {mergedItem.Type} уровня {mergedItem.Level}");

        // Сначала создаем новый предмет
        GameObject mergedGO = InventoryUIFactory.CreateItemUI(mergedItem);

        // Устанавливаем новый предмет в текущий слот
        mergedGO.transform.SetParent(transform, false);
        mergedGO.transform.localPosition = Vector3.zero;

        // Получаем компонент InventoryItem нового предмета
        InventoryItem mergedInventoryItem = mergedGO.GetComponent<InventoryItem>();

        // Только после успешного создания нового предмета уничтожаем старые
        Destroy(existing.gameObject);
        Destroy(newItem.gameObject);

        Debug.Log($"Предметы успешно объединены! Создан предмет уровня {newLevel}");

        // Дополнительная проверка через 0.1 секунду
        StartCoroutine(VerifySlotAfterMerge());
    }

    /// <summary>
    /// Дополнительная проверка слота после объединения
    /// Убеждается, что предмет остался в слоте после всех операций
    /// </summary>
    private IEnumerator VerifySlotAfterMerge()
    {
        // Ждем завершения кадра
        yield return new WaitForEndOfFrame();

        // Проверяем, остался ли предмет в слоте
        if (transform.childCount == 0)
        {
            Debug.LogWarning("Предмет исчез из слота после объединения! Возможно, его удалил другой процесс.");
        }
        else
        {
            Debug.Log($"Предмет остался в слоте: {transform.GetChild(0).name}");
        }
    }

    /// <summary>
    /// Очищает слот (удаляет все дочерние объекты)
    /// </summary>
    public void ClearSlot()
    {
        // Не очищаем слот, если в нем происходит объединение
        if (IsMerging) return;

        if (transform.childCount > 0)
        {
            Destroy(transform.GetChild(0).gameObject);
        }
    }
}