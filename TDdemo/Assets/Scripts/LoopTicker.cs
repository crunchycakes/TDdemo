using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoopTicker : MonoBehaviour
{

    private static Queue<int> enemyIDsToSummon;

    public bool ContinueLoop;

    // Start is called before the first frame update
    void Start()
    {
        enemyIDsToSummon = new Queue<int>();
        EntitySummoner.Init();

        StartCoroutine(GameLoop());
        //InvokeRepeating("summontest", 0f, 5f);
        Invoke("summontest", 1f);
    }

    void summontest()
    {
        enqueueEnemyIDToSummon(1);
    }

    IEnumerator GameLoop()
    {
        while (ContinueLoop)
        {
            // tick

            if (enemyIDsToSummon.Count > 0)
            {
                for(int i = 0; i < enemyIDsToSummon.Count; i++)
                {
                    EntitySummoner.SummonEnemy(enemyIDsToSummon.Dequeue());
                }
            }

            yield return null;
        }
    }

    public static void enqueueEnemyIDToSummon(int ID)
    {
        enemyIDsToSummon.Enqueue(ID);
    }

}
