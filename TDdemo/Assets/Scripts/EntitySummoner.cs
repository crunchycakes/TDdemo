using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntitySummoner : MonoBehaviour
{

    public static List<TestAgent> enemiesInGame;
    public static Dictionary<int, GameObject> enemyPrefabs;
    public static Dictionary<int, Queue<TestAgent>> enemyObjectPools;

    private static bool IsInit;

    // Start is called before the first frame update
    public static void Init()
    {
        if (!IsInit)
        {
            enemyPrefabs = new Dictionary<int, GameObject>();
            enemyObjectPools = new Dictionary<int, Queue<TestAgent>>();
            enemiesInGame = new List<TestAgent>();

            EnemySummonData[] enemies = Resources.LoadAll<EnemySummonData>("Enemies");

            foreach (EnemySummonData enemy in enemies)
            {
                enemyPrefabs.Add(enemy.enemyID, enemy.enemyPrefab);
                enemyObjectPools.Add(enemy.enemyID, new Queue<TestAgent>());
            }

            IsInit = true;

        } else
        {
            Debug.Log("The following class is already init: ");
        }

    }

    public static TestAgent SummonEnemy(int enemyID)
    {
        TestAgent summonedEnemy = null;

        if (enemyPrefabs.ContainsKey(enemyID))
        {
            Queue<TestAgent> referencedQueue = enemyObjectPools[enemyID];

            if (referencedQueue.Count > 0)
            {
                summonedEnemy = referencedQueue.Dequeue();
                summonedEnemy.Init();
            } else
            {
                GameObject newEnemy = Instantiate(enemyPrefabs[enemyID], Vector3.zero, Quaternion.identity);
                summonedEnemy = newEnemy.GetComponent<TestAgent>();
                summonedEnemy.Init();
            }

        } else
        {
            Debug.Log($"Enemy with ID: {enemyID} does not exist");
            return null;
        }

        return summonedEnemy;
    }

}
