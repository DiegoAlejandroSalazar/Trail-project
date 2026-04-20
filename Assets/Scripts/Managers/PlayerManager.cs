using UnityEngine;
using System.Collections.Generic;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance;

    public List<PlayerData> Players { get; private set; } = new();

    void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    public void RegisterPlayer(GameObject player)
    {
        PlayerData data = new(player);
        Players.Add(data);
    }

    public void UnregisterPlayer(GameObject player)
    {
        Players.RemoveAll(p => p.GameObject == player);
    }
    public void PlayerDied(GameObject player)
    {
        var data = Players.Find(p => p.GameObject == player);
        if (data == null) return;

        data.IsAlive = false;

        data.GameObject.SetActive(false);

        Debug.Log($"{player.name} è morto!");

        CheckForWinner();
    }
    private void CheckForWinner()
    {
        int aliveCount = 0;
        PlayerData lastAlive = null;

        foreach (var p in Players)
        {
            if (p.IsAlive)
            {
                aliveCount++;
                lastAlive = p;
            }
        }

        if (aliveCount <= 1)
        {
            Debug.Log("GAME OVER — abbiamo un vincitore!");

            GameUIManager.Instance.ShowEndScreen(lastAlive);
        }
    }


}

public class PlayerData
{
    public GameObject GameObject;
    public PlayerGridMovement Movement;
    public PlayerTrail Trail;
    public PlayerInputQueue InputQueue;
    public IDamageable Damageable;
    public PlayerWallet PlayerWallet;
    public bool IsAlive = true;

    public PlayerData(GameObject go)
    {
        GameObject = go;
        Movement = go.GetComponent<PlayerGridMovement>();
        Trail = go.GetComponent<PlayerTrail>();
        InputQueue = go.GetComponent<PlayerInputQueue>();
        Damageable = go.GetComponent<IDamageable>();
        PlayerWallet = go.GetComponent<PlayerWallet>();
    }
}
