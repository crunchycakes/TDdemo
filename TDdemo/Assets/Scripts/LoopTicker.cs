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
    private static Queue<Agent> enemiesToRemove;
    public bool ContinueLoop;

    // Start is called before the first frame update
    void Start()
    {
        enemyIDsToSummon = new Queue<int>();
        enemiesToRemove = new Queue<Agent>();
        EntitySummoner.Init();
        GridHandler.Init();

        StartCoroutine(GameLoop());
        InvokeRepeating("summontest", 0f, 0.5f);
        ContinueLoop = true;
    }

    void summontest()
    {
        EnqueueEnemyIDToSummon(1);
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
            // not sure if checking in job is faster than checking in main thread
            NativeArray<bool> shouldUpdateNode = new NativeArray<bool>(EntitySummoner.enemiesInGame.Count, Allocator.TempJob);
            NativeArray<float> enemySpeeds = new NativeArray<float>(EntitySummoner.enemiesInGame.Count, Allocator.TempJob);
            TransformAccessArray enemyAccess = new TransformAccessArray(EntitySummoner.enemiesInGameTransform.ToArray(), 
                2); // 2 threads

            for (int i = 0; i < EntitySummoner.enemiesInGame.Count; i++)
            {
                currentNodes[i] = EntitySummoner.enemiesInGame[i].movementScript.PathPoint;
                enemySpeeds[i] = EntitySummoner.enemiesInGame[i].Speed;
                shouldUpdateNode[i] = false;
            }

            MoveEnemiesJob moveJob = new MoveEnemiesJob
            {
                currentNodeList = currentNodes,
                enemySpeed = enemySpeeds,
                toUpdate = shouldUpdateNode,
                deltaTime = Time.deltaTime
            };

            JobHandle MoveJobHandle = moveJob.Schedule(enemyAccess);
            MoveJobHandle.Complete();

            for(int i = 0; i < EntitySummoner.enemiesInGame.Count; i++)
            {
                Agent currentAgent = EntitySummoner.enemiesInGame[i];
                if (shouldUpdateNode[i])
                {
                    Vector3 oldPathPoint = currentAgent.movementScript.PathPoint;
                    currentAgent.movementScript.PathPoint = GridHandler.NextPathPoint(currentAgent.transform.position);
                    if (oldPathPoint == currentAgent.movementScript.PathPoint)
                    {
                        EnqueueEnemyToRemove(currentAgent);
                    }
                }
            }

            currentNodes.Dispose();
            enemySpeeds.Dispose();
            enemyAccess.Dispose();
            shouldUpdateNode.Dispose();

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

    public static void EnqueueEnemyIDToSummon(int ID)
    {
        enemyIDsToSummon.Enqueue(ID);
    }

    public static void EnqueueEnemyToRemove(Agent agent)
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
    public NativeArray<bool> toUpdate;
    [NativeDisableParallelForRestriction]
    public float deltaTime;

    public void Execute(int index, TransformAccess transform)
    {

        transform.position = Vector3.MoveTowards(
            transform.position, currentNodeList[index], deltaTime * enemySpeed[index]
        );

        if (transform.position == currentNodeList[index]) { toUpdate[index] = true; }

    }
}