using UnityEngine;

public class CombineOnOverlap : MonoBehaviour
{
    public enum ObjectType { Stick, Branch, Board }
    public ObjectType objectType;

    public GameObject prefabStick;
    public GameObject prefabBranch;
    public GameObject prefabBoard;

    private bool isCombining = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isCombining) return;

        CombineOnOverlap other = collision.GetComponent<CombineOnOverlap>();
        if (other == null) return;

        // Проверяем типы объектов для комбинирования
        if (other.objectType == this.objectType && (objectType == ObjectType.Stick || objectType == ObjectType.Branch))
        {
            isCombining = true;
            other.isCombining = true;

            // Определяем позицию для нового объекта (средняя точка)
            Vector3 newPosition = (transform.position + other.transform.position) / 2;

            GameObject newObject = null;

            // Логика объединения
            if (objectType == ObjectType.Stick)
            {
                // Две палочки -> ветка
                newObject = Instantiate(prefabBranch, newPosition, Quaternion.identity);
            }
            else if (objectType == ObjectType.Branch)
            {
                // Две ветки -> доска
                newObject = Instantiate(prefabBoard, newPosition, Quaternion.identity);
            }

            if (newObject != null)
            {
                // Удаляем исходные объекты
                Destroy(other.gameObject);
                Destroy(this.gameObject);
            }
        }
    }
}
