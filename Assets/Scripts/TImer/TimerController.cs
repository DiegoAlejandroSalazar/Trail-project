using UnityEngine;
using TMPro;
using System.Collections.Generic;
using System.Collections;

public class TimerController : MonoBehaviour
{
    [Header("Timer Settings")]
    [SerializeField] private float _inputWindow = 2f;
    [SerializeField] private TMP_Text _timerText;

    private float _countdown;
    private bool _collecting = false;

    [SerializeField] private List<PlayerInputQueue> players = new();

    void Start()
    {
        _countdown = _inputWindow;
        _collecting = true;

        foreach (PlayerInputQueue p in players)
            p.StartCollecting();
    }

    void Update()
    {
        if (!_collecting) return;

        _countdown -= Time.deltaTime;
        _timerText.text = _countdown.ToString("0.0");

        if (_countdown <= 0)
        {
            EndWindow();
        }
    }

    private void EndWindow()
    {
        _collecting = false;
        _timerText.text = "GO!";

        StartCoroutine(StartExecution());
    }

    private IEnumerator StartExecution()
    {
        foreach (PlayerInputQueue p in players)
            p.PrepareExecution();

        //Aspetta un frame per sincronizzazione perfetta
        yield return null;

        foreach (PlayerInputQueue p in players)
            p.StartExecution();

        bool allDone = false;

        while (!allDone)
        {
            allDone = true;

            foreach (PlayerInputQueue p in players)
            {
                if (p.IsExecuting)
                {
                    allDone = false;
                    break;
                }
            }

            yield return null;
        }

        _countdown = _inputWindow;
        _collecting = true;

        foreach (PlayerInputQueue p in players)
            p.StartCollecting();
    }
}
