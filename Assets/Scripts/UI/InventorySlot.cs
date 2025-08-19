using System.Collections;
using UnityEngine;

/// <summary>
/// ����� �������������� ���� ���������
/// ��������� �����������, ��������� � ������������ ���������
/// </summary>
public class InventorySlot : MonoBehaviour
{
    // ���� ��� ������������ �������� �����������
    private bool IsMerging = false;

    /// <summary>
    /// ��������� ����� �� ����
    /// ���� ��������� �������, ���� � ��� ���� �������� ������ � ����������� InventoryItem
    /// </summary>
    public bool IsOccupied => transform.childCount > 0 && transform.GetChild(0).GetComponent<InventoryItem>() != null;

    /// <summary>
    /// �������� ���������� ������� � �����
    /// </summary>
    /// <param name="newItem">������� ��� ����������</param>
    /// <returns>True ���� ������� ������� ��������</returns>
    public bool TrySetItem(InventoryItem newItem)
    {
        if (newItem == null)
        {
            Debug.LogWarning("������� ���������� ������ ������� � �����");
            return false;
        }

        // �������� ���������� � ����������� ��������
        Debug.Log($"������� ���������� �������: {newItem.RuntimeItem.Type} ������ {newItem.RuntimeItem.Level}");

        // ���� ���� ������ - ������ ��������� �������
        if (!IsOccupied)
        {
            PlaceItem(newItem);
            Debug.Log("������� �������� � ������ �����");
            return true;
        }

        // �������� ������� ��� ����������� � �����
        InventoryItem existingItem = transform.GetChild(0).GetComponent<InventoryItem>();

        // �������� ���������� � ������������ ��������
        Debug.Log($"� ����� ��� ���� �������: {existingItem.RuntimeItem.Type} ������ {existingItem.RuntimeItem.Level}");

        // ��������� ����� �� ���������� ��������
        if (existingItem != null && existingItem.RuntimeItem.CanMergeWith(newItem.RuntimeItem))
        {
            Debug.Log("�������� ����� ���� ����������");

            // ������������� ���� �����������
            IsMerging = true;

            // ���������� ��������
            MergeItems(existingItem, newItem);

            // ���������� ���� ����� ����������
            IsMerging = false;

            return true;
        }

        // ���� ����� � ������� ����������
        Debug.Log("���� �����, ������� ����������");
        return false;
    }

    /// <summary>
    /// ��������� ������� � �����
    /// </summary>
    /// <param name="item">������� ��� ����������</param>
    private void PlaceItem(InventoryItem item)
    {
        // �� ��������� �������, ���� ���������� �����������
        if (IsMerging) return;

        // ������� ���������� ���� (���� ������� ������������ �� ������� �����)
        if (item.transform.parent != null)
        {
            var prevSlot = item.transform.parent.GetComponent<InventorySlot>();
            if (prevSlot != null && prevSlot != this)
            {
                // �� ������� ����, ���� � ��� ���������� �����������
                if (!prevSlot.IsMerging)
                {
                    prevSlot.ClearSlot();
                }
            }
        }

        // ������������� ����� �������� � �������
        item.transform.SetParent(transform, false);
        item.transform.localPosition = Vector3.zero;
    }

    /// <summary>
    /// ���������� ��� ��������
    /// </summary>
    /// <param name="existing">������� ��� ����������� � �����</param>
    /// <param name="newItem">����� ������� ��� �����������</param>
    private void MergeItems(InventoryItem existing, InventoryItem newItem)
    {
        // �������� ������� �����������
        Debug.Log($"�������� �����������: {existing.RuntimeItem.Type} ������ {existing.RuntimeItem.Level} + {newItem.RuntimeItem.Type} ������ {newItem.RuntimeItem.Level}");

        // ��������� ������������ ����� ������� (������� ������������� �������� + 1)
        int newLevel = existing.RuntimeItem.Level + 1;
        Debug.Log($"����� ������� ����� �����������: {newLevel}");

        // �������� ������ ��� ������ ������
        Sprite newSprite = ItemSpriteDatabase.Instance?.GetSprite(existing.RuntimeItem.Type, newLevel);

        if (newSprite == null)
        {
            Debug.LogError($"�� ������ ������ ��� {existing.RuntimeItem.Type} ������ {newLevel}");
            return;
        }

        // ������� ������������ �������
        Item mergedItem = ItemFactory.CreateItem(newLevel, existing.RuntimeItem.Type, newSprite);

        // �������� �������� ������ ��������
        Debug.Log($"������ ������������ �������: {mergedItem.Type} ������ {mergedItem.Level}");

        // ������� ������� ����� �������
        GameObject mergedGO = InventoryUIFactory.CreateItemUI(mergedItem);

        // ������������� ����� ������� � ������� ����
        mergedGO.transform.SetParent(transform, false);
        mergedGO.transform.localPosition = Vector3.zero;

        // �������� ��������� InventoryItem ������ ��������
        InventoryItem mergedInventoryItem = mergedGO.GetComponent<InventoryItem>();

        // ������ ����� ��������� �������� ������ �������� ���������� ������
        Destroy(existing.gameObject);
        Destroy(newItem.gameObject);

        Debug.Log($"�������� ������� ����������! ������ ������� ������ {newLevel}");

        // �������������� �������� ����� 0.1 �������
        StartCoroutine(VerifySlotAfterMerge());
    }

    /// <summary>
    /// �������������� �������� ����� ����� �����������
    /// ����������, ��� ������� ������� � ����� ����� ���� ��������
    /// </summary>
    private IEnumerator VerifySlotAfterMerge()
    {
        // ���� ���������� �����
        yield return new WaitForEndOfFrame();

        // ���������, ������� �� ������� � �����
        if (transform.childCount == 0)
        {
            Debug.LogWarning("������� ����� �� ����� ����� �����������! ��������, ��� ������ ������ �������.");
        }
        else
        {
            Debug.Log($"������� ������� � �����: {transform.GetChild(0).name}");
        }
    }

    /// <summary>
    /// ������� ���� (������� ��� �������� �������)
    /// </summary>
    public void ClearSlot()
    {
        // �� ������� ����, ���� � ��� ���������� �����������
        if (IsMerging) return;

        if (transform.childCount > 0)
        {
            Destroy(transform.GetChild(0).gameObject);
        }
    }
}