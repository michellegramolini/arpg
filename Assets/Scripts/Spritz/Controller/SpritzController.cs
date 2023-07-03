using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Spritz
{
    public class SpritzController : MonoBehaviour, IEnemy
    {
        [Header("Components")]
        public SpriteRenderer sr;
        public BoxCollider2D bc;
        public Rigidbody2D rb;

        [Header("States")]
        public SpritzState currentState;
        public SpritzState previousState;
        public Attack Attack;
        public Idle Idle;
        public Damaged Damaged;
        public Dead Dead;

        [Header("Health")]
        public int health;

        [Header("XP")]
        public int xpAmount;

        [Header("Respawn")]
        public Vector3 respawnPosition;

        [Header("Animation")]
        public AnimationState animationState;

        [Header("Movement")]
        public Vector2 moveVector;
        public Vector2 facingDirection;
        public float bounceSpeed;


        // Start is called before the first frame update
        void Start()
        {
            // Components
            sr = gameObject.GetComponentInChildren<SpriteRenderer>();
            //
            bc = gameObject.GetComponentInChildren<BoxCollider2D>();
            rb = gameObject.GetComponent<Rigidbody2D>();

            // Testing
            health = 2;
            xpAmount = 2;

            // States
            Idle = gameObject.AddComponent<Idle>();
            Attack = gameObject.AddComponent<Attack>();
            Damaged = gameObject.AddComponent<Damaged>();
            Dead = gameObject.AddComponent<Dead>();

            // Respawn
            respawnPosition = transform.position;

            // Get Components
            animationState = gameObject.GetComponentInChildren<AnimationState>();
            //animationState = gameObject.GetComponent<AnimationState>();

            // Init State
            currentState = Idle;
            currentState.StartState(this);

        }

        private void Update()
        {
            SetFacingDirection();

            currentState.UpdateState(this);
        }

        private void FixedUpdate()
        {
            currentState.FixedUpdateState(this);
        }

        private void LateUpdate()
        {
            currentState.LateUpdateState(this);
        }

        public void SetState(SpritzState state)
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
            if (moveVector != Vector2.zero)
            {
                // should stay on latest facing
                facingDirection = moveVector;
            }
        }

        public void Hit()
        {
            if (currentState != Damaged)
            {
                SetState(Damaged);
                //StartCoroutine(HitDebug());
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

}
