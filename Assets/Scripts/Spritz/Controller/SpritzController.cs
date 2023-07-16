using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SuperTiled2Unity;

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
        public Idle Idle;
        public Damaged Damaged;
        public Dead Dead;
        public Spawn Spawn;
        public Bounce Bounce;

        [Header("Health")]
        public int health;

        [Header("XP")]
        public int xpAmount;

        [Header("Respawn")]
        public Vector3 respawnPosition;
        public bool respawned;

        [Header("Animation")]
        public AnimationState animationState;
        public SpriteRenderer shadowSprite;
        public GameObject characterHolder;

        [Header("Movement")]
        public Vector2 moveVector;
        public Vector2 facingDirection;
        public float bounceSpeed;

        [Header("Idle")]
        public List<float> idleTimeRange;

        [Header("Attack/Hit")]
        public Vector2 hitDirection;
        public float knockbackForce;
        public float knockbackDrag;

        [Header("Tile Detection")]
        public TileDetector tileDetector;
        private Transform _feet;
        public int z;
        public bool canMove;
        public bool terrainInFrontOf;
        public bool heightInFrontOf;
        public bool nullTileInFrontOf;

        [Header("Player Detection")]
        public bool playerDetected;
        public float playerDetectionRadius;
        public LayerMask playerLayer;

        private void Awake()
        {
            characterHolder = transform.Find("CharacterHolder").gameObject;
            shadowSprite = transform.Find("CharacterHolder/shadow").GetComponent<SpriteRenderer>();
            _feet = transform.Find("CharacterHolder/Feet");
            playerLayer = LayerMask.GetMask("Player");
        }

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
            Damaged = gameObject.AddComponent<Damaged>();
            Dead = gameObject.AddComponent<Dead>();
            Spawn = gameObject.AddComponent<Spawn>();
            Bounce = gameObject.AddComponent<Bounce>();

            // Respawn
            respawnPosition = transform.position;

            // Get Components
            animationState = gameObject.GetComponentInChildren<AnimationState>();
            tileDetector = gameObject.GetComponentInChildren<TileDetector>();

            facingDirection = Vector2.down;
            moveVector = facingDirection;

            // Init State
            currentState = Spawn;
            currentState.StartState(this);

        }

        private void Update()
        {
            SetFacingDirection();

            if (currentState == Spawn)
            {
                DetectPlayer();
            }

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

        public void DisableSprites()
        {
            sr.enabled = false;
            shadowSprite.enabled = false;
        }

        public void EnabeSprites()
        {
            sr.enabled = true;
            shadowSprite.enabled = true;
        }

        private void DetectPlayer()
        {
            Collider2D hitPlayer = Physics2D.OverlapCircle(transform.position, playerDetectionRadius, playerLayer);

            if (hitPlayer != null)
            {
                playerDetected = true;

            }
            else
            {
                playerDetected = false;
            }
        }

        #region Tile Detection
        // IEnemy Interface for accessing enemy height as well. 
        public int GetCurrentZ()
        {
            if (tileDetector.GetTileProp("current", "height_value") != null)
            {
                return tileDetector.GetTileProp("current", "height_value").m_Value.ToInt();
            }

            return 0;
        }

        public bool DetectNextFacingTile(Vector3 moveVector, float buffer)
        {
            Vector3 pos = _feet.position + (moveVector * buffer);

            SuperTile _heightTile = tileDetector.GetHeightTile(pos);
            SuperTile _terrainTile = tileDetector.GetTerrainTile(pos);

            nullTileInFrontOf = terrainInFrontOf = heightInFrontOf = false;
            // if both tiles are null
            if (_terrainTile == null && _heightTile == null)
            {
                nullTileInFrontOf = true;
            }

            // if there is terrain data
            if (_terrainTile != null)
            {
                string _terrainValue = tileDetector.GetTilePropFromSuperTile(_terrainTile, "terrain").m_Value;

                if (_terrainValue == "water" || _terrainValue == "wall")
                {
                    terrainInFrontOf = true;
                }
            }

            // if there is height data
            if (_heightTile != null)
            {
                int? _heightValue = tileDetector.GetTilePropFromSuperTile(_heightTile, "height_value").m_Value.ToInt();

                if (_heightValue != null)
                {
                    // do not jump up or down from anything
                    if (_heightValue != GetCurrentZ())
                    {
                        heightInFrontOf = true;
                    }
                }
            }

            if (terrainInFrontOf || heightInFrontOf || nullTileInFrontOf)
            {
                canMove = false;
            }
            else
            {
                canMove = true;
            }

            return canMove;
        }

        public bool IsStandingOnBadTile()
        {
            SuperTile _heightTile = tileDetector.GetHeightTile(_feet.position);
            SuperTile _terrainTile = tileDetector.GetTerrainTile(_feet.position);

            bool standingOnBadTile = false;
            if (_terrainTile != null)
            {
                string _terrainValue = tileDetector.GetTilePropFromSuperTile(_terrainTile, "terrain").m_Value;
                if (_terrainValue == "water" || _terrainValue == "wall")
                {
                    standingOnBadTile = true;
                }
            }

            if (_heightTile != null)
            {
                int? _heightValue = tileDetector.GetTilePropFromSuperTile(_heightTile, "height_value").m_Value.ToInt();
                if (_heightValue != null)
                {
                    if (_heightValue != GetCurrentZ())
                    {
                        standingOnBadTile = true;
                    }
                }
                else
                {
                    standingOnBadTile = true;
                }
            }
            else
            {
                standingOnBadTile = true;
            }

            return standingOnBadTile;
        }

        #endregion

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
            if (moveVector != Vector2.zero && facingDirection != moveVector)
            {
                // should stay on latest facing
                facingDirection = moveVector;
            }
        }

        public void Hit(Vector2 hitDirection)
        {
            this.hitDirection = hitDirection;

            StopCoroutine(nameof(Squish));
            StartCoroutine(Squish(0.5f, 1.2f, 0.1f));

            if (currentState != Damaged)
            {
                SetState(Damaged);
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
                characterHolder.transform.localScale = Vector3.Lerp(originalSize, newSize, t);
                yield return null;
            }

            t = 0f;

            while (t <= 1.0)
            {
                t += Time.deltaTime / seconds;
                characterHolder.transform.localScale = Vector3.Lerp(newSize, originalSize, t);
                yield return null;
            }
        }

        #region Debug
        private IEnumerator HitDebug()
        {
            sr.color = Color.red;
            yield return new WaitForSeconds(0.2f);
            sr.color = Color.blue;
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, playerDetectionRadius);
        }
        #endregion
    }

}
