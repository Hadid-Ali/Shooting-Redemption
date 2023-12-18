using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CoverShooter
{
    /// <summary>
    /// Reset the level on character's death.
    /// </summary>
    public class ResetOnDeath : MonoBehaviour, ICharacterHealthListener
    {
        private Animator _animator;
        [Tooltip("Time in seconds to reset the level after character's death")]
        public float Delay = 2.0f;

        private static readonly int Dead = Animator.StringToHash("Dead");


        private void Awake()
        {
            _animator = GetComponent<Animator>();
            GameEvents.GamePlayEvents.TimeOver.Register(OnDead);
            GameEvents.GamePlayEvents.OnNPCKilled.Register(OnNPCKilled);
            
        }

        private void OnDestroy()
        {
            GameEvents.GamePlayEvents.TimeOver.Unregister(OnDead);
            GameEvents.GamePlayEvents.OnNPCKilled.Unregister(OnNPCKilled);
        }

        public void OnNPCKilled()
        {
            PlayerInputt.OnUnZoom();
            PlayerInputt.CanTakeInput = false;
            CustomCameraController.CameraStateChanged.Invoke(CamState.Follow);
            StartCoroutine(Wait());

        }

        IEnumerator Wait()
        {
            yield return new WaitForSeconds(2);
            GameEvents.GamePlayEvents.OnPlayerDead.Raise();
        }

        public void OnDead()
        {
            StartCoroutine(delayedReset());
        }

        public void OnResurrect() { }

        private IEnumerator delayedReset()
        {
            _animator.SetTrigger(Dead);
            PlayerInputt.OnUnZoom();
            PlayerInputt.CanTakeInput = false;
            CustomCameraController.CameraStateChanged.Invoke(CamState.Follow);
            
            yield return new WaitForSeconds(Delay);
            GameEvents.GamePlayEvents.OnPlayerDead.Raise();

        }
        
        private void OnValidate()
        {
            Delay = Mathf.Max(0, Delay);
        }
    }
}