using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// ��������������� ����� ��� ������������ ������� ���������
/// ��������� ��������� �������� �� ������� ������
/// </summary>
public class TestSpawner : MonoBehaviour
{
    [SerializeField] private Transform slotsRoot;     // ������������ ������ ��� ������ ���������

    /// <summary>
    /// ���������� ������ ���� ��� ��������� �����
    /// ��������� ������ �� �������������� �������
    /// </summary>
    void Update()
    {
        // �������� ������ �� ����������
        var keyboard = Keyboard.current;

        // ������������ ������� ������ � ������� �� �������������� ������������
        if (keyboard.qKey.wasPressedThisFrame)
        {
            Spawn(ItemType.Wood);
        }
        if (keyboard.wKey.wasPressedThisFrame)
        {
            Spawn(ItemType.Water);
        }
    }

    /// <summary>
    /// ������� ������� ���������� ���� � ������ ��������� �����
    /// </summary>
    /// <param name="type">��� ������������ ��������</param>
    private void Spawn(ItemType type)
    {
        // ������ �� ����� - ���������, �� �������������� �� ��� �������� ��������
        if (IsSpawning) return;

        // ������������� ����, ��� ������ �������� ��������
        IsSpawning = true;

        try
        {
            // ���� ������ ��������� ����
            foreach (Transform slotTransform in slotsRoot)
            {
                var slot = slotTransform.GetComponent<InventorySlot>();
                if (slot != null && !slot.IsOccupied)
                {
                    // ������� �������
                    var item = ItemFactory.CreateItem(1, type);

                    // �������� �������� ��������
                    Debug.Log($"������ �������: {type} ������� {item.Level}");

                    // ������� UI ������������� �������� ��� �������� ��������
                    GameObject itemGO = InventoryUIFactory.CreateItemUI(item);

                    // �������� ��������� InventoryItem
                    InventoryItem inventoryItem = itemGO.GetComponent<InventoryItem>();
                    if (inventoryItem == null)
                    {
                        Debug.LogError("�� ������� �������� ��������� InventoryItem");
                        Destroy(itemGO);
                        return;
                    }

                    // ��������� ������� � �����
                    if (slot.TrySetItem(inventoryItem))
                    {
                        Debug.Log($"������� �������� � ����� {slotTransform.name}");
                    }
                    else
                    {
                        Debug.LogWarning($"�� ������� ���������� ������� � ����� {slotTransform.name}");
                        Destroy(itemGO);
                    }

                    return;
                }
            }

            // ���� �� ����� ��������� ����
            Debug.Log("�� ������� ��������� ������");
        }
        finally
        {
            // ������� ���� �������� ��������
            IsSpawning = false;
        }
    }

    // ���� ��� ������ �� �������������� �������� ���������
    private bool IsSpawning = false;
}