using System.Collections;
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

    private void Awake()
    {
        _document = GetComponent<UIDocument>();

        LoadMainMenu();
    }

    #region MainMenu    
    private void LoadMainMenu()
    {
        _document.visualTreeAsset = _mainMenuTree;

        // Aggiorna header effects
        StartCoroutine(WaitAndRefresh());
        // Registra bottoni
        VisualElement root = _document.rootVisualElement;

        Button playButton = root.Q<Button>("PlayButton");
        playButton.RegisterCallback<ClickEvent>(OnPlayClick);

        Button exitButton = root.Q<Button>("ExitButton");
        exitButton.RegisterCallback<ClickEvent>(OnExitClick);
    }
    private IEnumerator WaitAndRefresh()
    {
        yield return new WaitForEndOfFrame();

        GetComponent<HeaderEffects>().RefreshHeaders();
    }
    private void OnPlayClick(ClickEvent click)
    {
        AudioManager.Instance.PlaySfx("ButtonClicked", false, 1f);
        LoadChoosePlayerMenu();
    }

    private void OnExitClick(ClickEvent click)
    {
        AudioManager.Instance.PlaySfx("ButtonClicked", false, 1f);
        Application.Quit();
    }
    private void OnBackClick()
    {
        AudioManager.Instance.PlaySfx("ButtonClicked", false, 1f);
        LoadMainMenu();
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

        root.Q<Button>("BackButton")?.RegisterCallback<ClickEvent>(evt => OnBackClick());
    }

    private void StartGame(int players)
    {
        AudioManager.Instance.PlaySfx("ButtonClicked", false, 1f);
        AudioManager.Instance.PlaySfx("SceneTransition", false, 1f);

        //Debug.Log($"number of players : {players}");
        GameManager.PlayerCount = players;
        SceneManager.LoadScene(1);
    }
    #endregion
}
