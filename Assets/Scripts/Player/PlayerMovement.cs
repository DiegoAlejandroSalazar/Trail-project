using UnityEngine;
using DG.Tweening;
using UnityEngine.Tilemaps;
using Unity.Mathematics;
using UnityEngine.InputSystem;

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

    void Start()
    {
        Vector3 startingPos = transform.position;

        currentCell = _grid.WorldToCell(startingPos);

        if (!_walkLayer.HasTile(currentCell))
        {
            Debug.LogWarning("Attenzione: Il Player non è sopra una tile valida al via!");
        }

        //Centra il player sulla cella
        transform.position = _grid.GetCellCenterWorld(currentCell);
    }

    void Update()
    {
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        Vector3Int mouseCell = _grid.WorldToCell(mouseWorldPos);
        Debug.Log("Il mouse punta alla cella: " + mouseCell);
        if (isMoving) return;

        Vector2 input = PlayerInputHandler.Instance.MovementInput;

        if (input != Vector2.zero)
        {
            Vector3Int direction = Vector3Int.zero;

            if (Mathf.Abs(input.x) > Mathf.Abs(input.y))
                direction.x = input.x > 0 ? 1 : -1;
            else
                direction.y = input.y > 0 ? 1 : -1;

            if (direction != Vector3Int.zero)
            {
                Vector3Int targetCell = currentCell + direction;

                // CONTROLLO
                if (_walkLayer.HasTile(targetCell))
                {
                    ExecuteJump(direction);
                }
                else
                {
                    Debug.Log("Muro o vuoto! Non posso andare in " + targetCell);
                }
            }
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