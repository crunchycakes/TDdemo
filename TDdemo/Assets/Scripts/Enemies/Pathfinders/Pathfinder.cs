using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Pathfinder : MonoBehaviour
{

    public abstract void Init();

    public abstract Vector3[] Pathfind(Vector3 worldStart, Vector3 worldEnd);

}
