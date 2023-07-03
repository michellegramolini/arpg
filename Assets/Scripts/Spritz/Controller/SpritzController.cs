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

        GameObject _characterHolder;

        public Vector2 hitDirection;
        public float knockbackForce;

        public SpriteRenderer shadowSprite;


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
            _characterHolder = transform.Find("CharacterHolder").gameObject;
            shadowSprite = transform.Find("CharacterHolder/shadow").GetComponent<SpriteRenderer>();

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

        public void Hit(Vector2 hitDirection)
        {
            this.hitDirection = hitDirection;
            if (currentState != Damaged)
            {
                StartCoroutine(Squish(0.5f, 1.2f, 0.1f));
                SetState(Damaged);
                //StartCoroutine(HitDebug());
            }
        }

        public IEnumerator Squish(float xSquash, float ySquash, float seconds)
        {
            Vector3 originalSize = Vector3.one;
            Vector3 newSize = new(xSquash, ySquash, originalSize.z);
            float t = 0f;

            while (t <= 1.0)
            {
                t += Time.deltaTime / seconds;
                _characterHolder.transform.localScale = Vector3.Lerp(originalSize, newSize, t);
                yield return null;
            }

            t = 0f;

            while (t <= 1.0)
            {
                t += Time.deltaTime / seconds;
                _characterHolder.transform.localScale = Vector3.Lerp(newSize, originalSize, t);
                yield return null;
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
