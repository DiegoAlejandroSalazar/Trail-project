using UnityEngine;
using TMPro;
using System.Collections;
using System.Collections.Generic;

public class PlayerInputQueue : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private PlayerGridMovement _movement;
    [SerializeField] private TMP_Text _debugQueueText;
    private PlayerInputHandler _playerInputHandler;

    [Header("Queue Settings")]
    [SerializeField] private float _actionInterval = 0.3f;

    private readonly Queue<Vector3Int> actionQueue = new();
    private bool _collecting = false;
    public bool IsExecuting { get; private set; } = false;

    private Vector2 _lastInput = Vector2.zero;

    void Start()
    {
        _playerInputHandler = GetComponent<PlayerInputHandler>();
    }

    void Update()
    {
        if (_collecting)
            ReadInput();

        UpdateDebugList();
    }

    public void StartCollecting()
    {
        actionQueue.Clear();
        _collecting = true;
        _lastInput = Vector2.zero;
    }

    public void StopCollectingAndExecute()
    {
        _collecting = false;
        StartCoroutine(ExecuteActions());
    }


    private void ReadInput()
    {
        if (IsExecuting) return;
        
        Vector2 input = _playerInputHandler.MovementInput;

        if (_lastInput == Vector2.zero && input != Vector2.zero)
        {
            if (actionQueue.Count < GameManager.Instance.GetBufferSize())
            {
                Vector3Int dir = Vector3Int.zero;

                if (Mathf.Abs(input.x) > Mathf.Abs(input.y))
                    dir.x = input.x > 0 ? 1 : -1;
                else
                    dir.y = input.y > 0 ? 1 : -1;

                actionQueue.Enqueue(dir);
            }
        }

        _lastInput = input;
    }

    public IEnumerator ExecuteActions()
    {
        IsExecuting = true;

        while (actionQueue.Count > 0)
        {
            Vector3Int next = actionQueue.Dequeue();
            _movement.TryMove(next);

            yield return new WaitUntil(() => _movement.IsFree);
            yield return new WaitForSeconds(_actionInterval);
        }

        IsExecuting = false;
    }
    public void PrepareExecution()
    {
        IsExecuting = false;
    }

    public void StartExecution()
    {
        StartCoroutine(ExecuteActions());
    }


    private void UpdateDebugList()
    {
        if (actionQueue.Count == 0)
        {
            _debugQueueText.text = "";
            return;
        }

        string s = "";
        foreach (var a in actionQueue)
        {
            if (a.x == 1) s += "→ ";
            else if (a.x == -1) s += "← ";
            else if (a.y == 1) s += "↑ ";
            else if (a.y == -1) s += "↓ ";
        }

        _debugQueueText.text = s;
    }
}
