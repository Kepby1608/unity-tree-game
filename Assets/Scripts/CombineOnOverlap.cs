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

        // ��������� ���� �������� ��� ��������������
        if (other.objectType == this.objectType && (objectType == ObjectType.Stick || objectType == ObjectType.Branch))
        {
            isCombining = true;
            other.isCombining = true;

            // ���������� ������� ��� ������ ������� (������� �����)
            Vector3 newPosition = (transform.position + other.transform.position) / 2;

            GameObject newObject = null;

            // ������ �����������
            if (objectType == ObjectType.Stick)
            {
                // ��� ������� -> �����
                newObject = Instantiate(prefabBranch, newPosition, Quaternion.identity);
            }
            else if (objectType == ObjectType.Branch)
            {
                // ��� ����� -> �����
                newObject = Instantiate(prefabBoard, newPosition, Quaternion.identity);
            }

            if (newObject != null)
            {
                // ������� �������� �������
                Destroy(other.gameObject);
                Destroy(this.gameObject);
            }
        }
    }
}
