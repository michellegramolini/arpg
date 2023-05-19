using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;

public class UI : MonoBehaviour
{
    [Header("Player Input")]
    private PlayerInput _playerUIInput;
    private InputAction _openInventoryAction;
    private InputAction _pauseMenuAction;
    private GameObject _playerObject;
    private InputActionMap _playerActionMap;

    [Header("UI Components")]
    private GameObject _dialogueBox;
    private GameObject _inventory;
    private GameObject _pauseMenu;
    private Image _portrait;
    private TextMeshProUGUI _name;

    private void OnEnable()
    {
        _openInventoryAction.Enable();
        _openInventoryAction.started += ShowInventory;
        _pauseMenuAction.Enable();
        _pauseMenuAction.started += ShowPauseMenu;
    }

    private void OnDisable()
    {
        _openInventoryAction.started -= ShowInventory;
        _openInventoryAction.Disable();
        _pauseMenuAction.started -= ShowPauseMenu;
        _pauseMenuAction.Disable();
    }

    private void Awake()
    {
        _playerObject = GameObject.Find("Player");
        // player input events
        //_playerUIInput = GameObject.Find("Canvas").GetComponent<PlayerInput>();
        _playerUIInput = _playerObject.GetComponent<PlayerInput>();
        _playerActionMap = _playerUIInput.actions.FindActionMap("Player");
        _openInventoryAction = _playerUIInput.actions["Inventory"];
        _pauseMenuAction = _playerUIInput.actions["Pause"];


        // Set elements
        _dialogueBox = transform.Find("DialogueBox").gameObject;
        _inventory = transform.Find("Inventory").gameObject;
        _pauseMenu = transform.Find("PauseMenu").gameObject;

        // individual UI elements
        _portrait = GameObject.Find("Canvas/DialogueBox/Container/Panel/Image").GetComponent<Image>();
        _name = GameObject.Find("Canvas/DialogueBox/Container/NameText").GetComponent<TextMeshProUGUI>();

    }

    // Start is called before the first frame update
    void Start()
    {
        // Set certain UI elements to inactive
        _dialogueBox.SetActive(false);
        _inventory.SetActive(false);
        _pauseMenu.SetActive(false);
    }

    // Dialogue
    public void HideDialogueBox()
    {
        ResetUI();

        _playerActionMap.Enable();
        _dialogueBox.SetActive(false);
    }

    public void ShowDialogueBox()
    {
        _playerActionMap.Disable();
        _dialogueBox.SetActive(true);
    }

    private void ResetUI()
    {
        _portrait.sprite = null;
        _name.text = null;
    }

    // Inventory
    public void ShowInventory(InputAction.CallbackContext context)
    {
        _playerActionMap.Disable();

        if (_inventory.activeSelf)
        {
            HideInventory();
        }
        else
        {
            _inventory.SetActive(true);
        }
    }

    public void HideInventory()
    {
        _playerActionMap.Enable();
        _inventory.SetActive(false);
    }

    // Pause Menu
    public void ShowPauseMenu(InputAction.CallbackContext context)
    {
        _playerActionMap.Disable();

        if (_pauseMenu.activeSelf)
        {
            HidePauseMenu();
        }
        else
        {
            _pauseMenu.SetActive(true);
        }
    }

    public void HidePauseMenu()
    {
        _playerActionMap.Enable();
        _pauseMenu.SetActive(false);
    }

}
