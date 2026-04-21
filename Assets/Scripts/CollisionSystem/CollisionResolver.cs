using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class CollisionResolver : MonoBehaviour
{
    public static CollisionResolver Instance;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    public void Resolve()
    {
        Dictionary<Vector3Int, List<(PlayerData player, int index)>> map = new();

        foreach (var p in PlayerManager.Instance.Players)
        {
            foreach (var (cell, index) in p.Trail.GetTrail())
            {
                if (!map.ContainsKey(cell))
                    map[cell] = new List<(PlayerData, int)>();

                map[cell].Add((p, index));
            }
        }

        foreach (var kvp in map)
        {
            var cell = kvp.Key;
            var list = kvp.Value;

            if (list.Count <= 1)
                continue;

            var ordered = list.OrderBy(x => x.index).ToList();
            var loser = ordered.Last();
            
            AudioManager.Instance.PlaySfx("ContattoTrail", false, 0.5f);

            loser.player.Damageable?.TakeDamage(1, cell);
        }
    }
}
