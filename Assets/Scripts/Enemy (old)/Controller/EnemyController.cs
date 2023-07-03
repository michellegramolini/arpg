using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enemy;

public class EnemyController : MonoBehaviour, IEnemy
{
    public SpriteRenderer sr;
    public BoxCollider2D bc;

    [Header("States")]
    public EnemyState currentState;
    public EnemyState previousState;
    public Attack Attack;
    public Enemy.Idle Idle;
    public Patrol Patrol;
    public Enemy.Damaged Damaged;
    public Enemy.Dead Dead;

    [Header("Health")]
    public int health;

    [Header("XP")]
    public int xpAmount;

    [Header("Respawn")]
    public Vector3 respawnPosition;

    //[Header("Animation")]
    //public AnimationState animationState;

    // Start is called before the first frame update
    void Start()
    {
        // Components
        sr = gameObject.GetComponent<SpriteRenderer>();
        bc = gameObject.GetComponent<BoxCollider2D>();

        // Testing
        health = 2;
        xpAmount = 2;

        // States
        Idle = gameObject.AddComponent<Enemy.Idle>();
        Attack = gameObject.AddComponent<Attack>();
        Patrol = gameObject.AddComponent<Patrol>();
        Damaged = gameObject.AddComponent<Enemy.Damaged>();
        Dead = gameObject.AddComponent<Enemy.Dead>();

        // Init State
        currentState = Idle;
        currentState.StartState(this);

        // Respawn
        respawnPosition = transform.position;
        // Get Components
        //animationState = gameObject.GetComponentInChildren<AnimationState>();
    }

    public void SetState(EnemyState state)
    {
        if (state == currentState)
        {
            return;
        }
        this.previousState = currentState;
        this.currentState = state;

        state.StartState(this);
    }

    // Update is called once per frame
    //void Update()
    //{

    //}

    public void Hit(Vector2 hitDirection)
    {
        if (currentState != Damaged)
        {
            SetState(Damaged);
            StartCoroutine(HitDebug());
        }
    }

    #region
    private IEnumerator HitDebug()
    {
        sr.color = Color.red;
        yield return new WaitForSeconds(0.2f);
        sr.color = Color.blue;
    }
    #endregion
}
