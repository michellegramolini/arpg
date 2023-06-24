using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enemy;

public class EnemyController : MonoBehaviour, IEnemy
{
    // Debug
    private SpriteRenderer _sr;

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

    //[Header("Animation")]
    //public AnimationState animationState;

    // Start is called before the first frame update
    void Start()
    {
        // Debug
        _sr = gameObject.GetComponent<SpriteRenderer>();

        // Testing
        health = 5;

        // States
        Idle = gameObject.AddComponent<Enemy.Idle>();
        Attack = gameObject.AddComponent<Attack>();
        Patrol = gameObject.AddComponent<Patrol>();
        Damaged = gameObject.AddComponent<Enemy.Damaged>();
        Dead = gameObject.AddComponent<Enemy.Dead>();

        // Init State
        currentState = Idle;
        currentState.StartState(this);

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

    public void Hit()
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
        _sr.color = Color.red;
        yield return new WaitForSeconds(0.2f);
        _sr.color = Color.blue;
    }
    #endregion
}
