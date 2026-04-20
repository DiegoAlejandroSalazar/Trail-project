using UnityEngine;
using DG.Tweening;
using UnityEngine.Tilemaps;

[RequireComponent(typeof(PlayerTrail))]
[RequireComponent(typeof(PlayerWallet))]
public class PlayerGridMovement : MonoBehaviour
{
    [Header("Grid Reference")]
    [SerializeField] private Grid _grid;
    [SerializeField] private Tilemap _walkLayer;

    [Header("Trail Reference")]
    private PlayerTrail _trail;
    private int _moveIndex = 0;

    [Header("Spawn Settings")]
    [SerializeField] private Vector3 spawnOffset = new(0, 0.25f, 0);


    [Header("Movement Settings")]

    [Tooltip("Durata singolo movimento")]
    [SerializeField] private float _moveDuration = 0.3f; // Durata del salto

    private Vector3Int currentCell;
    private bool isMoving = false;
    public bool IsFree => !isMoving;

    private Animator _animator;

    void Awake()
    {
        _animator = GetComponent<Animator>();
    }
    void Start()
    {
        Initialize();
    }
    private void Initialize()
    {
        PlayerManager.Instance.RegisterPlayer(gameObject);

        _trail = GetComponent<PlayerTrail>();

        Vector3 startingPos = transform.position;

        currentCell = _grid.WorldToCell(startingPos);

        if (!_walkLayer.HasTile(currentCell))
        {
            Debug.LogWarning("Attenzione: Il Player non è sopra una tile valida al via!");
        }

        transform.position = _grid.GetCellCenterWorld(currentCell) + spawnOffset;
    }

    public void ForceCell(Vector3Int cell)
    {
        currentCell = cell;
        transform.position = _grid.GetCellCenterWorld(cell) + spawnOffset;
    }



    public void TryMove(Vector3Int direction)
    {
        if (isMoving) return;

        Vector3Int targetCell = currentCell + direction;

        if (_walkLayer.HasTile(targetCell))
        {
            ExecuteJump(direction);
        }
        else
        {
            Debug.Log("Muro o vuoto! Non posso andare in " + targetCell);
        }
    }


    void ExecuteJump(Vector3Int direction)
    {
        isMoving = true;

        Vector3Int targetCell = currentCell + direction;
        currentCell = targetCell;

        Vector3 targetWorldPos = _grid.GetCellCenterWorld(targetCell);

        //aggiunge step per il trail
        _trail.AddStep(targetCell, _moveIndex);
        _moveIndex++;

        // animazione
        _animator.SetBool("IsMoving", true);
        _animator.SetTrigger("Moving");
        transform.DOMove(targetWorldPos, _moveDuration).SetEase(Ease.OutQuad).OnComplete(() =>
        {
            isMoving = false;
            _animator.SetBool("IsMoving", false);
        });

        //controllo moneta
        if (CoinSpawner.Instance.IsCoinAtCell(currentCell))
        {
            GetComponent<PlayerWallet>().AddCoin(1);
            CoinSpawner.Instance.SpawnCoin();
        }
    }
    public void ResetMoveIndex()
    {
        _moveIndex = 0;
    }


}