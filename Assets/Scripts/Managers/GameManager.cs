using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static int PlayerCount = 1;
    [SerializeField] private Grid _grid;
    [SerializeField] private GameObject _playerPrefab;

    public static readonly Vector3Int[] SpawnPositions = new Vector3Int[]
    {
        new(0, 0, 0),   // Basso sinistra
        new(9, 0, 0),   // Basso destra
        new(0, 9, 0),   // Alto sinistra
        new(9, 9, 0)    // Alto destra
    };

    void Start()
    {
        SpawnPlayers(PlayerCount);
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
