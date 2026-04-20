using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Tilemaps;

public class PlayerTrail : MonoBehaviour
{
    [Header("Reference")]
    private Tilemap _trailTilemap;
    [SerializeField] private TileBase _trailTile;
    private Color _trailColor;

    private readonly List<(Vector3Int cell, int index)> trail = new();

    public void Init(Tilemap map, Color color)
    {
        _trailTilemap = map;
        _trailColor = color;
    }
    public void AddStep(Vector3Int cell, int index)
    {
        trail.Add((cell, index));
        _trailTilemap.SetTile(cell, _trailTile);
        _trailTilemap.SetColor(cell, _trailColor);
        Debug.Log($"{gameObject.name} traccia cella {cell}");
    }

    public List<(Vector3Int cell, int index)> GetTrail()
    {
        return trail;
    }

    public void ClearTrail()
    {
        foreach (var (cell, index) in trail)
            _trailTilemap.SetTile(cell, null);

        trail.Clear();
    }

}
