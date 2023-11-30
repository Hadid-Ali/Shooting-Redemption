using System.Collections;
using CoverShooter;
using UnityEngine;

public class EnemyPoolManager : MonoBehaviour
{
    [HideInInspector] public int time;
    [HideInInspector] public Transform[] spawnPosition;

    public void ResurectEnemy(CharacterHealth h)
    {
        StartCoroutine(ResurrectEnemyAfterTime(h));
    }
    
    IEnumerator ResurrectEnemyAfterTime(CharacterHealth h)
    {
        yield return new WaitForSeconds(time);

        if (h != null)
        {
            var randomPosition = new Vector3(Random.Range(-1.0f, 1.0f), 0, Random.Range(-1.0f, 1.0f));
            
            Vector3 newPosition = spawnPosition[Random.Range(0, spawnPosition.Length)].position + randomPosition;
            Quaternion newRotation = spawnPosition[Random.Range(0, spawnPosition.Length)].rotation ;
            
            h.transform.SetPositionAndRotation(newPosition,newRotation);
            h.Heal(10);
        }
    }
}
