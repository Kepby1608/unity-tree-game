using UnityEngine;

/// <summary>
/// ����������� �����-������� ��� �������� ���������
/// ������������� ������ �������� ��������� ����� ���������
/// </summary>
public static class ItemFactory
{
    /// <summary>
    /// ������� ������� ���������� ���� � ������
    /// </summary>
    /// <param name="level">������� ������������ ��������</param>
    /// <param name="type">��� ������������ ��������</param>
    /// <param name="sprite">������������ ������ (���� �� ������, ����������� �� ���� ������)</param>
    /// <returns>��������� �������</returns>
    public static Item CreateItem(int level, ItemType type, Sprite sprite = null)
    {
        // ���� ������ �� ������ ����, ��������� ��� �� ���� ������
        // �������� ??= ���������, �������� �� sprite null, � ���� ��, �������� ��������� �� ���� ������
        sprite ??= ItemSpriteDatabase.Instance?.GetSprite(type, level);

        // ���������� switch expression ��� �������� �������� ������� ����
        return type switch
        {
            ItemType.Wood => new WoodItem(level, sprite),  // ������� ������� ���� Wood
            ItemType.Water => new WaterItem(level, sprite), // ������� ������� ���� Water
            _ => throw new System.ArgumentException($"Unknown item type: {type}") // ������ ��� ����������� ����
        };
    }
}