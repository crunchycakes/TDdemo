using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;
using UnityEngine.Jobs;

public class LoopTicker : MonoBehaviour
{
    
    private static Queue<int> enemyIDsToSummon;
    private static Queue<TestAgent> enemiesToRemove;
    public bool ContinueLoop;

    private GridHandler gridHandler;

    // Start is called before the first frame update
    void Start()
    {
        enemyIDsToSummon = new Queue<int>();
        enemiesToRemove = new Queue<TestAgent>();
        EntitySummoner.Init();

        StartCoroutine(GameLoop());
        InvokeRepeating("summontest", 0f, 3f);
        ContinueLoop = true;

        gridHandler = GameObject.Find("Admin").GetComponent<GridHandler>();
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

            NativeArray<Vector3> currentNodes = new NativeArray<Vector3>(EntitySummoner.enemiesInGame.Count, Allocator.TempJob);
            NativeArray<float> enemySpeeds = new NativeArray<float>(EntitySummoner.enemiesInGame.Count, Allocator.TempJob);
            TransformAccessArray enemyAccess = new TransformAccessArray(EntitySummoner.enemiesInGameTransform.ToArray(), 
                2); // 2 threads

            for (int i = 0; i < EntitySummoner.enemiesInGame.Count; i++)
            {
                currentNodes[i] = EntitySummoner.enemiesInGame[i].movementScript.pathPoint;
                enemySpeeds[i] = EntitySummoner.enemiesInGame[i].speed;

            }

            MoveEnemiesJob moveJob = new MoveEnemiesJob
            {
                currentNodeList = currentNodes,
                enemySpeed = enemySpeeds,
                deltaTime = Time.deltaTime
            };

            JobHandle MoveJobHandle = moveJob.Schedule(enemyAccess);
            MoveJobHandle.Complete();

            for(int i = 0; i < EntitySummoner.enemiesInGame.Count; i++)
            {
                TestAgent currentAgent = EntitySummoner.enemiesInGame[i];
                if (currentAgent.transform.position == currentAgent.movementScript.pathPoint)
                {
                    Vector3 oldPathPoint = currentAgent.movementScript.pathPoint;
                    currentAgent.movementScript.pathPoint = gridHandler.nextPathPoint(currentAgent.transform.position);
                    if (oldPathPoint == currentAgent.movementScript.pathPoint)
                    {
                        enqueueEnemyToRemove(currentAgent);
                    }
                }
            }

            currentNodes.Dispose();
            enemySpeeds.Dispose();
            enemyAccess.Dispose();

            // tick tower

            // apply effect

            // damage enemy

            // remove enemy

            if (enemiesToRemove.Count > 0)
            {
                for (int i = 0; i < enemiesToRemove.Count; i++)
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

// not extensible atm; TODO: some way to allow enemies more behaviour; also a way to pause movement cleanly
public struct MoveEnemiesJob : IJobParallelForTransform
{
    [NativeDisableParallelForRestriction]
    public NativeArray<Vector3> currentNodeList;
    [NativeDisableParallelForRestriction]
    public NativeArray<float> enemySpeed;
    [NativeDisableParallelForRestriction]
    public float deltaTime;

    public void Execute(int index, TransformAccess transform)
    {

        transform.position = Vector3.MoveTowards(
            transform.position, currentNodeList[index], deltaTime * enemySpeed[index]
        );

    }
}