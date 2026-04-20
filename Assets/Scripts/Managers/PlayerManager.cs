using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Tilemaps;
using System;

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
    public PlayerInputHandler PlayerInputHandler;
    public PlayerGridMovement Movement;
    public PlayerTrail Trail;
    public PlayerInputQueue InputQueue;
    public PlayerHealth Health;
    public IDamageable Damageable;
    public PlayerWallet PlayerWallet;
    public bool IsAlive = true;

    public PlayerData(GameObject go)
    {
        GameObject = go;
        PlayerInputHandler = go.GetComponent<PlayerInputHandler>();
        Movement = go.GetComponent<PlayerGridMovement>();
        Trail = go.GetComponent<PlayerTrail>();
        InputQueue = go.GetComponent<PlayerInputQueue>();
        Damageable = go.GetComponent<IDamageable>();
        Health = go.GetComponent<PlayerHealth>();
        PlayerWallet = go.GetComponent<PlayerWallet>();
    }
    public void InitializeComponents(Vector3Int startCell, Tilemap walkMap, Tilemap trailMap, Color assignedColor)
    {
        if (Movement != null)
            Movement.InitializeFromManager(startCell, walkMap, assignedColor);

        if (Trail != null)
            Trail.Init(trailMap, assignedColor);
        
        if(InputQueue != null)
            InputQueue.Init(Movement, PlayerInputHandler);


    }

}
