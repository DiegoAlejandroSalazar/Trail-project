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

    [Header("Patterns")]
    [SerializeField] private List<FallingObjectsPatternSO> _patterns;

    private FallingObjectsPatternSO _currentPattern;

    void Awake()
    {
        Instance = this;
    }

    private void ChooseRandomPattern()
    {
        _currentPattern = _patterns[Random.Range(0, _patterns.Count)];
        Debug.Log("Pattern scelto: " + _currentPattern.name);
    }
    private void ShowWarningTiles()
    {
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
        ChooseRandomPattern();
        ShowWarningTiles();
    }

    public void ExecutePattern()
    {
        if (_currentPattern == null)
            return;

        foreach (var cell in _currentPattern.cells)
        {
            Vector3 worldPos = _grid.GetCellCenterWorld(cell);

            // Spawn oggetto che cade
            GameObject obj = Instantiate(_fallingObjectPrefab, worldPos + Vector3.up * 5f, Quaternion.identity);

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
                Debug.Log($"{p.GameObject.name} colpito da oggetto caduto!");
                p.Damageable?.TakeDamage(1, cell);
            }
        }
    }
}
