using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement; // Necessario per cambiare scena

public class GameUIManager : MonoBehaviour
{
    public static GameUIManager Instance;

    [Header("UI References")]
    [SerializeField] private GameObject _endScreen;
    [SerializeField] private TMP_Text _winnerText;
    [SerializeField] private TMP_Text _turnText;
    [SerializeField] private TMP_Text _totalTurnText;
    [SerializeField] private TMP_Text _totalCoinText;
    [SerializeField] private TMP_Text _difficultyText;

    void Awake()
    {
        if (Instance == null) Instance = this;

        if (_endScreen.activeSelf)
        {
            _endScreen.SetActive(false);
        }
    }

    public void ShowEndScreen(PlayerData winner)
    {
        _endScreen.SetActive(true);
        GameManager.Instance.GameFinish = true;
        CoinSpawner.Instance.StopCoinMovement();

        if (winner != null)
        {
            _winnerText.text = $"{winner.GameObject.name} WINS!";
            _winnerText.color = winner.GameObject.GetComponentInChildren<SpriteRenderer>().color;
            _totalTurnText.text = $"Turns survived:{GameManager.Instance.CurrentTurn}";
            _totalCoinText.text = $"Gold gained: {winner.PlayerWallet.Coins}";
        }
        else
        {
            _winnerText.text = "DRAW!";
            _totalCoinText.text = "Gold gained: 0";
        }

        _turnText.text = $"Turns survived: {GameManager.Instance.CurrentTurn}";
    }

    #region buttons

    /// <summary>
    /// Ricarica la scena attuale (il gioco)
    /// </summary>
    public void OnClickRestart()
    {

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    /// <summary>
    /// Torna al Menu Principale (Scena 0)
    /// </summary>
    public void OnClickMainMenu()
    {
        if (GameManager.Instance != null)
        {
            Destroy(GameManager.Instance.gameObject);
        }

        SceneManager.LoadScene(0);
    }
    #endregion
    public void UpdateUi()
    {
        _turnText.text = $"Turn: \n{GameManager.Instance.CurrentTurn}";
        _difficultyText.text = GameManager.Instance.GetDifficultyName();
    }
}