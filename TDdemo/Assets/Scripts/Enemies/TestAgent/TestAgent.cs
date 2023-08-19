using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestAgent : MonoBehaviour
{
    public float maxHealth;
    [HideInInspector]
    public float health;
    public float speed;
    public int ID;

    [SerializeField]
    private Slider healthSlider;

    // this being public is kinda gross
    [HideInInspector]
    public TestAgentMovementReferToGrid movementScript;

    // Start is called before the first frame update
    void Start()
    {
        Init();
    }

    public void Init()
    {
        health = maxHealth;
        healthSlider.maxValue = health;
        healthSlider.value = health;

        movementScript = gameObject.GetComponent<TestAgentMovementReferToGrid>();
        movementScript.Init();
    }
}
