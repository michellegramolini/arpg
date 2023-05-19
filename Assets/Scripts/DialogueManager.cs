using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;
using UnityEngine.InputSystem;

public class DialogueManager : MonoBehaviour
{
    // invoke event to play sound or whatever
    public static event Action OnShowTextLetter;

    private GameObject _playerObject;
    private PlayerInput _playerUIInput;
    //private InputActionMap _playerActionMap;

    [Header("Dialogue UI Elements")]
    public TextMeshProUGUI dialogueTextUI;

    private Queue<string> _texts;

    float numchars;

    [SerializeField]
    private float _initTypingSpeed = 0.04f;
    [SerializeField]
    private float _typingSpeedFactor = 3f;
    // tracking value
    private float _typingSpeed;

    Coroutine displayLineCoroutine;
    //Coroutine interactCooldownCoroutine;

    [Header("Interacting")]
    private InputAction _continue;
    public float interactValue;

    private UI _ui;

    private bool _lineInProgress;

    //private Data data;

    private void OnEnable()
    {
        _continue.Enable();
        _continue.started += Continue;
    }

    private void OnDisable()
    {
        _continue.started -= Continue;
        _continue.Disable();
    }

    private void Awake()
    {
        _playerObject = GameObject.Find("Player");
        //_playerUIInput = GameObject.Find("Canvas").GetComponent<PlayerInput>();
        _playerUIInput = _playerObject.GetComponent<PlayerInput>();

        // is this the right map?
        //_playerActionMap = _playerUIInput.actions.FindActionMap("Player");
        _continue = _playerUIInput.actions["Read"];
        _ui = GameObject.Find("Canvas").GetComponent<UI>();
        _texts = new Queue<string>();

        // Dialogue text UI element
        dialogueTextUI = GameObject.Find("Canvas/DialogueBox/Container/Text (TMP)").GetComponent<TextMeshProUGUI>();
        //data = GameObject.Find("Player").GetComponent<PlayerController>().data;
    }
    // Start is called before the first frame update
    void Start()
    {
        numchars = 0f;
    }

    private void Update()
    {
        TrackNumChars(2f);
    }

    // sets _texts to whatever is passed here
    public void StartDialogue(string[] lines)
    {
        _typingSpeed = _initTypingSpeed;
        //StopTime();
        //data.CanInteract = false;
        //_playerActionMap.Disable();

        _ui.ShowDialogueBox();
        _texts.Clear();

        foreach (string text in lines)
        {
            _texts.Enqueue(text);
        }

        DisplayNext(dialogueTextUI);
    }

    private void Continue(InputAction.CallbackContext context)
    {
        //if (!data.CanInteract)
        //{
        //    DisplayNext(dialogueTextUI);
        //}

        if (!_lineInProgress)
        {
            DisplayNext(dialogueTextUI);
        }
        else
        {
            _typingSpeed = _initTypingSpeed / _typingSpeedFactor;
        }
    }

    // Call from button
    public void DisplayNext(TextMeshProUGUI textElem)
    {
        _typingSpeed = _initTypingSpeed;

        if (_texts.Count == 0)
        {
            EndDialogue();
            return;
        }

        string text = _texts.Dequeue();

        if (displayLineCoroutine != null)
        {
            StopCoroutine(displayLineCoroutine);
        }

        displayLineCoroutine = StartCoroutine(DisplayLineTypeEffect(text, textElem));

    }

    IEnumerator DisplayLineTypeEffect(string text, TextMeshProUGUI textElem)
    {
        _lineInProgress = true;

        textElem.text = "";

        foreach (char letter in text.ToCharArray())
        {
            textElem.text += letter;
            numchars += 1;

            yield return new WaitForSecondsRealtime(_typingSpeed);
        }

        _lineInProgress = false;
    }

    void TrackNumChars(float num)
    {
        // reset numchars when number of dialogue chars displayed hits limit num
        if (numchars >= num)
        {
            // reset
            numchars = 0f;
            // invoke event to play sound or whatever
            OnShowTextLetter?.Invoke();
        }

    }

    public void EndDialogue()
    {
        numchars = 0f;
        //ResumeTime();
        _ui.HideDialogueBox();
        // TODO: cooldown?

        //if (interactCooldownCoroutine != null)
        //{
        //    StopCoroutine(interactCooldownCoroutine);
        //}

        //interactCooldownCoroutine = StartCoroutine(InteractCooldown());
    }

    //private IEnumerator InteractCooldown()
    //{
    //    yield return new WaitForEndOfFrame();
    //    //data.CanInteract = true;
    //    _playerActionMap.Enable();
    //}

    //void StopTime()
    //{
    //    Time.timeScale = 0;
    //}

    //void ResumeTime()
    //{
    //    Time.timeScale = 1;
    //}

}