using UnityEngine;
using UnityEngine.Tilemaps;

public class CoinSpawner : MonoBehaviour
{
    public static CoinSpawner Instance;
    [Header("References")]
    [SerializeField] private Grid _grid;
    [SerializeField] private Tilemap _walkableLayer;
    [SerializeField] private Transform _coin;

    [Header("Spawn Settings")]
    [SerializeField] private int maxAttempts = 50;

    private Vector3Int _lastSpawnCell;

    void Awake()
    {
        Instance = this;
    }
    void Start()
    {
        SpawnCoin();
    }

    public void SpawnCoin()
    {
        StopCoinMovement();

        for (int i = 0; i < maxAttempts; i++)
        {
            Vector3Int randomCell = GetRandomWalkableCell();

            bool cellOccupied = false;

            foreach (var p in PlayerManager.Instance.Players)
            {
                Vector3Int playerCell = _grid.WorldToCell(p.Movement.transform.position);
                if (playerCell == randomCell)
                {
                    cellOccupied = true;
                    break;
                }
            }

            if (cellOccupied)
                continue;

            _lastSpawnCell = randomCell;

            Vector3 worldPos = _grid.GetCellCenterWorld(randomCell);
            _coin.position = worldPos;
            StartCoinMovement(worldPos);
            return;
        }

        Debug.LogWarning("Impossibile trovare una cella valida per la moneta!");
    }

    private Vector3Int GetRandomWalkableCell()
    {
        BoundsInt bounds = _walkableLayer.cellBounds;

        int x = Random.Range(bounds.xMin, bounds.xMax);
        int y = Random.Range(bounds.yMin, bounds.yMax);

        Vector3Int cell = new(x, y, 0);

        if (_walkableLayer.HasTile(cell))
            return cell;

        return GetRandomWalkableCell();
    }
    public bool IsCoinAtCell(Vector3Int cell)
    {
        return cell == _lastSpawnCell;
    }
    public void StopCoinMovement()
    {
        CoinMovement movement = _coin.GetComponent<CoinMovement>();
        movement.StopFloating();

    }
    private void StartCoinMovement(Vector3 WordPosition)
    {
        CoinMovement movement = _coin.GetComponent<CoinMovement>();
        movement.StartFloating(WordPosition);
    }

}
