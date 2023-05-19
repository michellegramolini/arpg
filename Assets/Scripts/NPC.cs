using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class NPC : MonoBehaviour, IInteractable
{
    [Header("Dialogue")]
    private DialogueManager _dialogueManager;
    private Image _portrait;
    private TextMeshProUGUI _name;

    [SerializeField]
    private string[] _sentences;
    [SerializeField]
    private NPCScriptable data;

    private void Awake()
    {
        _dialogueManager = GameObject.Find("DialogueManager").GetComponent<DialogueManager>();
        _portrait = GameObject.Find("Canvas/DialogueBox/Container/Panel/Image").GetComponent<Image>();
        _name = GameObject.Find("Canvas/DialogueBox/Container/NameText").GetComponent<TextMeshProUGUI>();

    }

    // Set the portrait and other attributes for this NPC's dialogue UI elements
    private void SetDialogueUI()
    {
        _portrait.sprite = data.NPCPortrait;
        _name.text = data.NPCName;
    }

    public void Interact()
    {
        SetDialogueUI();
        TriggerDialogue();
    }

    private void TriggerDialogue()
    {
        _dialogueManager.StartDialogue(_sentences);
    }
}
