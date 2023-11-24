using UnityEngine;
using Hit = CoverShooter.Hit;

public class NPC : MonoBehaviour
{
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
    }

    public void OnHit(Hit hit)
    {
        isDead = true;
        OverlayGun.OnGunShoot -= OnAlert;
        
        anim.Play(Death);
        
        print("Wokring");
        
    }

    public void OnAlert()
    {
        if (!isDead && !hasScreamed)
        {
            anim.SetTrigger(Scared);
            audioSource.PlayOneShot(scream);
            hasScreamed = true;
            
        }
    }
}
