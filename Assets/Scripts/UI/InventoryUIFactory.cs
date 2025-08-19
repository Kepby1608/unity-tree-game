using UnityEngine;

/// <summary>
/// ����������� �����-������� ��� �������� UI ��������� ���������
/// ������������� ������ �������� � ��������� ���������� ������������� ���������
/// </summary>
public static class InventoryUIFactory
{
    // ���� � ������� �������� � ����� Resources
    private const string PrefabPath = "UI/InventoryItem";

    /// <summary>
    /// ������� UI ������������� ��������
    /// </summary>
    /// <param name="item">������ ��������</param>
    /// <param name="parent">������������ transform (�����������)</param>
    /// <returns>��������� GameObject ��� null ��� ������</returns>
    public static GameObject CreateItemUI(Item item, Transform parent = null)
    {
        try
        {
            // �������� ������� �� ����� Resources
            GameObject prefab = Resources.Load<GameObject>(PrefabPath);
            if (prefab == null)
            {
                Debug.LogError($"������ InventoryItem �� ������ �� ����: Resources/{PrefabPath}");
                return null;
            }

            // �������� ���������� ������� ��� �������� ��������
            // �������� ����� ���������� ����� � ������ PlaceItem
            GameObject go = Object.Instantiate(prefab);
            go.transform.localPosition = Vector3.zero;
            go.transform.localScale = Vector3.one;

            // �������� ��������� InventoryItem
            InventoryItem ui = go.GetComponent<InventoryItem>();
            if (ui == null)
            {
                Debug.LogError("��������� InventoryItem ����������� �� �������");
                return go;
            }

            // �������������� ��������� ������� ��������
            ui.Init(item);

            return go;
        }
        catch (System.Exception ex)
        {
            // ��������� ����� ���������� � �������� ��������
            Debug.LogError($"������ �������� UI ��������: {ex.Message}");

            // ������� ������� ������ � ������ ������
            return new GameObject($"����������������_{item.Type}");
        }
    }
}