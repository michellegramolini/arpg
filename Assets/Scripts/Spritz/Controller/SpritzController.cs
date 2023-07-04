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

        [Header("Tile Detection")]
        public TileDetector tileDetector;
        private Transform _feet;
        public int z;
        public bool canMove;
        public bool terrainInFrontOf;
        public bool heightInFrontOf;

        private void Awake()
        {
            _characterHolder = transform.Find("CharacterHolder").gameObject;
            shadowSprite = transform.Find("CharacterHolder/shadow").GetComponent<SpriteRenderer>();
            _feet = transform.Find("CharacterHolder/Feet");
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
            Attack = gameObject.AddComponent<Attack>();
            Damaged = gameObject.AddComponent<Damaged>();
            Dead = gameObject.AddComponent<Dead>();

            // Respawn
            respawnPosition = transform.position;

            // Get Components
            animationState = gameObject.GetComponentInChildren<AnimationState>();
            //_characterHolder = transform.Find("CharacterHolder").gameObject;
            //shadowSprite = transform.Find("CharacterHolder/shadow").GetComponent<SpriteRenderer>();
            tileDetector = gameObject.GetComponentInChildren<TileDetector>();
            //_feet = transform.Find("CharacterHolder/Feet");

            // Init Height
            // Only need to get this once at Start because current implementation does not allow jumping up or down things
            //z = GetCurrentZ();

            facingDirection = Vector2.down;

            // Init State
            currentState = Idle;
            currentState.StartState(this);

        }

        private void Update()
        {
            if (z != GetCurrentZ())
            {
                z = GetCurrentZ();
            }

            SetFacingDirection();
            DetectNextFacingTile(facingDirection);

            if (terrainInFrontOf || heightInFrontOf)
            {
                canMove = false;
            }
            else
            {
                canMove = true;
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

        #region Tile Detection
        private int GetCurrentZ()
        {
            if (tileDetector.GetTileProp("current", "height_value") != null)
            {
                return tileDetector.GetTileProp("current", "height_value").m_Value.ToInt();
            }

            return 0;
        }

        private void DetectNextFacingTile(Vector3 facingDirection)
        {
            Vector3 pos = _feet.position + (facingDirection);

            SuperTile _heightTile = tileDetector.GetHeightTile(pos);
            SuperTile _terrainTile = tileDetector.GetTerrainTile(pos);

            if (_terrainTile != null)
            {
                string _terrainValue = tileDetector.GetTilePropFromSuperTile(_terrainTile, "terrain").m_Value;

                if (_terrainValue == "water" || _terrainValue == "wall")
                {
                    terrainInFrontOf = true;
                }
                else
                {
                    terrainInFrontOf = false;
                }
            }
            else
            {
                terrainInFrontOf = false;
            }

            if (_heightTile != null)
            {
                int? _heightValue = tileDetector.GetTilePropFromSuperTile(_heightTile, "height_value").m_Value.ToInt();

                if (_heightValue != null)
                {
                    if (_heightValue != z)
                    {
                        heightInFrontOf = true;
                    }
                    else
                    {
                        // can go
                        heightInFrontOf = false;
                    }
                }
                else
                {
                    heightInFrontOf = true;
                }
            }
            else
            {
                // In other words, no tile available
                heightInFrontOf = true;
            }
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
                    if (_heightValue != z)
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
