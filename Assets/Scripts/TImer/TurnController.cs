using UnityEngine;
using TMPro;
using System.Collections;

public class TurnController : MonoBehaviour
{
    [Header("UiReferences")]
    [SerializeField] private TMP_Text _timerText;
    [Header("Timer Settings")]
    [SerializeField] private float _tranistionTime = 1f;

    private float _countdown;
    private bool _collecting = false;

    void Start()
    {
        _countdown = GameManager.Instance.GetInputWindow();
        _collecting = true;
        GameUIManager.Instance.UpdateUi();

        foreach (PlayerData player in PlayerManager.Instance.Players)
        {
            player.InputQueue.StartCollecting();
        }

        FallingObjectManager.Instance.InitializePattern();
    }

    void Update()
    {
        if(GameManager.Instance.GameFinish) return;
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
        if (!GameManager.Instance.GameFinish)
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
            FallingObjectManager.Instance.ExecutePattern();

            yield return new WaitForSeconds(_tranistionTime);

            // Reset turno
            _countdown = GameManager.Instance.GetInputWindow();
            _collecting = true;


            foreach (var player in PlayerManager.Instance.Players)
            {
                player.Trail.ClearTrail();
                player.Movement.ResetMoveIndex();
            }

            GameManager.Instance.NextTurn();
            GameUIManager.Instance.UpdateUi();

            foreach (var player in PlayerManager.Instance.Players)
                player.InputQueue.StartCollecting();

            FallingObjectManager.Instance.InitializePattern();
        }
    }
}
