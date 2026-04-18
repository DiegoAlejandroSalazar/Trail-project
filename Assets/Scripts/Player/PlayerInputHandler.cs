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
    [SerializeField] private string ActionMapName = "Player";
    [Space]

    [Header("Action Name Reference")]
    [SerializeField] private string movement = "Move";
    [SerializeField] private string rotation = "Look";
 
    private InputAction movementAction;
    private InputAction rotationAction;

    public Vector2 MovementInput { get; private set; }
    public Vector2 RotationInput { get; private set; }

    public static PlayerInputHandler Instance;

    void Awake()
    {
        InputActionMap MapReference = playerControls.FindActionMap(ActionMapName);

        if (MapReference == null)
        {
            Debug.LogError("Action Map NOT FOUND: " + ActionMapName);
        }

        movementAction = MapReference.FindAction(movement);
        rotationAction = MapReference.FindAction(rotation);
        SubscribeActionValueToInputEvent();


        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        MapReference.Enable();
    }
    private void SubscribeActionValueToInputEvent()
    {
        movementAction.performed += inputInfo => MovementInput = inputInfo.ReadValue<Vector2>();
        movementAction.canceled += inputInfo => MovementInput = Vector2.zero;

        rotationAction.performed += inputInfo => RotationInput = inputInfo.ReadValue<Vector2>();
        rotationAction.canceled += inputInfo => RotationInput = Vector2.zero;


    }
    private void OnEnable()
    {
        StartCoroutine(EnableInputNextFrame());
    }
    IEnumerator EnableInputNextFrame()
    {
        yield return null;

        playerControls.FindActionMap(ActionMapName).Enable();
    }
    void OnDisable()
    {
        playerControls.FindActionMap(ActionMapName).Disable();
    }

}
