using System;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GridGenerator : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Grid _grid;
    [SerializeField] private Tilemap _tilemap;
    [SerializeField] private TileBase _groundTile;

    [Header("Grid Settings")]
    [SerializeField] private int _width = 10;
    [SerializeField] private int _height = 10;
    [SerializeField, Range(0f, 1f)] private float _spacing = 0.1f;

    void Start()
    {
        GenerateMap();
    }

    private void GenerateMap()
    {
        _tilemap.ClearAllTiles();
        UpdateGridSpacing();


        for (int x = 0; x < _width; x++)
        {
            for (int y = 0; y < _height; y++)
            {
                _tilemap.SetTile(new Vector3Int(x, y, 0), _groundTile);
            }
        }
    }
    private void UpdateGridSpacing()
    {
        _grid.cellGap = new(_spacing, _spacing, 0);
    }
}
