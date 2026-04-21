using UnityEngine;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine.Tilemaps;

public class FallingObjectManager : MonoBehaviour
{
    public static FallingObjectManager Instance;

    [Header("References")]
    [SerializeField] private Grid _grid;
    [SerializeField] private Tilemap _warningTileMap;
    [SerializeField] private TileBase _waringTile;
    [SerializeField] private GameObject _fallingObjectPrefab;
    private FallingObjectsPatternSO _currentPattern;
    void Awake()
    {
        Instance = this;
    }
    private void ChoosePatternFromDifficulty()
    {
        _currentPattern = GameManager.Instance.GetPattern();

        if (_currentPattern.RandomPosition)
        {
            _currentPattern.RandomPositionCount = GameManager.Instance.GetRandomCellCount();
            _currentPattern.RebuildCells();
        }

        //Debug.Log($" Pattern scelto: {_currentPattern.name}");
    }
    private void ShowWarningTiles()
    {
        if (GameManager.Instance.GameFinish) return;
        foreach (Vector3Int cell in _currentPattern.cells)
        {
            _warningTileMap.SetTile(cell, _waringTile);
        }
    }
    private void ClearTiles()
    {
        foreach (Vector3Int cell in _currentPattern.cells)
        {
            _warningTileMap.SetTile(cell, null);
        }
    }
    public void InitializePattern()
    {
        if (AudioManager.Instance != null)
            AudioManager.Instance.PlaySfx("StartFallingObject", false, 0.8f);
        ChoosePatternFromDifficulty();
        ShowWarningTiles();
    }


    public void ExecutePattern()
    {
        if (_currentPattern == null)
            return;
            
        if (AudioManager.Instance != null)
            AudioManager.Instance.PlaySfx("AttaccoMeteore", false, 0.5f);

        foreach (var cell in _currentPattern.cells)
        {
            Vector3 worldPos = _grid.GetCellCenterWorld(cell);

            // Spawn oggetto che cade
            GameObject obj = Instantiate(_fallingObjectPrefab, worldPos + Vector3.up * 5f, _fallingObjectPrefab.transform.rotation);

            // Animazione caduta
            obj.transform
                .DOMove(worldPos, 0.5f)
                .SetEase(Ease.InQuad)
                .OnComplete(() =>
                {
                    CheckDamage(cell);
                    Destroy(obj);
                });
        }
        ClearTiles();
    }

    private void CheckDamage(Vector3Int cell)
    {
        foreach (var p in PlayerManager.Instance.Players)
        {
            Vector3Int playerCell = _grid.WorldToCell(p.Movement.transform.position);

            if (playerCell == cell)
            {
                Debug.Log($"{p.GameObject.name} colpito da oggetto caduto! posizione {cell}");
                p.Damageable?.TakeDamage(1, cell);
            }
        }
    }
}
