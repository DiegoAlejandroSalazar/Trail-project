using UnityEngine;
using TMPro;

public class PlayerWallet : MonoBehaviour
{
    public int Coins { get; private set; } = 0;

    [HideInInspector] public TMP_Text CoinText;

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
        if (CoinText != null)
	{
        	CoinText.text = Coins.ToString();
	}
    }
}
