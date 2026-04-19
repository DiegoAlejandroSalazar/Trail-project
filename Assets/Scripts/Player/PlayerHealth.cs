using UnityEngine;

public class PlayerHealth : MonoBehaviour, IDamageable
{
    [Header("Player Health Settings")]
    [SerializeField] private int _lives = 3;
    public void TakeDamage(int amount, Vector3Int collisionCenter)
    {
        _lives -= amount;

        Debug.Log($"{gameObject.name} ha preso danno! Vite rimaste: {_lives}");

        if (_lives <= 0)
        {
            Debug.Log($"{gameObject.name} GAME OVER");
            
        }
    }

}
