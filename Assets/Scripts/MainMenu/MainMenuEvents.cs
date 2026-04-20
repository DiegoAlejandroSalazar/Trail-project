using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

[RequireComponent(typeof(UIDocument))]
[RequireComponent(typeof(HeaderEffects))]
public class MainMenuEvents : MonoBehaviour
{
    private UIDocument _document;

    [Header("Visual Trees")]
    [SerializeField] private VisualTreeAsset _mainMenuTree;
    [SerializeField] private VisualTreeAsset _choosePlayerVisualTree;

    private AudioSource _audioSource;

    private void Awake()
    {
        _document = GetComponent<UIDocument>();
        _audioSource = GetComponent<AudioSource>();

        LoadMainMenu();
    }

#region MainMenu
    private void LoadMainMenu()
    {
        _document.visualTreeAsset = _mainMenuTree;

        // Aggiorna header effects
        GetComponent<HeaderEffects>().RefreshHeaders();

        // Registra bottoni
        var root = _document.rootVisualElement;

        var playButton = root.Q<Button>("PlayButton");
        playButton.RegisterCallback<ClickEvent>(OnPlayClick);

        var exitButton = root.Q<Button>("ExitButton");
        exitButton.RegisterCallback<ClickEvent>(OnExitClick);
    }

    private void OnPlayClick(ClickEvent click)
    {
        _audioSource.Play();
        LoadChoosePlayerMenu();
    }

    private void OnExitClick(ClickEvent click)
    {
        Application.Quit();
    }
    #endregion

    #region number player choise
    private void LoadChoosePlayerMenu()
    {
        _document.visualTreeAsset = _choosePlayerVisualTree;

        GetComponent<HeaderEffects>().RefreshHeaders();

        var root = _document.rootVisualElement;

        root.Q<Button>("OnePlayerButton")?.RegisterCallback<ClickEvent>(evt => StartGame(1));
        root.Q<Button>("TwoPlayerButton")?.RegisterCallback<ClickEvent>(evt => StartGame(2));
        root.Q<Button>("ThreePlayerButton")?.RegisterCallback<ClickEvent>(evt => StartGame(3));
        root.Q<Button>("FourPlayerButton")?.RegisterCallback<ClickEvent>(evt => StartGame(4));

        root.Q<Button>("BackButton")?.RegisterCallback<ClickEvent>(evt => LoadMainMenu());
    }

    private void StartGame(int players)
    {
        Debug.Log(players);
        GameManager.PlayerCount = players;
        //SceneManager.LoadScene("GameScene");
    }
    #endregion
}
