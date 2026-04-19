using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(FallingObjectsPatternSO))]
public class PatternEditor : Editor
{
    private const int size = 10;
    private const int cellSize = 25;

    public override void OnInspectorGUI()
    {
        FallingObjectsPatternSO pattern = (FallingObjectsPatternSO)target;

        EditorGUILayout.LabelField("Pattern Name");
        pattern.patternName = EditorGUILayout.TextField(pattern.patternName);

        EditorGUILayout.Space(10);

        pattern.RandomPosition = EditorGUILayout.Toggle("Random Position", pattern.RandomPosition);

        EditorGUILayout.Space(10);

        if (pattern.RandomPosition)
        {
            pattern.RandomPositionCount = EditorGUILayout.IntField("Random Count", pattern.RandomPositionCount);
        }
        else
        {
            EditorGUILayout.LabelField("Clicca sulla griglia per attivare/disattivare le celle");

            // Disegna la griglia 10x10
            for (int y = size - 1; y >= 0; y--)
            {
                EditorGUILayout.BeginHorizontal();

                for (int x = 0; x < size; x++)
                {
                    bool value = pattern.grid[x, y];

                    GUIStyle style = new(GUI.skin.button);
                    style.normal.textColor = value ? Color.green : Color.white;

                    if (GUILayout.Button(value ? "■" : "□", style, GUILayout.Width(cellSize), GUILayout.Height(cellSize)))
                    {
                        pattern.grid[x, y] = !value;
                        pattern.RebuildCells();
                        EditorUtility.SetDirty(pattern);
                    }
                }

                EditorGUILayout.EndHorizontal();
            }
        }

        EditorGUILayout.Space(10);

        if (GUILayout.Button("Rigenera celle"))
        {
            pattern.RebuildCells();
            EditorUtility.SetDirty(pattern);
        }

        EditorGUILayout.Space(10);
        EditorGUILayout.LabelField("Celle generate:", EditorStyles.boldLabel);

        if (pattern.cells != null)
        {
            foreach (var c in pattern.cells)
                EditorGUILayout.LabelField($"({c.x}, {c.y})");
        }
    }
}
