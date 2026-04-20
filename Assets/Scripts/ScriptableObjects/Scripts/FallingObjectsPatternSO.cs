using UnityEngine;

[CreateAssetMenu(fileName = "FallingObjectsPatternSO", menuName = "Pattern/FallingObjectsPatternSO")]
public class FallingObjectsPatternSO : ScriptableObject
{
    public string patternName;

    [HideInInspector] public bool[,] grid = new bool[10,10];

    [Header("Random Pattern")]
    public bool RandomPosition = false;
    public int RandomPositionCount = 0;

    [Tooltip("Celle generate automaticamente dall'editor o dal random")]
    public Vector3Int[] cells;

    public void RebuildCells()
    {
        var list = new System.Collections.Generic.List<Vector3Int>();

        if (RandomPosition)
        {
            list.Clear();

            for (int i = 0; i < RandomPositionCount; i++)
            {
                int x = Random.Range(0, 10);
                int y = Random.Range(0, 10);

                Vector3Int cell = new(x, y, 0);

                // evita duplicati
                if (!list.Contains(cell))
                    list.Add(cell);
                else
                    i--; 
            }

            cells = list.ToArray();
            return;
        }

        for (int x = 0; x < 10; x++)
        {
            for (int y = 0; y < 10; y++)
            {
                if (grid[x, y])
                    list.Add(new Vector3Int(x, y, 0));
            }
        }

        cells = list.ToArray();
    }
}
