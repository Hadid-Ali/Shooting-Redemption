using System.Collections;
using UnityEngine;

public class cSpeedTest : MonoBehaviour
{
   [SerializeField] private int Counterr;
    
    private bool isCounting = false;
    private int i = 0;
    private float timer = 0;

    [ContextMenu("Count")]
    void Count()
    {
        StartCoroutine(Counter());
        i = 0;
        timer = 0;
        while (i < 1000000000000000)
        {
            i++;
        }
    }

    IEnumerator Counter()
    {
        while (i < 1000000000000000)
        {
            yield return new WaitForSecondsRealtime(.01f);
            timer += .01f;
        }
        print(timer);

    }




}
