using TMPro;
using UnityEngine;

public class PlayerHealth : MonoBehaviour, IDamageable
{
    [Header("Ui references")]
    [SerializeField] private TMP_Text _healthText;
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

        Debug.Log($"{gameObject.name} ha preso danno! Vite rimaste: {_lives}");
        UpdateUI();

        if (_lives <= 0)
        {
            //Debug.Log($"{gameObject.name} GAME OVER");
            PlayerManager.Instance.PlayerDied(gameObject);

        }
    }
    private void UpdateUI()
    {
        if (_healthText != null)
            _healthText.text = _lives.ToString();
    }

}
