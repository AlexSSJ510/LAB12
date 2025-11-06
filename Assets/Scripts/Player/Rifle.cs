using UnityEngine;
using UnityEngine.InputSystem;

public class Rifle : MonoBehaviour
{
    public float range = 50f;
    public Key fireKey = Key.F;  // Use 'Key' instead of 'KeyCode' for the new Input System

    private PlayerInputActions _inputActions;

    private void Awake()
    {
        _inputActions = new PlayerInputActions(); // Make sure you have this class set up for input
    }

    private void OnEnable()
    {
        _inputActions.Player.Enable();
        _inputActions.Player.Fire.performed += OnFire;  // Listen for the 'Fire' action
    }

    private void OnDisable()
    {
        _inputActions.Player.Fire.performed -= OnFire;
        _inputActions.Player.Disable();
    }

    private void OnFire(InputAction.CallbackContext context)
    {
        Shoot();
    }

    private void Update()
    {
   
    }

    private void Shoot()
    {
        RaycastHit hit;
        Ray ray = new Ray(transform.position, transform.forward);

        if (Physics.Raycast(ray, out hit, range))
        {
            IInteractable interactable = hit.collider.GetComponent<IInteractable>();
            if (interactable != null)
            {
                interactable.Interact();
                Debug.Log("¡Enemigo aturdido!");
            }
        }
    }
}
