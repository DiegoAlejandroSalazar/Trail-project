using UnityEngine;
using UnityEngine.UIElements;
using System.Collections;
using System.Collections.Generic;

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
        RefreshHeaders();
        StartCoroutine(FlickerRoutine());
    }
    public void RefreshHeaders()
    {
        _headers.Clear();
        _headers.AddRange(_doc.rootVisualElement.Query<Label>(className: "header").ToList());

    }

    private IEnumerator FlickerRoutine()
    {
        while (_running)
        {
            foreach (var h in _headers)
            {
                h.style.opacity = _opacity;
                h.style.unityTextOutlineColor = new StyleColor(_color);
            }
            yield return new WaitForSeconds(UnityEngine.Random.Range(0.05f, 0.15f));

            foreach (var h in _headers)
            {
                h.style.opacity = 1f;
                h.style.unityTextOutlineColor = new StyleColor(Color.black);
            }
            yield return new WaitForSeconds(UnityEngine.Random.Range(-_flickerTime, _flickerTime));
        }
    }
}
