using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;
using SuperTiled2Unity;
using System;
using static UnityEditor.Experimental.GraphView.GraphView;

public class PlayerController : MonoBehaviour, IAttackable
{
    private PlayerInput _playerInput;

    [Header("Player Data")]
    private PlayerData _playerData;

    [Header("States")]
    public State currentState;
    public State previousState;
    public Idle Idle;
    public Walk Walk;
    public Run Run;
    public Shoot Shoot;
    public Damaged Damaged;
    public Dead Dead;
    public Jump Jump;
    public Swim Swim;
    public Fall Fall;
    public MeleeAttack MeleeAttack;

    [Header("Components")]
    public Rigidbody2D rb;
    public BoxCollider2D col;
    private BoxCollider2D interactionCollider;

    [Header("Inputs")]
    private InputAction _moveAction;
    private InputAction _shiftAction;
    private InputAction _jumpAction;
    private InputAction _attackAction;

    [Header("Movement Speeds")]

    public float runSpeed;
    public float walkSpeed;
    public float swimSpeed;
    public float fallSpeed;
    public float jumpSpeed;

    [Header("Movement Vectors")]
    public Vector2 moveVector;
    public Vector2 previousMoveVector;
    public Vector2 idleVector;
    public bool isMovingRight;
    public bool isMovingLeft;
    public bool isMovingUp;
    public bool isMovingDown;
    public bool isMovingUpRight;
    public bool isMovingUpLeft;
    public bool isMovingDownRight;
    public bool isMovingDownLeft;
    public bool wasMovingRight;
    public bool wasMovingLeft;
    public bool wasMovingUp;
    public bool wasMovingDown;
    public bool wasMovingUpRight;
    public bool wasMovingUpLeft;
    public bool wasMovingDownRight;
    public bool wasMovingDownLeft;

    public Vector2 locked_moveVector;
    public Vector2 locked_previousMoveVector;
    public bool locked_isMovingRight;
    public bool locked_isMovingLeft;
    public bool locked_isMovingUp;
    public bool locked_isMovingDown;
    public bool locked_isMovingUpRight;
    public bool locked_isMovingUpLeft;
    public bool locked_isMovingDownRight;
    public bool locked_isMovingDownLeft;
    public bool locked_wasMovingRight;
    public bool locked_wasMovingLeft;
    public bool locked_wasMovingUp;
    public bool locked_wasMovingDown;
    public bool locked_wasMovingUpRight;
    public bool locked_wasMovingUpLeft;
    public bool locked_wasMovingDownRight;
    public bool locked_wasMovingDownLeft;

    [Header("Facing Direction")]
    public Vector2 facingDirection;
    public Vector2 locked_facingDirection;

    [Header("Jump")]
    public bool isJumping;
    public Vector2 jumpVector;

    [Header("Shifting")]
    public bool isShifting;

    [Header("Interacting")]
    private Vector2 _detectionPoint;

    [Header("Attack")]
    public bool isAttacking;
    public Vector2 attackPoint;
    public float attackRange;

    [Header("Enemies")]
    public LayerMask enemyLayer;
    public LayerMask enemyLayerCollision;

    [Header("Animation")]
    public AnimationState animationState;

    [Header("Height")]
    public int z;

    [Header("Tile Detector")]
    public TileDetector tileDetector;
    public bool canMove;
    public bool canSwim;
    public bool onWall;
    private Vector2 _adjacentTileDetectionPoint;
    private Transform _feet;

    // TODO: player data
    [Header("XP")]
    // Testing
    public float currentXP;
    [SerializeField]
    private int _currentLevel;
    private float _a, _b;

    [Header("Damage and Death")]
    public bool isKnockedBack;
    public float knockbackForce;
    public int health;


    #region Event Handlers
    private void HandleXPChange(int newXP)
    {
        currentXP += newXP;
        Debug.Log($"{currentXP} current xp, {GetMaxXP(_currentLevel, _a, _b)} xp needed to level up");
        if (currentXP >= GetMaxXP(_currentLevel, _a, _b))
        {
            // TODO: total xp
            LevelUp();
        }
    }
    #endregion


    private void OnEnable()
    {
        _shiftAction.Enable();
        _shiftAction.started += HitShift;
        _shiftAction.canceled += CancelShift;
        _jumpAction.started += HitJump;
        _jumpAction.canceled += CancelJump;
        _attackAction.started += HitAttack;
        _attackAction.canceled += CancelAttack;

        // XP
        XPManager.Instance.OnXPChange += HandleXPChange;
    }

    private void OnDisable()
    {
        _shiftAction.started -= HitShift;
        _shiftAction.canceled -= CancelShift;
        _jumpAction.started -= HitJump;
        _jumpAction.canceled -= CancelJump;
        _attackAction.started -= HitAttack;
        _attackAction.canceled -= CancelAttack;
        _shiftAction.Disable();

        // XP
        XPManager.Instance.OnXPChange -= HandleXPChange;
    }

    private void HitShift(InputAction.CallbackContext context)
    {
        isShifting = true;
    }

    private void CancelShift(InputAction.CallbackContext context)
    {
        isShifting = false;
    }

    private void HitJump(InputAction.CallbackContext context)
    {
        isJumping = true;
    }

    private void CancelJump(InputAction.CallbackContext context)
    {
        isJumping = false;
    }

    private void HitAttack(InputAction.CallbackContext context)
    {
        isAttacking = true;
    }

    private void CancelAttack(InputAction.CallbackContext context)
    {
        isAttacking = false;
    }

    private void Awake()
    {
        // Input
        _playerInput = GetComponent<PlayerInput>();
        _moveAction = _playerInput.actions["Move"];
        _shiftAction = _playerInput.actions["Shift"];
        _jumpAction = _playerInput.actions["Jump"];
        _attackAction = _playerInput.actions["Attack"];

        // Collisions
        interactionCollider = transform.Find("InteractionCollider").GetComponent<BoxCollider2D>();
        _feet = transform.Find("Feet");
        // Vector default for when there is no player movement input
        idleVector = new Vector2(0f, 0f);

        // start facing right
        facingDirection = new Vector2(1f, 0f);
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
        Jump = gameObject.AddComponent<Jump>();
        Swim = gameObject.AddComponent<Swim>();
        Fall = gameObject.AddComponent<Fall>();
        MeleeAttack = gameObject.AddComponent<MeleeAttack>();

        // Get components
        rb = gameObject.GetComponent<Rigidbody2D>();
        col = gameObject.GetComponent<BoxCollider2D>();
        animationState = gameObject.GetComponentInChildren<AnimationState>();
        tileDetector = gameObject.GetComponentInChildren<TileDetector>();

        // Layers
        enemyLayer = LayerMask.GetMask("Enemy");
        enemyLayerCollision = LayerMask.NameToLayer("Enemy");

        // Init Height
        z = GetCurrentZ();

        // Init State
        currentState = Idle;
        currentState.StartState(this);

        // XP
        _currentLevel = 4;
        currentXP = 0;
        _a = 1;
        _b = 1.1f;

        // Health
        health = 5;

        //// Move Vector
        //moveVector = idleVector;
    }

    // TODO: dictionary?
    private void SetMoveVectors()
    {
        // if moveVector is changing
        if (moveVector != _moveAction.ReadValue<Vector2>())
        {
            previousMoveVector = moveVector;
            // x and y 0, 1, or -1
            moveVector = _moveAction.ReadValue<Vector2>();
        }

        isMovingRight = (moveVector == Vector2.right);
        isMovingLeft = (moveVector == Vector2.left);
        isMovingUp = (moveVector == Vector2.up);
        isMovingDown = (moveVector == Vector2.down);
        isMovingUpRight = (moveVector.x > 0 && moveVector.y > 0);
        isMovingUpLeft = (moveVector.x < 0 && moveVector.y > 0);
        isMovingDownRight = (moveVector.x > 0 && moveVector.y < 0);
        isMovingDownLeft = (moveVector.x < 0 && moveVector.y < 0);

        wasMovingRight = (previousMoveVector == Vector2.right);
        wasMovingLeft = (previousMoveVector == Vector2.left);
        wasMovingUp = (previousMoveVector == Vector2.up);
        wasMovingDown = (previousMoveVector == Vector2.down);
        wasMovingUpRight = (previousMoveVector.x > 0 && previousMoveVector.y > 0);
        wasMovingUpLeft = (previousMoveVector.x < 0 && previousMoveVector.y > 0);
        wasMovingDownRight = (previousMoveVector.x > 0 && previousMoveVector.y < 0);
        wasMovingDownLeft = (previousMoveVector.x < 0 && previousMoveVector.y < 0);
    }

    // TODO: dictionary?
    public void SetLockedMoveVectors()
    {
        locked_facingDirection = facingDirection;
        locked_moveVector = moveVector;
        locked_previousMoveVector = previousMoveVector;
        locked_isMovingRight = isMovingRight;
        locked_isMovingLeft = isMovingLeft;
        locked_isMovingUp = isMovingUp;
        locked_isMovingDown = isMovingDown;
        locked_isMovingUpRight = isMovingUpRight;
        locked_isMovingUpLeft = isMovingUpLeft;
        locked_isMovingDownRight = isMovingDownRight;
        locked_isMovingDownLeft = isMovingDownLeft;
        locked_wasMovingRight = wasMovingRight;
        locked_wasMovingLeft = wasMovingLeft;
        locked_wasMovingUp = wasMovingUp;
        locked_wasMovingDown = wasMovingDown;
        locked_wasMovingUpRight = wasMovingUpRight;
        locked_wasMovingUpLeft = wasMovingUpLeft;
        locked_wasMovingDownRight = wasMovingDownRight;
        locked_wasMovingDownLeft = wasMovingDownLeft;
    }

    // Update is called once per frame
    void Update()
    {
        SetMoveVectors();

        if (facingDirection != idleVector)
        {
            // TODO: rename this more aptly
            _detectionPoint = new Vector2(transform.position.x, transform.position.y) + facingDirection;
            attackPoint = _detectionPoint;
            interactionCollider.transform.position = _detectionPoint;
            //_adjacentTileDetectionPoint = new Vector2(_feet.position.x, _feet.position.y) + (-facingDirection * .6f);
            if (currentState == Damaged)
            {
                _adjacentTileDetectionPoint = GetAdjacentTileDetectionPoint(-facingDirection);
            }
            else
            {
                _adjacentTileDetectionPoint = GetAdjacentTileDetectionPoint(facingDirection);
            }
        }

        SetFacingDirection();
        EnableMovement();
        SetCurrentZ();

        // Temporary TODO: Kill her!
        if (health <= 0)
        {
            Debug.Log("She ded.");
        }

        currentState.UpdateState(this);
    }

    private Vector2 GetAdjacentTileDetectionPoint(Vector2 orientationVector)
    {
        return new Vector2(_feet.position.x, _feet.position.y) + (orientationVector * .6f);
    }

    private void SetCurrentZ()
    {
        // HACK:
        if (currentState == Walk || currentState == Run || currentState == Idle || currentState == Swim || currentState == Damaged)
        {
            if (GetCurrentZ() < z)
            {
                z = GetCurrentZ();
            }
        }
    }

    private void FixedUpdate()
    {
        HandleKnockback();

        currentState.FixedUpdateState(this);
    }

    private void HandleKnockback()
    {
        if (isKnockedBack)
        {
            if (canMove)
            {
                rb.velocity = -facingDirection * knockbackForce;
            }
            else
            {
                rb.velocity = Vector2.zero;
            }
        }
    }

    private void LateUpdate()
    {
        currentState.LateUpdateState(this);
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
            if (wasMovingRight)
            {
                if (isMovingRight || isMovingDownRight || isMovingUpRight)
                {
                    // face right
                    facingDirection = Vector2.right;
                }
                else
                {
                    DefaultSetFacing();
                }

            }
            else if (wasMovingLeft)
            {
                if (isMovingLeft || isMovingDownLeft || isMovingUpLeft)
                {
                    // face left
                    facingDirection = Vector2.left;
                }
                else
                {
                    DefaultSetFacing();
                }
            }
            else if (wasMovingUp)
            {
                if (isMovingUp || isMovingUpRight || isMovingUpLeft)
                {
                    // face up
                    facingDirection = Vector2.up;
                }
                else
                {
                    DefaultSetFacing();
                }
            }
            else if (wasMovingDown)
            {
                if (isMovingDown || isMovingDownRight || isMovingDownLeft)
                {
                    // face down
                    facingDirection = Vector2.down;
                }
                else
                {
                    DefaultSetFacing();
                }
            }
            else
            {
                DefaultSetFacing();
            }
        }

    }

    private void DefaultSetFacing()
    {
        if (moveVector.x > 0f)
        {
            facingDirection = Vector2.right;
        }
        else if (moveVector.x < 0f)
        {
            facingDirection = Vector2.left;
        }
        else if (moveVector.x == 0f && moveVector.y > 0f)
        {
            facingDirection = Vector2.up;
        }
        else
        {
            facingDirection = Vector2.down;
        }
    }

    private int GetCurrentZ()
    {
        if (tileDetector.GetTileProp("current", "height_value") != null)
        {
            return tileDetector.GetTileProp("current", "height_value").m_Value.ToInt();
        }

        return 0;
    }

    private void DetectNextTile(Vector2 detectionPoint)
    {
        SuperTile heightTile = tileDetector.GetHeightTile(detectionPoint);
        CustomProperty heightProp = tileDetector.GetTilePropFromSuperTile(heightTile, "height_value");

        SuperTile terrainTile = tileDetector.GetTerrainTile(detectionPoint);
        CustomProperty terrainProp = tileDetector.GetTilePropFromSuperTile(terrainTile, "terrain");

        // All walkable tiles should have a height_value property.
        if (heightProp != null)
        {
            if (heightProp.m_Value.ToInt() > z)
            {
                canMove = false;
            }
            // Creating an invisible collision when faced with a ledge.
            else if (heightProp.m_Value.ToInt() < z)
            {
                // TODO: or Leap
                if (currentState == Jump)
                {
                    canMove = true;
                }
                else
                {
                    canMove = false;
                }
            }
            else // z is equal
            {
                canMove = true;
                // Wall handling
                if (terrainProp != null)
                {
                    if (terrainProp.m_Value == "wall" && currentState != Jump)
                    {
                        canMove = false;
                    }
                }
            }
        }
        else
        {
            canMove = false;
        }
    }

    private void DetectCurrentTile()
    {
        //SuperTile heightTile = tileDetector.GetHeightTile(_adjacentTileDetectionPoint);
        //CustomProperty heightProp = tileDetector.GetTilePropFromSuperTile(heightTile, "height_value");

        SuperTile tile = tileDetector.GetTerrainTile(_feet.transform.position);
        CustomProperty terrainProp = tileDetector.GetTilePropFromSuperTile(tile, "terrain");

        if (terrainProp != null)
        {
            if (terrainProp.m_Value == "water")
            {
                canSwim = true;
            }
            else
            {
                canSwim = false;
            }

            if (terrainProp.m_Value == "wall")
            {
                onWall = true;
            }
            else
            {
                onWall = false;
            }
        }
        else
        {
            canSwim = false;
            onWall = false;
        }

    }

    private void EnableMovement()
    {
        DetectCurrentTile();
        DetectNextTile(_adjacentTileDetectionPoint);
    }

    public void Hit()
    {
        if (currentState != Damaged)
        {
            SetState(Damaged);
        }
    }

    #region Experience XP Stuff
    private void LevelUp()
    {
        _currentLevel++;
        // TODO: total xp
        currentXP = 0f;
        //Debug.Log("Level Up!");
        XPManager.Instance.LevelUpPopup(transform.position, _currentLevel);
    }

    // Given a player's level, return the amount of xp needed to reach the next level.
    private double GetMaxXP(int level, float a, float b)
    {
        double xp = a * Mathf.Sqrt(level) + b;
        xp = Mathf.Ceil((float)xp);
        return xp;
    }
    #endregion

    private void OnDrawGizmos()
    {
        // Display the explosion radius when selected
        //Gizmos.color = Color.yellow;
        //Gizmos.DrawWireSphere(_detectionPoint, .5f);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(attackPoint, attackRange);
        //if (currentState == Damaged)
        //{
        //    Gizmos.color = Color.red;
        //    Gizmos.DrawWireSphere(_adjacentTileDetectionPoint, .5f);
        //}
    }


}
