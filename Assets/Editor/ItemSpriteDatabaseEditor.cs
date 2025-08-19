using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ItemSpriteDatabase))]
public class ItemSpriteDatabaseEditor : Editor
{
    public override void OnInspectorGUI()
    {
        // ������������ ����������� ���������
        DrawDefaultInspector();

        // ��������� ��������� ������
        ItemSpriteDatabase database = (ItemSpriteDatabase)target;

        if (GUILayout.Button("Ensure All Types"))
        {
            database.EnsureAllTypes();
            EditorUtility.SetDirty(database); // �������� ������ ��� ����������
        }

        // ������ ��� ������� ������ ������� (�����������)
        if (GUILayout.Button("Remove Empty Types"))
        {
            database.items.RemoveAll(item => item.levelSprites.Count == 0);
            EditorUtility.SetDirty(database);
        }
    }
}