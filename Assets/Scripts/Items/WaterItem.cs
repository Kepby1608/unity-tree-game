using UnityEngine;

/// <summary>
/// ���������� ���������� �������� "����"
/// ���������� ������������� �������� ��� ��������� ���� Water
/// </summary>
public class WaterItem : Item
{
    // ��� �������� - Water (����)
    public override ItemType Type => ItemType.Water;

    // ������������ ������� ��� ��������� Water - 4
    public override int MaxLevel => 4;

    /// <summary>
    /// ����������� ��� �������� �������� Water
    /// </summary>
    /// <param name="level">������� ��������</param>
    /// <param name="sprite">������ ��� �����������</param>
    public WaterItem(int level, Sprite sprite) : base(level, sprite) { }
}