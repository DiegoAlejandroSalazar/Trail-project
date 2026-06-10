using TMPro;
using UnityEngine;

public class PlayerUIDisplay : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private TMP_Text _playerName;
    [SerializeField] private TMP_Text _playerHealth;
    [SerializeField] private TMP_Text _playerGold;
    [SerializeField] private TextMeshProUGUI _playerDebug;

    public void SetUpUi(PlayerData player, Color color)
    {
        _playerName.text = player.GameObject.name;
        _playerName.color = color;
        player.InputQueue.DebugQueueText = _playerDebug;
        _playerDebug.color = color;
        player.PlayerWallet.CoinText = _playerGold;
        _playerGold.color = color;
        player.Health.HealthText = _playerHealth;
        _playerHealth.color = color;
    }
}
