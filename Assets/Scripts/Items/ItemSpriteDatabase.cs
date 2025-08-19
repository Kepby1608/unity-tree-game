using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// База данных спрайтов для предметов инвентаря
/// Хранит соответствие между типами предметов, их уровнями и спрайтами для отображения
/// </summary>
[CreateAssetMenu(fileName = "ItemSpriteDatabase", menuName = "Game/Item Sprite Database")]
public class ItemSpriteDatabase : ScriptableObject
{
    /// <summary>
    /// Статическое свойство для доступа к экземпляру базы данных
    /// Реализует паттерн Singleton для ScriptableObject
    /// </summary>
    public static ItemSpriteDatabase Instance
    {
        get
        {
            // Проверяем, не загружен ли уже экземпляр базы данных
            if (_instance == null)
                // Если не загружен, загружаем из папки Resources по указанному пути
                _instance = Resources.Load<ItemSpriteDatabase>("ItemSpriteDatabase");

            // Если база данных все еще не найдена, выводим ошибку
            if (_instance == null)
                Debug.LogError("База данных спрайтов не найдена! Убедитесь, что файл ItemSpriteDatabase.asset находится в папке Resources");

            return _instance;
        }
    }
    private static ItemSpriteDatabase _instance;

    /// <summary>
    /// Вложенный класс для хранения данных о спрайтах одного типа предмета
    /// </summary>
    [System.Serializable]
    public class ItemSpriteData
    {
        public ItemType type; // Тип предмета

        // Список спрайтов для каждого уровня предмета данного типа
        // Индекс в списке соответствует уровню предмета:
        // levelSprites[0] - спрайт для уровня 1
        // levelSprites[1] - спрайт для уровня 2
        // levelSprites[2] - спрайт для уровня 3
        // и т.д.
        public List<Sprite> levelSprites = new List<Sprite>();
    }

    // Список всех данных о спрайтах для различных типов предметов
    public List<ItemSpriteData> items = new List<ItemSpriteData>();

    /// <summary>
    /// Метод для получения спрайта по типу и уровню предмета
    /// </summary>
    /// <param name="type">Тип предмета</param>
    /// <param name="level">Уровень предмета (должен быть ≥ 1)</param>
    /// <returns>Спрайт для указанного типа и уровня предмета или null, если не найден</returns>
    public Sprite GetSprite(ItemType type, int level)
    {
        // Проверяем валидность уровня
        if (level < 1)
        {
            Debug.LogError($"Запрошен недопустимый уровень: {level}. Уровень должен быть ≥ 1");
            return null;
        }

        // Ищем данные о спрайтах для указанного типа предмета
        var data = items.Find(i => i.type == type);

        // Проверяем, найдены ли данные для указанного типа
        if (data == null)
        {
            Debug.LogError($"Не найдены данные для типа предмета: {type}");
            return null;
        }

        // Вычисляем индекс в списке (уровень - 1, так как индексы начинаются с 0)
        int spriteIndex = level - 1;

        // Проверяем, существует ли спрайт для запрошенного уровня
        if (spriteIndex < 0 || spriteIndex >= data.levelSprites.Count)
        {
            Debug.LogError($"Не найден спрайт для {type} уровня {level}. " +
                          $"Доступно спрайтов: {data.levelSprites.Count}");
            return null;
        }

        // Получаем спрайт из списка
        Sprite sprite = data.levelSprites[spriteIndex];

        // Проверяем, что спрайт не null
        if (sprite == null)
        {
            Debug.LogError($"Спрайт для {type} уровня {level} равен null");
            return null;
        }

        return sprite;
    }

    /// <summary>
    /// Вспомогательный метод для получения максимального доступного уровня для типа предмета
    /// </summary>
    /// <param name="type">Тип предмета</param>
    /// <returns>Максимальный уровень, для которого есть спрайт</returns>
    public int GetMaxAvailableLevel(ItemType type)
    {
        var data = items.Find(i => i.type == type);
        return data != null ? data.levelSprites.Count : 0;
    }

    /// <summary>
    /// Вспомогательный метод для редактора
    /// Автоматически добавляет записи для всех существующих типов предметов
    /// </summary>
    public void EnsureAllTypes()
    {
        // Перебираем все значения перечисления ItemType
        foreach (ItemType t in System.Enum.GetValues(typeof(ItemType)))
        {
            // Проверяем, существует ли уже запись для данного типа предмета
            if (!items.Exists(x => x.type == t))
            {
                // Если запись не существует, добавляем новую пустую запись
                items.Add(new ItemSpriteData
                {
                    type = t,
                    levelSprites = new List<Sprite>()
                });
                Debug.Log($"Добавлена запись для типа предмета: {t}");
            }
        }
    }
}