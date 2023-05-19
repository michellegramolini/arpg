using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InteractionColliderController : MonoBehaviour
{
    private InputAction _interactAction;
    private PlayerInput _playerInput;
    private PlayerController _playerController;
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
        _playerController = GetComponentInParent<PlayerController>();
        _interactAction = _playerInput.actions["Interact"];
    }

    private void Interact(InputAction.CallbackContext context)
    {
        //if (_playerController.data.CanInteract)
        //{
        //    Debug.Log("called Interact on Player");

        //    if (_interactable != null)
        //    {
        //        _interactable.Interact();
        //    }
        //}

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
        else
        {
            _interactable = null;
        }
    }
}
