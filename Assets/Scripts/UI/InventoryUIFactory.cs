using UnityEngine;

/// <summary>
/// Статический класс-фабрика для создания UI элементов инвентаря
/// Инкапсулирует логику создания и настройки визуальных представлений предметов
/// </summary>
public static class InventoryUIFactory
{
    // Путь к префабу предмета в папке Resources
    private const string PrefabPath = "UI/InventoryItem";

    /// <summary>
    /// Создает UI представление предмета
    /// </summary>
    /// <param name="item">Данные предмета</param>
    /// <param name="parent">Родительский transform (опционально)</param>
    /// <returns>Созданный GameObject или null при ошибке</returns>
    public static GameObject CreateItemUI(Item item, Transform parent = null)
    {
        try
        {
            // Загрузка префаба из папки Resources
            GameObject prefab = Resources.Load<GameObject>(PrefabPath);
            if (prefab == null)
            {
                Debug.LogError($"Префаб InventoryItem не найден по пути: Resources/{PrefabPath}");
                return null;
            }

            // Создание экземпляра префаба БЕЗ указания родителя
            // Родитель будет установлен позже в методе PlaceItem
            GameObject go = Object.Instantiate(prefab);
            go.transform.localPosition = Vector3.zero;
            go.transform.localScale = Vector3.one;

            // Получаем компонент InventoryItem
            InventoryItem ui = go.GetComponent<InventoryItem>();
            if (ui == null)
            {
                Debug.LogError("Компонент InventoryItem отсутствует на префабе");
                return go;
            }

            // Инициализируем компонент данными предмета
            ui.Init(item);

            return go;
        }
        catch (System.Exception ex)
        {
            // Обработка любых исключений в процессе создания
            Debug.LogError($"Ошибка создания UI предмета: {ex.Message}");

            // Создаем простой объект в случае ошибки
            return new GameObject($"ОшибочныйПредмет_{item.Type}");
        }
    }
}