using UnityEngine;
/// <summary>
/// ���������� ���������� �������� "������"
/// ���������� ������������� �������� ��� ��������� ���� Wood
/// </summary>
public class WoodItem : Item
{
    // ��� �������� - Wood (������)
    public override ItemType Type => ItemType.Wood;

    // ������������ ������� ��� ��������� Wood - 6
    public override int MaxLevel => 6;

    /// <summary>
    /// ����������� ��� �������� �������� Wood
    /// </summary>
    /// <param name="level">������� ��������</param>
    /// <param name="sprite">������ ��� �����������</param>
    public WoodItem(int level, Sprite sprite) : base(level, sprite) { }
}