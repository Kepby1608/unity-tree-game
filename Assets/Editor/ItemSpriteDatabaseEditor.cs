using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ItemSpriteDatabase))]
public class ItemSpriteDatabaseEditor : Editor
{
    public override void OnInspectorGUI()
    {
        // Отрисовываем стандартный инспектор
        DrawDefaultInspector();

        // Добавляем кастомную кнопку
        ItemSpriteDatabase database = (ItemSpriteDatabase)target;

        if (GUILayout.Button("Ensure All Types"))
        {
            database.EnsureAllTypes();
            EditorUtility.SetDirty(database); // Помечаем объект как измененный
        }

        // Кнопка для очистки пустых записей (опционально)
        if (GUILayout.Button("Remove Empty Types"))
        {
            database.items.RemoveAll(item => item.levelSprites.Count == 0);
            EditorUtility.SetDirty(database);
        }
    }
}