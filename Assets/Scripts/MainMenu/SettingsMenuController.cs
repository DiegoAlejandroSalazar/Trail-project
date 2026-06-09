using UnityEngine;
using UnityEngine.UIElements;

public class SettingsMenuController : MonoBehaviour
{
    private VisualElement root;
    private Button backButton;
    private Slider slider1;
    private Slider slider2;

    private void OnEnable()
    {
        root = GetComponent<UIDocument>().rootVisualElement;

        // Recupero elementi dal UXML
        backButton = root.Q<Button>("BacktoMenu");
        slider1 = root.Q<Slider>("SoundFXVolume");
        slider2 = root.Q<Slider>("MusicVolume");

        // Eventi
        backButton.clicked += Close;

        slider1.RegisterValueChangedCallback(evt =>
        {
            Debug.Log("SoundFXVolume : " + evt.newValue);
        });

        slider2.RegisterValueChangedCallback(evt =>
        {
            Debug.Log("MusicVolume: " + evt.newValue);
        });

        // All'inizio il menu è nascosto
        Close();
    }

    public void Open()
    {
        root.style.display = DisplayStyle.Flex;
    }

    public void Close()
    {
        root.style.display = DisplayStyle.None;
    }
}
