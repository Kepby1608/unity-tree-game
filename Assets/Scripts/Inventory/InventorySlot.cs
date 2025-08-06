using UnityEngine;

public class InventorySlot : MonoBehaviour
{
    public bool IsOccupied => transform.childCount > 0;

    public bool TrySetItem(GameObject item)
    {
        Debug.Log($"[InventorySlot] Попытка поместить {item.name} в слот {name}");

        if (!IsOccupied)
        {
            Debug.Log("[InventorySlot] Слот пуст — просто кладём предмет");
            item.transform.SetParent(transform);
            item.transform.localPosition = Vector3.zero;
            return true;
        }

        Debug.Log("[InventorySlot] Слот занят — пробуем объединить");

        var existingItem = transform.GetChild(0).GetComponent<CombineOnOverlap>();
        var newItem = item.GetComponent<CombineOnOverlap>();

        if (existingItem != null && newItem != null)
        {
            Debug.Log($"[InventorySlot] Сравнение типов: {existingItem.objectType} и {newItem.objectType}");

            if (existingItem.objectType == CombineOnOverlap.ObjectType.Stick && newItem.objectType == CombineOnOverlap.ObjectType.Stick)
            {
                Debug.Log("[InventorySlot] Две палки — создаём ветку");
                GameObject combined = Instantiate(existingItem.prefabBranch, transform.position, Quaternion.identity);
                combined.transform.SetParent(transform);
                combined.transform.localPosition = Vector3.zero;

                existingItem.transform.SetParent(null);
                newItem.transform.SetParent(null);

                Destroy(existingItem.gameObject);
                Destroy(newItem.gameObject);
                return true;
            }

            else if (existingItem.objectType == CombineOnOverlap.ObjectType.Branch &&
                     newItem.objectType == CombineOnOverlap.ObjectType.Branch)
            {
                Debug.Log("[InventorySlot] Две ветки — создаём доску");
                GameObject combined = Instantiate(existingItem.prefabBoard, transform.position, Quaternion.identity);
                combined.transform.SetParent(transform);
                combined.transform.localPosition = Vector3.zero;

                Destroy(existingItem.gameObject);
                Destroy(newItem.gameObject);
                return true;
            }
        }
        else
        {
            Debug.Log("[InventorySlot] Один или оба объекта не имеют CombineOnOverlap");
        }

        Debug.Log("[InventorySlot] Объединение не удалось");
        return false;
    }


}
