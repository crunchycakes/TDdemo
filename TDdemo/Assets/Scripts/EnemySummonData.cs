using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New EnemySummonData", menuName = "Create Enemy Summon Data")]
public class EnemySummonData : ScriptableObject
{
    public GameObject enemyPrefab;
    public int enemyID;
}
