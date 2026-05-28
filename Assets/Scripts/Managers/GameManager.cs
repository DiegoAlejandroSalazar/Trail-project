using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

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

    [Header("walkable References")]
    [SerializeField] private Grid _grid;
    [SerializeField] private Tilemap _walkLayer;

    [Header("trail References")]
    [SerializeField] private Tilemap _trailLayer;
    [SerializeField] private TileBase _trailTile;
    [Header("Ui References")]
    [SerializeField] private Transform _uiContainer;
    [SerializeField] private GameObject _playerUIPrefab;
    [Header("Player References")]
    [SerializeField] private GameObject _playerPrefab;

    [Header("Difficulty settings")]
    [SerializeField] private GameDifficultySettings _settings;

    [SerializeField]
    private List<Color> _playerColors = new()
    {
        Color.red,
        Color.blue,
        Color.green,
        Color.yellow
    };

    public static int PlayerCount { get; set; } = 2;
    public int CurrentTurn { get; private set; } = 1;
    [HideInInspector] public bool GameFinish = false;

    public static readonly Vector3Int[] SpawnPositions = new Vector3Int[]
    {
        new(0, 0, 0),   // Basso sinistra
        new(9, 0, 0),   // Basso destra
        new(0, 9, 0),   // Alto sinistra
        new(9, 9, 0)    // Alto destra
    };
    private static readonly Vector2[] UIAnchors = new Vector2[]
    {
        new(0, 1), // Player 0: Alto Sinistra
        new(1, 1), // Player 1: Alto Destra
        new(0, 0), // Player 2: Basso Sinistra
        new(1, 0)  // Player 3: Basso Destra
    };
    private static readonly Vector2[] UIPivots = new Vector2[]
    {
        new(0, 1), // Alto Sinistra
        new(1, 1), // Alto Destra
        new(0, 0), // Basso Sinistra
        new(1, 0)  // Basso Destra
    };

    void Awake()
    {
        Instance = this;
        SpawnPlayers(PlayerCount);
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
        //List<FallingObjectsPatternSO> pool = new();

        // Easy sempre incluso
        //pool = _settings.easyPatterns;

        if (CurrentTurn > _settings.EasyTurnThreshold)
            //pool.AddRange(_settings.mediumPatterns);
        return _settings.easyPatterns[Random.Range(0, _settings.mediumPatterns.Length)];

        if (CurrentTurn > _settings.MediumTurnThreshold)
            //pool.AddRange(_settings.hardPatterns);
        return _settings.hardPatterns[Random.Range(0, _settings.hardPatterns.Length)];

        if (CurrentTurn > _settings.HardTurnThreshold)
            //pool.AddRange(_settings.insanePatterns);
        return _settings.insanePatterns[Random.Range(0, _settings.insanePatterns.Length)];

        return _settings.easyPatterns[Random.Range(0, _settings.easyPatterns.Length)];
    }

    public void SpawnPlayers(int playerCount)
    {
        for (int i = 0; i < playerCount; i++)
        {
            Color assignedColor = _playerColors[i % _playerColors.Count];

            GameObject playerObj = Instantiate(_playerPrefab);

            playerObj.name = $"Player_{i + 1}";

            PlayerManager.Instance.RegisterPlayer(playerObj);

            PlayerManager.Instance.Players[i].PlayerInputHandler.Initialize(i);

            PlayerManager.Instance.Players[i].InitializeComponents(SpawnPositions[i], _walkLayer, _trailLayer, assignedColor);

            GameObject uiObj = Instantiate(_playerUIPrefab, _uiContainer);

            RectTransform uiRect = uiObj.GetComponent<RectTransform>();
            SetUIAnchor(uiRect, i);

            if (uiObj.TryGetComponent<PlayerUIDisplay>(out var uiDisplay))
            {
                uiDisplay.SetUpUi(PlayerManager.Instance.Players[i], assignedColor);
            }
        }
    }
    private void SetUIAnchor(RectTransform rect, int index)
    {
        Vector2 anchor = UIAnchors[index];
        rect.anchorMin = anchor;
        rect.anchorMax = anchor;
        rect.pivot = UIPivots[index];

        rect.anchoredPosition = Vector2.zero;

        float margin = 20f;
        float offsetX = (anchor.x == 0) ? margin : -margin;
        float offsetY = (anchor.y == 0) ? margin : -margin;
        rect.anchoredPosition = new Vector2(offsetX, offsetY);
    }
}
