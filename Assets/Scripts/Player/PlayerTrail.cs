using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Tilemaps;

public class PlayerTrail : MonoBehaviour
{
    [Header("Reference")]
    [SerializeField] private Tilemap _trailTilemap;
    [SerializeField] private TileBase _trailTile;

    private readonly List<(Vector3Int cell, int index)> trail = new();

    public void AddStep(Vector3Int cell, int index)
    {
        trail.Add((cell, index));
        _trailTilemap.SetTile(cell, _trailTile);
        Debug.Log($"{gameObject.name} traccia cella {cell} indice {index}");
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
