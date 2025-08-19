using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class ItemInventorySetupEditor
{
    [MenuItem("Tools/Setup/Create InventoryItem prefab & Database")]
    public static void CreateInventoryAssets()
    {
        // Проверяем существование базы данных
        var existingDB = AssetDatabase.LoadAssetAtPath<ItemSpriteDatabase>("Assets/Resources/ItemSpriteDatabase.asset");
        if (existingDB == null)
        {
            // Создаем базу данных только если она не существует
            if (!AssetDatabase.IsValidFolder("Assets/Resources"))
                AssetDatabase.CreateFolder("Assets", "Resources");

            var db = ScriptableObject.CreateInstance<ItemSpriteDatabase>();
            AssetDatabase.CreateAsset(db, "Assets/Resources/ItemSpriteDatabase.asset");

            // Автоматически заполняем всеми типами
            db.EnsureAllTypes();
            EditorUtility.SetDirty(db);

            Debug.Log("Created ItemSpriteDatabase at Assets/Resources/ItemSpriteDatabase.asset");
        }
        else
        {
            Debug.Log("ItemSpriteDatabase already exists");
        }

        // Проверяем существование префаба
        var existingPrefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Resources/UI/InventoryItem.prefab");
        if (existingPrefab == null)
        {
            // Создаем папку UI если нужно
            if (!AssetDatabase.IsValidFolder("Assets/Resources/UI"))
                AssetDatabase.CreateFolder("Assets/Resources", "UI");

            // Создаем префаб
            GameObject go = new GameObject("InventoryItem",
                typeof(RectTransform),
                typeof(CanvasGroup),
                typeof(Image),
                typeof(InventoryItem));

            RectTransform rect = go.GetComponent<RectTransform>();
            rect.sizeDelta = new Vector2(100, 100);

            Image img = go.GetComponent<Image>();
            img.raycastTarget = true;

            // Настраиваем ссылку на иконку
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

        // Выбираем базу данных для редактирования
        Selection.activeObject = AssetDatabase.LoadAssetAtPath<ItemSpriteDatabase>("Assets/Resources/ItemSpriteDatabase.asset");
    }
}