using UnityEngine;

/// <summary>
/// Абстрактный базовый класс для всех предметов в игре.
/// Определяет общие свойства и поведение для всех типов предметов.
/// </summary>
public abstract class Item
{
    // Тип предмета (реализуется в производных классах)
    public abstract ItemType Type { get; }

    // Максимальный уровень для этого типа предмета
    public abstract int MaxLevel { get; }

    // Текущий уровень предмета (от 1 до MaxLevel)
    public int Level { get; protected set; }

    // Спрайт для отображения предмета в UI
    public Sprite Sprite { get; protected set; }

    /// <summary>
    /// Базовый конструктор для всех предметов
    /// </summary>
    /// <param name="level">Уровень создаваемого предмета</param>
    /// <param name="sprite">Спрайт для отображения</param>
    protected Item(int level, Sprite sprite)
    {
        // Ограничиваем уровень в допустимых пределах
        Level = Mathf.Clamp(level, 1, MaxLevel);
        Sprite = sprite;
    }

    /// <summary>
    /// Проверяет, может ли этот предмет объединиться с другим предметом
    /// </summary>
    /// <param name="other">Другой предмет для проверки</param>
    /// <returns>True если предметы могут объединиться</returns>
    public virtual bool CanMergeWith(Item other)
    {
        // Предметы могут объединиться если:
        return other != null &&                   // Другой предмет существует
               other.Type == this.Type &&         // Типы предметов совпадают
               other.Level == this.Level &&       // Уровни предметов совпадают
               this.Level < this.MaxLevel;        // Текущий предмет еще не достиг максимального уровня
    }
}