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
        Debug.Log($"[CombineOnOverlap] {gameObject.name} ���������� � {collision.name}");

        if (isCombining)
        {
            Debug.Log("[CombineOnOverlap] ��� ��� ������� ����������� � �������");
            return;
        }

        CombineOnOverlap other = collision.GetComponent<CombineOnOverlap>();
        if (other == null)
        {
            Debug.Log("[CombineOnOverlap] ������ ������ �� ����� CombineOnOverlap");
            return;
        }

        Debug.Log($"[CombineOnOverlap] ��������� �����: {objectType} � {other.objectType}");

        if (other.objectType == this.objectType && (objectType == ObjectType.Stick || objectType == ObjectType.Branch))
        {
            Debug.Log("[CombineOnOverlap] ������� ��� ����������� ���������");

            isCombining = true;
            other.isCombining = true;

            Vector3 newPosition = (transform.position + other.transform.position) / 2;
            GameObject newObject = null;

            if (objectType == ObjectType.Stick)
            {
                Debug.Log("[CombineOnOverlap] ������ �����");
                newObject = Instantiate(prefabBranch, newPosition, Quaternion.identity);
            }
            else if (objectType == ObjectType.Branch)
            {
                Debug.Log("[CombineOnOverlap] ������ �����");
                newObject = Instantiate(prefabBoard, newPosition, Quaternion.identity);
            }

            if (newObject != null)
            {
                Destroy(other.gameObject);
                Destroy(this.gameObject);
            }
        }
        else
        {
            Debug.Log("[CombineOnOverlap] ���� �� ��������� ��� ��� �� ��������������");
        }
    }

}
