using UnityEngine;

[CreateAssetMenu(fileName = "GameDifficultySettings", menuName = "Game/GameDifficultySettings")]
public class GameDifficultySettings : ScriptableObject
{
    [Header("Difficulty Threshold")]
    public int EasyTurnThreshold = 5;
    public int MediumTurnThreshold = 7;
    public int HardTurnThreshold = 10;


    [Header("Input Window (seconds)")]
    public float easyInputWindow = 4f;
    public float mediumInputWindow = 3f;
    public float hardInputWindow = 2.5f;
    public float insaneInputWindow = 2f;

    [Header("Input Buffer")]
    public int easyBuffer = 4;
    public int mediumBuffer = 5;
    public int hardBuffer = 6;
    public int insaneBuffer = 7;

    [Header("Random Pattern Cells")]
    public int easyRandomCells = 2;
    public int mediumRandomCells = 4;
    public int hardRandomCells = 6;
    public int insaneRandomCells = 8;

    [Header("Pattern Pools")]
    public FallingObjectsPatternSO[] easyPatterns;
    public FallingObjectsPatternSO[] mediumPatterns;
    public FallingObjectsPatternSO[] hardPatterns;
    public FallingObjectsPatternSO[] insanePatterns;
}
