using UnityEngine;

/// <summary>
/// Конкретная реализация предмета "Вода"
/// Определяет специфические свойства для предметов типа Water
/// </summary>
public class WaterItem : Item
{
    // Тип предмета - Water (вода)
    public override ItemType Type => ItemType.Water;

    // Максимальный уровень для предметов Water - 4
    public override int MaxLevel => 4;

    /// <summary>
    /// Конструктор для создания предмета Water
    /// </summary>
    /// <param name="level">Уровень предмета</param>
    /// <param name="sprite">Спрайт для отображения</param>
    public WaterItem(int level, Sprite sprite) : base(level, sprite) { }
}