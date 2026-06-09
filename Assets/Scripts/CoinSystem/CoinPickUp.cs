using UnityEngine;

public class CoinPickup : MonoBehaviour
{
    private CoinSpawner spawner;

    void Start()
    {
        spawner = FindFirstObjectByType<CoinSpawner>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Moneta raccolta!");
            spawner.SpawnCoin();
        }
    }
}
