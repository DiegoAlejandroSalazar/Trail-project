using TMPro;
using UnityEngine;

public class GameUIManager : MonoBehaviour
{
    public static GameUIManager Instance;
    [Header("UiReferences")]
    [SerializeField] private GameObject _endScreen;
    [SerializeField] private TMP_Text _winnerText;
    [SerializeField] private TMP_Text _turnText;
    [SerializeField] private TMP_Text _difficultyText;

    void Awake()
    {
        Instance = this;
    }

    public void ShowEndScreen(PlayerData winner)
    {
        _endScreen.SetActive(true);

        if (winner != null)
            _winnerText.text = $"{winner.GameObject.name} wins!";
        else
            _winnerText.text = "Nessun vincitore!";
    }

    public void UpdateUi()
    {
        _turnText.text = GameManager.Instance.CurrentTurn.ToString();
        _difficultyText.text = GameManager.Instance.GetDifficultyName();
    }
}