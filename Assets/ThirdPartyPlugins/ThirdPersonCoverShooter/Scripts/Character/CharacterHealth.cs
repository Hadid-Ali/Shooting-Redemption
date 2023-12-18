using System;
using System.Collections.Generic;
using CoverShooter.AI;
using UnityEngine;
using UnityEngine.UI;

namespace CoverShooter
{
    /// <summary>
    /// Manages health and sets Is Alive in Character Motor to false when it reaches 0. Registers damage done by bullets.
    /// Multiple hitboxes can be setup inside the character. On setup Character Health will stop registering hits and instead will expect child Body Part Health components to pass taken damage to it.
    /// </summary>
    public class CharacterHealth : MonoBehaviour, ICharacterHealthListener
    {

        [HideInInspector] public Animator Animator;
        private GameEvent<CharacterHealth> m_OnDie = new();
        
        // [SerializeField] private Image _hpUI;
        /// <summary>
        /// Current health of the character.
        /// </summary>
        [Tooltip("Current health of the character.")]
        public float Health = 100f;

        /// <summary>
        /// Max health to regenerate to.
        /// </summary>
        [Tooltip("Max health to regenerate to.")]
        public float MaxHealth = 100f;

        /// <summary>
        /// Amount of health regenerated per second.
        /// </summary>
        [Tooltip("Amount of health regenerated per second.")]
        public float Regeneration = 0f;

        /// <summary>
        /// How much the incoming damage is multiplied by.
        /// </summary>
        [Tooltip("How much the incoming damage is multiplied by.")]
        public float DamageMultiplier = 1;

        /// <summary>
        /// Does the component reduce damage on hits. Usually used for debugging purposes to make immortal characters.
        /// </summary>
        [Tooltip(
            "Does the component reduce damage on hits. Usually used for debugging purposes to make immortal characters.")]
        public bool IsTakingDamage = true;

        /// <summary>
        /// Are bullet hits done to the main collider registered as damage.
        /// </summary>
        [Tooltip("Are bullet hits done to the main collider registered as damage.")]
        public bool IsRegisteringHits = true;

        /// <summary>
        /// Executed on death.
        /// </summary>
        public Action Died;

        /// <summary>
        /// Executed after being hurt.
        /// </summary>
        public Action<float> Hurt;
        public Action<float> customHurt;

        /// <summary>
        /// Executed after being healed by any amount.
        /// </summary>
        public Action<float> Healed;

        /// <summary>
        /// Executed after any health change.
        /// </summary>
        public Action<float> Changed;
        
        public Action OnHit;

        private CharacterMotor _motor;

        private bool _isDead;
        private float _previousHealth;


        public bool isDead;

        public bool isMainPlayer = false;
        public bool isNPC = false;

        private static Dictionary<GameObject, CharacterHealth> _map = new Dictionary<GameObject, CharacterHealth>();

        public static GameEvent<float> OnHealthUIUpdate = new();

        public static CharacterHealth Get(GameObject gameObject)
        {
            if (_map.ContainsKey(gameObject))
                return _map[gameObject];
            else
                return null;
        }

        public static bool Contains(GameObject gameObject)
        {
            return _map.ContainsKey(gameObject);
        }

        private void Awake()
        {
            Animator = GetComponent<Animator>();

            _previousHealth = Health;
            _motor = GetComponent<CharacterMotor>();

            if (isMainPlayer)
            {
                PlayerInputt.OnZoom += OnZoom;
                PlayerInputt.OnUnZoom += OnUnZoom;
            }

        }

        private void OnDestroy()
        {
            if (isMainPlayer)
            {
                PlayerInputt.OnZoom -= OnZoom;
                PlayerInputt.OnUnZoom -= OnUnZoom;
            }
        }

        private void OnZoom()
        {
            IsTakingDamage = true;
        }

        private void OnUnZoom()
        {
            IsTakingDamage = false;
        }

        private void OnEnable()
        {
            _map[gameObject] = this;
        }

        private void OnDisable()
        {
            _map.Remove(gameObject);
        }

        private void OnValidate()
        {
            Health = Mathf.Max(0, Health);
            MaxHealth = Mathf.Max(0, MaxHealth);
        }

        private void LateUpdate()
        {
            if (!_isDead)
            {
                Health = Mathf.Clamp(Health + Regeneration * Time.deltaTime, 0, MaxHealth);
                if (isMainPlayer)
                {
                    float percentage = (Health / MaxHealth) * 100;
                    float hp = percentage / 100;
                    OnHealthUIUpdate.Raise(hp);
                }
                check();
            }
        }

        /// <summary>
        /// Catch the resurrection event and remember that the character is alive.
        /// </summary>
        public void OnResurrect()
        {
            _isDead = false;

            if (Health <= float.Epsilon)
                Health = 0.001f;
        }

        public void Initialize(Action<CharacterHealth> onactionDie)
        {
            m_OnDie.Register(onactionDie);
        }
        public void UnInitialize()
        {
            m_OnDie.UnRegisterAll();
        }

        /// <summary>
        /// Catch the death event, set health to zero and remember that the character is dead now.
        /// </summary>
        public void OnDead()
        {
            var wasOff = _isDead;
            _isDead = true;
            Health = 0;

            //  _hpUI.fillAmount = 0;
            if (!wasOff)
                if (!isMainPlayer)
                    m_OnDie.Raise(this);
                    
                

            if (!wasOff && Died != null)
            {
                Died();
                if (isMainPlayer)
                {
                    GameAdEvents.GamestateEvents.GameLost.Raise();
                    CharacterStates.gameState = GameStates.GameOver;
                }
            }
        }

        /// <summary>
        /// Reduce health on bullet hit.
        /// </summary>
        public void OnTakenHit(Hit hit)
        {
            if (isMainPlayer)
            {
                if (CharacterStates.playerState == PlayerCustomStates.InZoom)
                {
                    OnHit?.Invoke();
                    Deal(hit.Damage);
                    GameEvents.GamePlayEvents.OnPlayerHit.Raise();
                }
            }
            else
            {
                //  _hpUI.fillAmount = Health / 100;
                Deal(hit.Damage);
            }
        }


        /// <summary>
        /// Heals by some amount.
        /// </summary>
        public void Heal(float amount)
        {
            Health = Mathf.Clamp(Health + amount, 0, MaxHealth);

            if (Health > float.Epsilon && _motor != null && !_motor.IsAlive)
                _motor.Resurrect();

            if (!_isDead)
                check();
        }

        /// <summary>
        /// Deals a specific amount of damage.
        /// </summary>
        public void Deal(float amount)
        {
            if (Health <= 0 || !IsTakingDamage)
                return;

            amount *= DamageMultiplier;

            Health = Mathf.Clamp(Health - amount, 0, MaxHealth);
            check();
            
            if(customHurt != null) customHurt(Health);

            if (Health <= 0 && _motor != null)
                _motor.Die();
        }

        private void check()
        {
            if (_previousHealth != Health)
            {
                _previousHealth = Health;
                if (Changed != null) Changed(Health);
                if (_previousHealth < Health && Healed != null) Healed(Health);
                if (_previousHealth > Health && Hurt != null) Hurt(Health);
            }
        }
    }
}