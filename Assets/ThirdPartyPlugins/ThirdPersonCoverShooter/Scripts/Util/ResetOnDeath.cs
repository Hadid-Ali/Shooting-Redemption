using System;
using System.Collections;
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
            GameEvents.GamePlayEvents.GameOver.Register(OnDead);
            
        }

        private void OnDestroy()
        {
            GameEvents.GamePlayEvents.GameOver.Unregister(OnDead);
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
            
            //SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
        
        private void OnValidate()
        {
            Delay = Mathf.Max(0, Delay);
        }
    }
}