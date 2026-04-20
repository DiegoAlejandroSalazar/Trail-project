using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class PlayerSelectMenu : MonoBehaviour
{
    private UIDocument _doc;

    private void Awake()
    {
        _doc = GetComponent<UIDocument>();
        var root = _doc.rootVisualElement;

        root.Q<Button>("OnePlayer").RegisterCallback<ClickEvent>(evt => StartGame(1));
        root.Q<Button>("TwoPlayers").RegisterCallback<ClickEvent>(evt => StartGame(2));
        root.Q<Button>("ThreePlayers").RegisterCallback<ClickEvent>(evt => StartGame(3));
        root.Q<Button>("FourPlayers").RegisterCallback<ClickEvent>(evt => StartGame(4));

        root.Q<Button>("BackButton").RegisterCallback<ClickEvent>(OnBack);
    }

    private void StartGame(int players)
    {
        // Salva il numero di giocatori in un GameManager statico
        //GameManager.PlayerCount = players;

        // Carica la scena del gioco
        SceneManager.LoadScene("1");
    }

    private void OnBack(ClickEvent evt)
    {
        // Torna al menu precedente
        //FindObjectOfType<MainMenuEvents>().LoadMainMenu();
    }
}
