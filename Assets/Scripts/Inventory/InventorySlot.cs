using UnityEngine;

public class InventorySlot : MonoBehaviour
{
    public bool IsOccupied => transform.childCount > 0;

    public bool TrySetItem(GameObject item)
    {
        if (IsOccupied) return false;
        item.transform.SetParent(transform);
        item.transform.localPosition = Vector3.zero;
        return true;
    }
}
