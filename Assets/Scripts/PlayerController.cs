using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;
using SuperTiled2Unity;
using System;

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
    public Jump Jump;
    public Swim Swim;
    public Fall Fall;

    [Header("Components")]
    public Rigidbody2D rb;
    public BoxCollider2D col;
    private BoxCollider2D interactionCollider;

    [Header("Inputs")]
    private InputAction _moveAction;
    private InputAction _shiftAction;
    private InputAction _jumpAction;

    [Header("Movement")]
    public Vector2 moveVector;
    public float runSpeed;
    public float walkSpeed;
    public float swimSpeed;
    public float fallSpeed;
    public float jumpSpeed;
    public Vector2 idleVector;
    public Vector2 facingDirection;

    [Header("Jump")]
    public bool isJumping;
    public float jumpDuration;
    public Vector2 jumpVector;

    [Header("Shifting")]
    public bool isShifting;

    [Header("Interacting")]
    private Vector2 _detectionPoint;

    // Data class
    public Data data;

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

    private void OnEnable()
    {
        _shiftAction.Enable();
        _shiftAction.started += HitShift;
        _shiftAction.canceled += CancelShift;
        _jumpAction.started += HitJump;
        _jumpAction.canceled += CancelJump;
    }

    private void OnDisable()
    {
        _shiftAction.started -= HitShift;
        _shiftAction.canceled -= CancelShift;
        _jumpAction.started -= HitJump;
        _jumpAction.canceled -= CancelJump;
        _shiftAction.Disable();
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

    private void Awake()
    {
        // Input
        _playerInput = GetComponent<PlayerInput>();
        _moveAction = _playerInput.actions["Move"];
        _shiftAction = _playerInput.actions["Shift"];
        _jumpAction = _playerInput.actions["Jump"];

        // Collisions
        interactionCollider = transform.Find("InteractionCollider").GetComponent<BoxCollider2D>();
        _feet = transform.Find("Feet");
        // Vector default for when there is no player movement input
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
        Jump = gameObject.AddComponent<Jump>();
        Swim = gameObject.AddComponent<Swim>();
        Fall = gameObject.AddComponent<Fall>();

        // Get components
        rb = gameObject.GetComponent<Rigidbody2D>();
        col = gameObject.GetComponent<BoxCollider2D>();
        animationState = gameObject.GetComponentInChildren<AnimationState>();
        tileDetector = gameObject.GetComponentInChildren<TileDetector>();

        // Init Height
        z = GetCurrentZ();

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
            // TODO: rename this more aptly
            _detectionPoint = new Vector2(transform.position.x, transform.position.y) + facingDirection;
            interactionCollider.transform.position = _detectionPoint;
            _adjacentTileDetectionPoint = new Vector2(_feet.position.x, _feet.position.y) + (facingDirection * .6f);
        }

        SetFacingDirection();
        EnableMovement();
        SetCurrentZ();

        currentState.UpdateState(this);
    }

    private void SetCurrentZ()
    {
        if (currentState == Walk || currentState == Run || currentState == Idle || currentState == Swim)
        {
            if (GetCurrentZ() < z)
            {
                z = GetCurrentZ();
            }
        }
    }

    private void FixedUpdate()
    {
        currentState.FixedUpdateState(this);
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
            // should stay on latest facing
            facingDirection = moveVector;
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

    // TODO: dictionary or a better structure for this
    // TODO: we could use facing direction here i believe instead of move vector
    public string GetTileKeyFromFacingDirection()
    {
        if (facingDirection == Vector2.right)
        {
            return "right";
        }
        else if (facingDirection == Vector2.left)
        {
            return "left";
        }
        else if (facingDirection == Vector2.up)
        {
            return "up";
        }
        else if (facingDirection == Vector2.down)
        {
            return "down";
        }
        else if (facingDirection.x > 0 && moveVector.y > 0)
        {
            return "up-right";
        }
        else if (facingDirection.x > 0 && moveVector.y < 0)
        {
            return "down-right";
        }
        else if (facingDirection.x < 0 && moveVector.y > 0)
        {
            return "up-left";
        }
        else //if (facingDirection.x < 0 && moveVector.y < 0)
        {
            return "down-left";
        }
    }

    //private void HandleDiagonalKeys(string tileKey)
    //{
    //    Debug.Log($"diagonal key! {tileKey}");
    //    // check both dirs of the composite key
    //    string[] dirs = tileKey.Split('-');

    //    int i;
    //    for (i = 0; i < dirs.Length; i++)
    //    {
    //        if (tileDetector.GetTileProp(dirs[i], "height_value") != null)
    //        {
    //            // Get tile height from tile key
    //            int? height = tileDetector.GetTileProp(dirs[i], "height_value").m_Value.ToInt();
    //            if (height > z)
    //            {
    //                Debug.Log("high tile diagonal break");
    //                canMove = false;
    //                break;
    //            }
    //        }
    //        else
    //        {
    //            Debug.Log("null tile diagonal break");
    //            canMove = false;
    //            break;
    //        }
    //    }
    //}

    // FIXME: should change to DetectNext and DetectCurrent.

    // TODO: if a "wall" terrain is next, then stop movement.

    private void DetectNextTile()
    {
        SuperTile heightTile = tileDetector.GetHeightTile(_adjacentTileDetectionPoint);
        CustomProperty heightProp = tileDetector.GetTilePropFromSuperTile(heightTile, "height_value");

        SuperTile terrainTile = tileDetector.GetTerrainTile(_adjacentTileDetectionPoint);
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
            Debug.Log("height prop is null");
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

        //if (heightTile != null)
        //{
        //    if (heightProp.m_Value.ToInt() <= z)
        //    {
        //        canMove = true;
        //    }
        //}
    }

    //private void DetectWalkableTiles()
    //{
    //    SuperTile tile = tileDetector.GetHeightTile(_adjacentTileDetectionPoint);
    //    CustomProperty heightProp = tileDetector.GetTilePropFromSuperTile(tile, "height_value");

    //    // All walkable tiles should have a height_value property.
    //    if (heightProp != null)
    //    {
    //        if (heightProp.m_Value.ToInt() > z && !canSwim)
    //        {
    //            Debug.Log("yup");
    //            canMove = false;
    //        }
    //        // Creating an invisible collision when faced with a ledge.
    //        else if (heightProp.m_Value.ToInt() < z)
    //        {
    //            // TODO: or Leap
    //            if (currentState == Jump)
    //            {
    //                canMove = true;
    //            }
    //            else
    //            {
    //                canMove = false;
    //            }
    //        }
    //        else
    //        {
    //            canMove = true;
    //        }
    //    }
    //    else
    //    {
    //        canMove = false;
    //    }
    //}

    //private void DetectSwimmableTiles()
    //{
    //    SuperTile tile = tileDetector.GetTerrainTile(_feet.transform.position);
    //    CustomProperty terrainProp = tileDetector.GetTilePropFromSuperTile(tile, "terrain");

    //    if (terrainProp != null)
    //    {
    //        if (terrainProp.m_Value == "water")
    //        {
    //            canSwim = true;
    //        }
    //        else
    //        {
    //            canSwim = false;
    //        }
    //    }
    //    else
    //    {
    //        canSwim = false;
    //    }
    //}

    private void EnableMovement()
    {
        //DetectWalkableTiles();
        //DetectSwimmableTiles();
        DetectCurrentTile();
        DetectNextTile();
    }

    private void OnDrawGizmos()
    {
        // Display the explosion radius when selected
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(_detectionPoint, .5f);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(_adjacentTileDetectionPoint, 0.5f);
    }
}
