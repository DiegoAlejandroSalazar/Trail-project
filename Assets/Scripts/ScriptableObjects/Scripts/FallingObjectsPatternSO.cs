using UnityEngine;

[CreateAssetMenu(fileName = "FallingObjectsPatternSO", menuName = "Pattern/FallingObjectsPatternSO")]
public class FallingObjectsPatternSO : ScriptableObject
{
    public string Name;
    [Tooltip("celle relative alla griglia")]
    public Vector3Int[] cells; // celle relative alla griglia
}
