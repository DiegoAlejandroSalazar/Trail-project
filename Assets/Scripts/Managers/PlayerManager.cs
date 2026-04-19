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
}

public class PlayerData
{
    public GameObject GameObject;
    public PlayerGridMovement Movement;
    public PlayerTrail Trail;
    public PlayerInputQueue InputQueue;
    public IDamageable Damageable;
    public PlayerWallet PlayerWallet;

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
