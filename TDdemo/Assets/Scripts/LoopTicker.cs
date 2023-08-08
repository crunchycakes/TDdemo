using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoopTicker : MonoBehaviour
{

    private static Queue<int> enemyIDsToSummon;
    private static Queue<TestAgent> enemiesToRemove;
    public bool ContinueLoop;

    // Start is called before the first frame update
    void Start()
    {
        enemyIDsToSummon = new Queue<int>();
        enemiesToRemove = new Queue<TestAgent>();
        EntitySummoner.Init();

        StartCoroutine(GameLoop());
        InvokeRepeating("summontest", 0f, 3f);
        ContinueLoop = true;
    }

    void summontest()
    {
        enqueueEnemyIDToSummon(1);
    }

    IEnumerator GameLoop()
    {
        while (ContinueLoop)
        {
            // spawn enemy
            if (enemyIDsToSummon.Count > 0)
            {
                for(int i = 0; i < enemyIDsToSummon.Count; i++)
                {
                    EntitySummoner.SummonEnemy(enemyIDsToSummon.Dequeue());
                }
            }

            // spawn tower

            // move enemy

            // tick tower

            // apply effect

            // damage enemy

            // remove enemy

            if (enemiesToRemove.Count > 0)
            {
                for (int i = 0; i < enemyIDsToSummon.Count; i++)
                {
                    EntitySummoner.RemoveEnemy(enemiesToRemove.Dequeue());
                }
            }

            // remove tower

            yield return null;
        }
    }

    public static void enqueueEnemyIDToSummon(int ID)
    {
        enemyIDsToSummon.Enqueue(ID);
    }

    public static void enqueueEnemyToRemove(TestAgent agent)
    {
        enemiesToRemove.Enqueue(agent);
    }

}
