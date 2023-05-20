using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InteractionColliderController : MonoBehaviour
{
    private InputAction _interactAction;
    private PlayerInput _playerInput;
    private IInteractable _interactable;

    private void OnEnable()
    {
        _interactAction.Enable();
        _interactAction.started += Interact;
    }

    private void OnDisable()
    {
        _interactAction.started -= Interact;
        _interactAction.Disable();
    }

    private void Awake()
    {
        _playerInput = GetComponentInParent<PlayerInput>();
        _interactAction = _playerInput.actions["Interact"];
    }

    private void Interact(InputAction.CallbackContext context)
    {
        if (_interactable != null)
        {
            _interactable.Interact();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out IInteractable interactable))
        {
            _interactable = interactable;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        _interactable = null;
    }

}
