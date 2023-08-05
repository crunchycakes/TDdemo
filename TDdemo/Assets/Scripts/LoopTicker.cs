using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoopTicker : MonoBehaviour
{

    private static Queue<int> enemyIDsToSummon;
    private static Queue<int> enemiesToRemove;
    public bool ContinueLoop;

    // Start is called before the first frame update
    void Start()
    {
        enemyIDsToSummon = new Queue<int>();
        EntitySummoner.Init();

        StartCoroutine(GameLoop());
        InvokeRepeating("summontest", 0f, 3f);
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
                    Debug.Log("spawning testagent");
                    EntitySummoner.SummonEnemy(enemyIDsToSummon.Dequeue());
                }
            }

            // spawn tower

            // move enemy

            // tick tower

            // apply effect

            // damage enemy

            // remove enemy

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

    }
}
