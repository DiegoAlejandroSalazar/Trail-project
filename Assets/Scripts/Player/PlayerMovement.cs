using UnityEngine;
using DG.Tweening;
using UnityEngine.Tilemaps;

public class PlayerGridMovement : MonoBehaviour
{
    [Header("Grid Reference")]
    [SerializeField] private Grid _grid;
    private Tilemap _walkLayer;
    private PlayerTrail _trail;
    private int _moveIndex = 0;

    [Header("Movement Settings")]
    [SerializeField] private float _moveDuration = 0.3f;

    [SerializeField] private AudioClip moveSound;

    private Vector3Int currentCell;
    private bool isMoving = false;
    public bool IsFree => !isMoving;

    private Animator _animator;

    void Awake()
    {
        _animator = GetComponentInChildren<Animator>();
    }

    public void InitializeFromManager(Vector3Int startCell, Tilemap walkMap, Color color)
    {
        _walkLayer = walkMap;
        _grid = _walkLayer.layoutGrid;

        currentCell = startCell;

        transform.position = _walkLayer.GetCellCenterWorld(startCell);

        // Applica il colore allo SpriteRenderer nel figlio
        SpriteRenderer sr = GetComponentInChildren<SpriteRenderer>();
        if (sr != null)
        {
            sr.color = color;
        }

        Initialize();
    }
    private void Initialize()
    {
        _trail = GetComponent<PlayerTrail>();


        Vector3 pos = transform.position;
        currentCell = _walkLayer.WorldToCell(new Vector3(pos.x, pos.y, 0));

        if (!_walkLayer.HasTile(currentCell))
        {
            currentCell = _walkLayer.LocalToCell(transform.localPosition);
        }

        if (!_walkLayer.HasTile(currentCell))
        {
            Debug.LogError($"ERRORE: Player a {transform.position} non trova tile in {currentCell}.");
        }

        transform.position = _walkLayer.GetCellCenterWorld(currentCell);
    }
    public void ForceCell(Vector3Int cell)
    {
        currentCell = cell;
        transform.position = _walkLayer.GetCellCenterWorld(cell);
    }

    public void TryMove(Vector3Int direction)
    {
        if (isMoving) return;
        Vector3Int targetCell = currentCell + direction;

        if (_walkLayer.HasTile(targetCell))
        {
            ExecuteMove(direction);
        }
    }

    void ExecuteMove(Vector3Int direction)
    {
        isMoving = true;
        currentCell += direction;

        Vector3 targetWorldPos = _walkLayer.GetCellCenterWorld(currentCell);
        //Debug.Log($"{gameObject.name} traccia cella {currentCell}");


        _trail.AddStep(currentCell, _moveIndex);
        _moveIndex++;

        if (_animator != null)
        {
            _animator.SetBool("IsMoving", true);
            _animator.SetTrigger("Moving");
        }

        AudioManager.Instance.PlaySfx("PlayerMove", true, 0.4f);
        
        transform.DOMove(targetWorldPos, _moveDuration).SetEase(Ease.OutQuad).OnComplete(() =>
        {
            isMoving = false;
            if (_animator != null) _animator.SetBool("IsMoving", false);
        });

        // Logica monete
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
