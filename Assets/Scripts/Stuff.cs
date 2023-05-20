using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

// TODO: rough sketch
// FIXME: override NPC or create a different class for Item
public class Stuff : MonoBehaviour, IInteractable
{
    // reference to HUD counter
    private TextMeshProUGUI _counter;

    [SerializeField]
    private string[] _sentences;

    [Header("Dialogue")]
    private DialogueManager _dialogueManager;

    private void Awake()
    {
        _counter = GameObject.Find("Canvas/HUD/StuffContainer/StuffCounter").GetComponent<TextMeshProUGUI>();
        _dialogueManager = GameObject.Find("DialogueManager").GetComponent<DialogueManager>();
    }

    public void Interact()
    {
        if (gameObject.activeSelf)
        {
            TriggerDialogue();

            int count = int.Parse(_counter.text) + 1;
            // TODO: get the current count
            _counter.text = count.ToString();
            //TODO: object pooler
            gameObject.SetActive(false);
        }

    }

    private void TriggerDialogue()
    {
        _dialogueManager.StartDialogue(_sentences);
    }

}
