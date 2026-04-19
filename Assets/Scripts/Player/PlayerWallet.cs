using UnityEngine;
using TMPro;

public class PlayerWallet : MonoBehaviour
{
    public int Coins { get; private set; } = 0;

    [SerializeField] private TMP_Text _coinText;

    void Start()
    {
        UpdateUI();
    }

    public void AddCoin(int amount = 1)
    {
        Coins += amount;
        UpdateUI();
    }

    private void UpdateUI()
    {
        if (_coinText != null)
            _coinText.text = Coins.ToString();
    }
}
