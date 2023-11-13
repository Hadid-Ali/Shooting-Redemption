using System.Collections;
using CoverShooter;
using UnityEngine;

public class EnemyPoolManager : MonoBehaviour
{
    public int time;
    public Transform spawnPosition;

    public static GameEvent<CharacterHealth> OnEnemyResurrect = new();

    public void ResurectEnemy(CharacterHealth h)
    {
        StartCoroutine(ResurrectEnemyAfterTime(h));
    }
    
    IEnumerator ResurrectEnemyAfterTime(CharacterHealth h)
    {
        yield return new WaitForSeconds(time);

        if (h != null)
        {
            var randomPosition = new Vector3(Random.Range(-5.0f, 5.0f), 0, Random.Range(-5.0f, 5.0f));
            h.transform.SetPositionAndRotation(spawnPosition.position + randomPosition, spawnPosition.rotation);
        }
        
        h.Heal(10);
    }
}
