using UnityEngine;
using UnityEngine.UIElements;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(UIDocument))]
public class HeaderEffects : MonoBehaviour
{
    private UIDocument _doc;
    private List<Label> _headers = new();

    [Header("Flicker Settings")]
    [SerializeField] private bool _running = true;
    [SerializeField] private float _opacity = 0.8f;
    [SerializeField] private float _flickerTime = 0.15f;
    [SerializeField] private Color _color;


    private void Awake()
    {
        _doc = GetComponent<UIDocument>();
        //RefreshHeaders();
        StartCoroutine(FlickerRoutine());
    }
    public void RefreshHeaders()
    {
        if (_doc == null) _doc = GetComponent<UIDocument>();

        if (_doc.rootVisualElement == null) return;

        _headers.Clear();
        var foundHeaders = _doc.rootVisualElement.Query<Label>(className: "header").ToList();

        if (foundHeaders != null)
        {
            _headers.AddRange(foundHeaders);
        }

        // foreach (Label label in _headers)
        // {
        //     Debug.Log("Header trovato: " + label.name);
        // }
    }

    private IEnumerator FlickerRoutine()
    {
        while (_running)
        {
            if (_headers.Count == 0)
            {
                yield return new WaitForSeconds(0.5f);
                continue;
            }

            foreach (var h in _headers)
            {
                if (h == null) continue; 
                h.style.opacity = _opacity;
                h.style.unityTextOutlineColor = new StyleColor(_color);
            }
            yield return new WaitForSeconds(Random.Range(0.05f, 0.15f));

            foreach (var h in _headers)
            {
                if (h == null) continue;
                h.style.opacity = 1f;
                h.style.unityTextOutlineColor = new StyleColor(Color.black);
            }

            // Assicurati che _flickerTime sia positivo.
            float wait = Mathf.Max(0.05f, Random.Range(0.05f, _flickerTime));
            yield return new WaitForSeconds(wait);
        }
    }
}
