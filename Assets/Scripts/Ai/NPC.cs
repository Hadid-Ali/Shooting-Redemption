using UnityEngine;
using Hit = CoverShooter.Hit;

public class NPC : MonoBehaviour
{
    [SerializeField] private GameObject helpmeCanvas;
    
    private AudioSource audioSource;
    
    public AudioClip scream;
    public AudioClip Die;
    
    private Animator anim;
    
    private bool isDead;
    private bool hasScreamed;
    private static readonly int Scared = Animator.StringToHash("Scared");
    private static readonly int Death = Animator.StringToHash("Death");

    
    private void Awake()
    {
        anim = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        OverlayGun.OnGunShoot += OnAlert;

        isDead = false;
        hasScreamed = false;
    }

    private void OnDestroy()
    {
        OverlayGun.OnGunShoot -= OnAlert;
        if(SessionData.Instance)
        SessionData.Instance.civiliansKilled = 0;
    }

    public void OnHit(Hit hit)
    {
        if(!isDead)
            SessionData.Instance.civiliansKilled += 1;
        
        isDead = true;
        OverlayGun.OnGunShoot -= OnAlert;
        
        anim.Play(Death);
        helpmeCanvas.SetActive(false);
        GameEvents.GamePlayEvents.GameOver.Raise();
    }

    public void OnHit()
    {
        isDead = true;
        OverlayGun.OnGunShoot -= OnAlert;
        
        anim.Play(Death);
        helpmeCanvas.SetActive(false);
        SessionData.Instance.civiliansKilled += 1;
        
    }

    public void OnAlert()
    {
        if (!isDead && !hasScreamed)
        {
            anim.SetTrigger(Scared);
            audioSource.PlayOneShot(scream);
            hasScreamed = true;
            helpmeCanvas.SetActive(true);
            
        }
    }
}
