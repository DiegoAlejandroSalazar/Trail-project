using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
[DisallowMultipleComponent]
public class PlayerInputHandler : MonoBehaviour
{
    [Header("Input Action Asset")]
    [SerializeField] private InputActionAsset playerControls;

    [Space]
    [Header("Action Map Name Reference")]
    [SerializeField] private string ActionMapName = "Player_";
    [Space]

    [Header("Action Name Reference")]
    [SerializeField] private string movement = "Move";
    [SerializeField] private string deleteAction = "DeleteAction";

    private InputAction movementAction;
    private InputAction deleteActionAction;

    public Vector2 MovementInput { get; private set; }
    public bool DeleateActionInput { get; private set; } // Variabile per il tasto singolo


    public void Initialize(int playerIndex)
    {
        string mapName = ActionMapName + playerIndex;
        InputActionMap map = playerControls.FindActionMap(mapName);

        if (map == null)
        {
            Debug.LogError($"Mappa non trovata: {mapName}");
            return;
        }

        movementAction = map.FindAction(movement);
        deleteActionAction = map.FindAction(deleteAction);

        SubscribeActionValueToInputEvent();

        map.Enable();
    }

    private void SubscribeActionValueToInputEvent()
    {
        movementAction.performed += inputInfo => MovementInput = inputInfo.ReadValue<Vector2>();
        movementAction.canceled += inputInfo => MovementInput = Vector2.zero;

        deleteActionAction.started += ctx => DeleateActionInput = true;
        deleteActionAction.canceled += ctx => DeleateActionInput = false;
    }
    private void OnEnable()
    {
        StartCoroutine(EnableInputNextFrame());
    }
    IEnumerator EnableInputNextFrame()
    {
        yield return null;

        movementAction?.Enable();
    }
    private void OnDisable()
    {
        movementAction?.Disable();
    }
    private void LateUpdate()
    {
        DeleateActionInput = false;
    }

}
