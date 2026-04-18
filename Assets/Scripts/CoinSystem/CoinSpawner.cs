using UnityEngine;
using UnityEngine.Tilemaps;

public class CoinSpawner : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Grid _grid;
    [SerializeField] private Tilemap _walkableLayer;
    [SerializeField] private Transform _coin;
    [SerializeField] private Transform _player;

    [Header("Spawn Settings")]
    [SerializeField] private int _maxAttempts = 50; // da vede se mette cosi 

    private Vector3Int _lastSpawnCell;

    void Start()
    {
        SpawnCoin();
    }

    public void SpawnCoin()
    {
        for (int i = 0; i < _maxAttempts; i++)
        {
            Vector3Int randomCell = GetRandomWalkableCell();

            // evita di spawnare sulla cella del player
            Vector3Int playerCell = _grid.WorldToCell(_player.position);
            if (randomCell == playerCell)
                continue;

            _lastSpawnCell = randomCell;

            Vector3 worldPos = _grid.GetCellCenterWorld(randomCell);
            _coin.position = worldPos;
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
}
