using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntitySummoner : MonoBehaviour
{

    public static List<Agent> enemiesInGame;
    public static List<Transform> enemiesInGameTransform;
    public static Dictionary<int, GameObject> enemyPrefabs;
    public static Dictionary<int, Queue<Agent>> enemyObjectPools;

    private static bool IsInit;

    // Start is called before the first frame update
    public static void Init()
    {
        if (!IsInit)
        {
            enemyPrefabs = new Dictionary<int, GameObject>();
            enemyObjectPools = new Dictionary<int, Queue<Agent>>();
            enemiesInGame = new List<Agent>();
            enemiesInGameTransform = new List<Transform>();

            EnemySummonData[] enemies = Resources.LoadAll<EnemySummonData>("Enemies");

            foreach (EnemySummonData enemy in enemies)
            {
                enemyPrefabs.Add(enemy.enemyID, enemy.enemyPrefab);
                enemyObjectPools.Add(enemy.enemyID, new Queue<Agent>());
            }

            IsInit = true;

        } else
        {
            Debug.Log("EntitySummoner is already init");
        }

    }

    public static Agent SummonEnemy(int enemyID)
    {
        Agent summonedEnemy;

        if (enemyPrefabs.ContainsKey(enemyID))
        {
            Queue<Agent> referencedQueue = enemyObjectPools[enemyID];

            if (referencedQueue.Count > 0)
            {
                summonedEnemy = referencedQueue.Dequeue();
                summonedEnemy.transform.position = new Vector3(-3.5f, 1.5f);
                summonedEnemy.Init();

                summonedEnemy.gameObject.SetActive(true);
            } else
            {
                GameObject newEnemy = Instantiate(enemyPrefabs[enemyID], new Vector3(-3.5f, 1.5f), Quaternion.identity);
                summonedEnemy = newEnemy.GetComponent<Agent>();
                summonedEnemy.Init();
            }

        } else
        {
            Debug.Log($"Enemy with ID: {enemyID} does not exist");
            return null;
        }

        enemiesInGameTransform.Add(summonedEnemy.transform);
        enemiesInGame.Add(summonedEnemy);
        summonedEnemy.ID = enemyID;
        return summonedEnemy;
    }

    public static void RemoveEnemy(Agent agent)
    {
        enemyObjectPools[agent.ID].Enqueue(agent);
        agent.gameObject.SetActive(false);
        enemiesInGame.Remove(agent);
        enemiesInGameTransform.Remove(agent.transform);
        // pooling; don't make gc run
    }

}
