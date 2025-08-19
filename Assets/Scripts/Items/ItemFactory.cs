using UnityEngine;

/// <summary>
/// Статический класс-фабрика для создания предметов
/// Инкапсулирует логику создания различных типов предметов
/// </summary>
public static class ItemFactory
{
    /// <summary>
    /// Создает предмет указанного типа и уровня
    /// </summary>
    /// <param name="level">Уровень создаваемого предмета</param>
    /// <param name="type">Тип создаваемого предмета</param>
    /// <param name="sprite">Опциональный спрайт (если не указан, загружается из базы данных)</param>
    /// <returns>Созданный предмет</returns>
    public static Item CreateItem(int level, ItemType type, Sprite sprite = null)
    {
        // Если спрайт не указан явно, загружаем его из базы данных
        // Оператор ??= проверяет, является ли sprite null, и если да, пытается загрузить из базы данных
        sprite ??= ItemSpriteDatabase.Instance?.GetSprite(type, level);

        // Используем switch expression для создания предмета нужного типа
        return type switch
        {
            ItemType.Wood => new WoodItem(level, sprite),  // Создаем предмет типа Wood
            ItemType.Water => new WaterItem(level, sprite), // Создаем предмет типа Water
            _ => throw new System.ArgumentException($"Unknown item type: {type}") // Ошибка при неизвестном типе
        };
    }
}