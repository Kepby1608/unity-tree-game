using UnityEngine;
/// <summary>
/// Конкретная реализация предмета "Дерево"
/// Определяет специфические свойства для предметов типа Wood
/// </summary>
public class WoodItem : Item
{
    // Тип предмета - Wood (дерево)
    public override ItemType Type => ItemType.Wood;

    // Максимальный уровень для предметов Wood - 6
    public override int MaxLevel => 6;

    /// <summary>
    /// Конструктор для создания предмета Wood
    /// </summary>
    /// <param name="level">Уровень предмета</param>
    /// <param name="sprite">Спрайт для отображения</param>
    public WoodItem(int level, Sprite sprite) : base(level, sprite) { }
}