using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using UnityEngine.Audio;

[RequireComponent(typeof(UIDocument))]
[RequireComponent(typeof(HeaderEffects))]
public class MainMenuEvents : MonoBehaviour
{
    private UIDocument _document;
    [SerializeField]private AudioMixer mixer;

    [Header("Visual Trees")]
    [SerializeField] private VisualTreeAsset _mainMenuTree;
    [SerializeField] private VisualTreeAsset _choosePlayerVisualTree;
    [SerializeField] private VisualTreeAsset _optionVisualTree;
    [SerializeField] private AudioClip ost;

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
        StartCoroutine(WaitAndRefresh());
        // Registra bottoni
        VisualElement root = _document.rootVisualElement;

        Button playButton = root.Q<Button>("PlayButton");
        playButton.RegisterCallback<ClickEvent>(OnPlayClick);

        Button optionsButton = root.Q<Button>("OptionsButton");
        optionsButton.RegisterCallback<ClickEvent>(OnOptionClick);

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

    private void OnOptionClick(ClickEvent click)
    {
        _document.visualTreeAsset = _optionVisualTree;
        var root = _document.rootVisualElement;
	
	Slider slider1 = root.Q<Slider>("SoundFXVolume");  
	Slider slider2 = root.Q<Slider>("MusicVolume");  

        slider1.RegisterValueChangedCallback(evt =>
        {
	SetMixerVolume("AudioFXVolume", evt.newValue);
            Debug.Log("AudioFXVolume : " + evt.newValue);
        });

        slider2.RegisterValueChangedCallback(evt =>
        {
	SetMixerVolume("MusicaVolume", evt.newValue);
            Debug.Log("MusicVolume: " + evt.newValue);
        });

        root.Q<Button>("BackToMenu")?.RegisterCallback<ClickEvent>(evt => LoadMainMenu());
    }

	private void SetMixerVolume(string exposedParam, float sliderValue)
{
// Evita log(0)
    if (sliderValue <= 0.0001f)
    {
        mixer.SetFloat(exposedParam, -80f); // muto
        return;
    }

    // Conversione lineare → decibel
    float dB = Mathf.Log10(sliderValue) * 20f;

    mixer.SetFloat(exposedParam, dB);
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
