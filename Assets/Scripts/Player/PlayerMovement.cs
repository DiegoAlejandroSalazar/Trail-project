using UnityEngine;
using DG.Tweening;
using UnityEngine.Tilemaps;

public class PlayerGridMovement : MonoBehaviour
{
    [Header("Grid Reference")]
    [SerializeField] private Grid _grid;
    [SerializeField] private Tilemap _walkLayer;
    [Header("Movement Settings")]
    [Tooltip("Durata singolo movimento")]
    [SerializeField] private float _moveDuration = 0.3f; // Durata del salto
    [Tooltip("Altezza singolo movimento")]
    [SerializeField] private float _jumpPower = 0.5f;    // Altezza del salto visivo

    private Vector3Int currentCell;
    private bool isMoving = false;
    public bool IsFree => !isMoving;


    void Start()
    {
        Vector3 startingPos = transform.position;

        currentCell = _grid.WorldToCell(startingPos);

        if (!_walkLayer.HasTile(currentCell))
        {
            Debug.LogWarning("Attenzione: Il Player non è sopra una tile valida al via!");
        }


        transform.position = _grid.GetCellCenterWorld(currentCell);
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

        transform.DOJump(targetWorldPos, _jumpPower, 1, _moveDuration)
            .SetEase(Ease.OutQuad)
            .OnComplete(() => isMoving = false);
    }

}