using TMPro;
using UnityEngine;

public class PlayerHealth : MonoBehaviour, IDamageable
{
    [Header("Ui references")]
    public TMP_Text HealthText;
    [Header("Player Health Settings")]
    [SerializeField] private int _maxLives = 3;
    private int _lives;

    void Start()
    {
        _lives = _maxLives;
        UpdateUI();
    }
    public void TakeDamage(int amount, Vector3Int collisionCenter)
    {
        _lives -= amount;

        Debug.Log($"{gameObject.name} ha preso danno! alla posizione {collisionCenter} Vite rimaste: {_lives}");
        UpdateUI();

        if (_lives <= 0)
        {
            Debug.Log($"{gameObject.name} GAME OVER");
            PlayerManager.Instance.PlayerDied(gameObject);

        }
    }
    private void UpdateUI()
    {
        if (HealthText != null)
            HealthText.text = _lives.ToString();
    }

}
