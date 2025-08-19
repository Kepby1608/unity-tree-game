using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class ItemInventorySetupEditor
{
    [MenuItem("Tools/Setup/Create InventoryItem prefab & Database")]
    public static void CreateInventoryAssets()
    {
        // ��������� ������������� ���� ������
        var existingDB = AssetDatabase.LoadAssetAtPath<ItemSpriteDatabase>("Assets/Resources/ItemSpriteDatabase.asset");
        if (existingDB == null)
        {
            // ������� ���� ������ ������ ���� ��� �� ����������
            if (!AssetDatabase.IsValidFolder("Assets/Resources"))
                AssetDatabase.CreateFolder("Assets", "Resources");

            var db = ScriptableObject.CreateInstance<ItemSpriteDatabase>();
            AssetDatabase.CreateAsset(db, "Assets/Resources/ItemSpriteDatabase.asset");

            // ������������� ��������� ����� ������
            db.EnsureAllTypes();
            EditorUtility.SetDirty(db);

            Debug.Log("Created ItemSpriteDatabase at Assets/Resources/ItemSpriteDatabase.asset");
        }
        else
        {
            Debug.Log("ItemSpriteDatabase already exists");
        }

        // ��������� ������������� �������
        var existingPrefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Resources/UI/InventoryItem.prefab");
        if (existingPrefab == null)
        {
            // ������� ����� UI ���� �����
            if (!AssetDatabase.IsValidFolder("Assets/Resources/UI"))
                AssetDatabase.CreateFolder("Assets/Resources", "UI");

            // ������� ������
            GameObject go = new GameObject("InventoryItem",
                typeof(RectTransform),
                typeof(CanvasGroup),
                typeof(Image),
                typeof(InventoryItem));

            RectTransform rect = go.GetComponent<RectTransform>();
            rect.sizeDelta = new Vector2(100, 100);

            Image img = go.GetComponent<Image>();
            img.raycastTarget = true;

            // ����������� ������ �� ������
            var inv = go.GetComponent<InventoryItem>();
            var so = new SerializedObject(inv);
            var prop = so.FindProperty("_icon");
            if (prop != null)
            {
                prop.objectReferenceValue = img;
                so.ApplyModifiedProperties();
            }

            string prefabPath = "Assets/Resources/UI/InventoryItem.prefab";
            PrefabUtility.SaveAsPrefabAsset(go, prefabPath);
            GameObject.DestroyImmediate(go);

            Debug.Log("Created InventoryItem prefab at " + prefabPath);
        }
        else
        {
            Debug.Log("InventoryItem prefab already exists");
        }

        // �������� ���� ������ ��� ��������������
        Selection.activeObject = AssetDatabase.LoadAssetAtPath<ItemSpriteDatabase>("Assets/Resources/ItemSpriteDatabase.asset");
    }
}