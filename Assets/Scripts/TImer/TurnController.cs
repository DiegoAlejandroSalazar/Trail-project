using UnityEngine;
using TMPro;
using System.Collections.Generic;
using System.Collections;

public class TurnController : MonoBehaviour
{
    [Header("Timer Settings")]
    [SerializeField] private float _inputWindow = 2f;
    [SerializeField] private TMP_Text _timerText;

    private float _countdown;
    private bool _collecting = false;

    void Start()
    {
        _countdown = _inputWindow;
        _collecting = true;

        foreach (var player in PlayerManager.Instance.Players)
            player.InputQueue.StartCollecting();
    }

    void Update()
    {
        if (!_collecting) return;

        _countdown -= Time.deltaTime;
        _timerText.text = _countdown.ToString("0.0");

        if (_countdown <= 0)
            EndWindow();
    }

    private void EndWindow()
    {
        _collecting = false;
        _timerText.text = "GO!";
        StartCoroutine(StartExecution());
    }

    private IEnumerator StartExecution()
    {
        // Prepara i player
        foreach (var player in PlayerManager.Instance.Players)
            player.InputQueue.PrepareExecution();
        yield return null;

        // Avvia l’esecuzione simultanea
        foreach (var player in PlayerManager.Instance.Players)
            player.InputQueue.StartExecution();

        // Aspetta che tutti abbiano finito
        bool allDone = false;

        while (!allDone)
        {
            allDone = true;

            foreach (var player in PlayerManager.Instance.Players)
            {
                if (player.InputQueue.IsExecuting)
                {
                    allDone = false;
                    break;
                }
            }

            yield return null;
        }

        // Collisioni
        CollisionResolver.Instance.Resolve();


        // Reset turno
        _countdown = _inputWindow;
        _collecting = true;

        foreach (var player in PlayerManager.Instance.Players)
        {
            player.Trail.ClearTrail();
            player.Movement.ResetMoveIndex();
        }

        foreach (var player in PlayerManager.Instance.Players)
            player.InputQueue.StartCollecting();
    }
}
