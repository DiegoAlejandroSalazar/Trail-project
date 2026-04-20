using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public enum DifficultyTier
    {
        Easy,
        Medium,
        Hard,
        Insane
    }
    public static GameManager Instance;

    [Header("References")]
    [SerializeField] private Grid _grid;
    [SerializeField] private GameObject _playerPrefab;

    [Header("Difficulty settings")]
    [SerializeField] private GameDifficultySettings _settings;



    public int PlayerCount { get; set; }
    public int CurrentTurn { get; private set; } = 1;

    public static readonly Vector3Int[] SpawnPositions = new Vector3Int[]
    {
        new(0, 0, 0),   // Basso sinistra
        new(9, 0, 0),   // Basso destra
        new(0, 9, 0),   // Alto sinistra
        new(9, 9, 0)    // Alto destra
    };

    void Awake()
    {
        Instance = this;
    }
    void Start()
    {
        //SpawnPlayers(2);
    }
    public void NextTurn()
    {
        CurrentTurn++;
    }

    private DifficultyTier GetTier()
    {
        if (CurrentTurn <= _settings.EasyTurnThreshold) return DifficultyTier.Easy;
        if (CurrentTurn <= _settings.MediumTurnThreshold) return DifficultyTier.Medium;
        if (CurrentTurn <= _settings.HardTurnThreshold) return DifficultyTier.Hard;
        return DifficultyTier.Insane;
    }
    public string GetDifficultyName()
    {
        return GetTier() switch
        {
            DifficultyTier.Easy => "Easy",
            DifficultyTier.Medium => "Medium",
            DifficultyTier.Hard => "Hard",
            _ => "Insane",
        };
    }


    public float GetInputWindow()
    {
        return GetTier() switch
        {
            DifficultyTier.Easy => _settings.easyInputWindow,
            DifficultyTier.Medium => _settings.mediumInputWindow,
            DifficultyTier.Hard => _settings.hardInputWindow,
            _ => _settings.insaneInputWindow,
        };
    }

    public int GetBufferSize()
    {
        return GetTier() switch
        {
            DifficultyTier.Easy => _settings.easyBuffer,
            DifficultyTier.Medium => _settings.mediumBuffer,
            DifficultyTier.Hard => _settings.hardBuffer,
            _ => _settings.insaneBuffer,
        };
    }

    public int GetRandomCellCount()
    {
        return GetTier() switch
        {
            DifficultyTier.Easy => _settings.easyRandomCells,
            DifficultyTier.Medium => _settings.mediumRandomCells,
            DifficultyTier.Hard => _settings.hardRandomCells,
            _ => _settings.insaneRandomCells,
        };
    }

    public FallingObjectsPatternSO GetPattern()
    {
        List<FallingObjectsPatternSO> pool = new();

        // Easy sempre incluso
        pool.AddRange(_settings.easyPatterns);

        if (CurrentTurn > _settings.EasyTurnThreshold)
            pool.AddRange(_settings.mediumPatterns);

        if (CurrentTurn > _settings.MediumTurnThreshold)
            pool.AddRange(_settings.hardPatterns);

        if (CurrentTurn > _settings.HardTurnThreshold)
            pool.AddRange(_settings.insanePatterns);

        return pool[Random.Range(0, pool.Count)];
    }

    public void SpawnPlayers(int playerCount)
    {
        for (int i = 0; i < playerCount; i++)
        {
            GameObject player = Instantiate(_playerPrefab);

            Vector3 worldPos = _grid.GetCellCenterWorld(SpawnPositions[i]);
            player.transform.position = worldPos;

            player.GetComponent<PlayerGridMovement>().ForceCell(SpawnPositions[i]);
        }
    }
}
