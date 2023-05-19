using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private PlayerInput _playerInput;

    [Header("States")]
    public State currentState;
    public State previousState;
    public Idle Idle;
    public Walk Walk;
    public Run Run;
    public Shoot Shoot;
    public Damaged Damaged;
    public Dead Dead;

    [Header("Components")]
    public Rigidbody2D rb;
    public BoxCollider2D col;
    private BoxCollider2D interactionCollider;

    [Header("Movement")]
    private InputAction _moveAction;

    public Vector2 moveVector;
    public float walkSpeed;
    public Vector2 idleVector;
    public Vector2 facingDirection;

    [Header("Interacting")]
    private Vector2 _detectionPoint;

    // Data class
    public Data data;

    [Header("Animation")]
    public AnimationState animationState;

    private void Awake()
    {
        _playerInput = GetComponent<PlayerInput>();
        _moveAction = _playerInput.actions["Move"];
        interactionCollider = transform.Find("InteractionCollider").GetComponent<BoxCollider2D>();

        idleVector = new Vector2(0f, 0f);

        // start facing right
        facingDirection = new Vector2(1f, 0f);

        // data class
        data = new Data();
    }

    // Start is called before the first frame update
    void Start()
    {
        // States
        Idle = gameObject.AddComponent<Idle>();
        Walk = gameObject.AddComponent<Walk>();
        Run = gameObject.AddComponent<Run>();
        Shoot = gameObject.AddComponent<Shoot>();
        Damaged = gameObject.AddComponent<Damaged>();
        Dead = gameObject.AddComponent<Dead>();

        // Get components
        rb = gameObject.GetComponent<Rigidbody2D>();
        col = gameObject.GetComponent<BoxCollider2D>();
        animationState = gameObject.GetComponent<AnimationState>();

        // Init State
        currentState = Idle;
        currentState.StartState(this);
    }

    // Update is called once per frame
    void Update()
    {
        // x and y 0, 1, or -1
        moveVector = _moveAction.ReadValue<Vector2>();

        if (facingDirection != idleVector)
        {
            _detectionPoint = new Vector2(transform.position.x, transform.position.y) + facingDirection;
            interactionCollider.transform.position = _detectionPoint;
        }

        SetFacingDirection();
        //Detection();

        currentState.UpdateState(this);
    }

    private void FixedUpdate()
    {
        currentState.FixedUpdateState(this);
    }

    public void SetState(State state)
    {
        if (state == currentState)
        {
            return;
        }
        this.previousState = currentState;
        this.currentState = state;

        state.StartState(this);
    }

    private void SetFacingDirection()
    {
        if (moveVector != idleVector)
        {
            // should stay on latest facing
            facingDirection = moveVector;
        }
    }

    private void OnDrawGizmos()
    {
        // Display the explosion radius when selected
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(_detectionPoint, .5f);
    }
}
