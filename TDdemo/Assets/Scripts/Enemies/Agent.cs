using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Agent : MonoBehaviour
{
    [SerializeField]
    private float maxHealth;
    public float MaxHealth
    {
        get { return maxHealth; }
        set { maxHealth = value; }
    }

    private float health;
    public float Health
    {
        get { return health; }
        set { if (value >= 0 && value <= maxHealth) health = value; }
    }

    [SerializeField]
    private float speed;
    public float Speed
    {
        get { return speed; }
        set { if (speed >= 0) speed = value; }
    }

    private int id;
    public int ID
    {
        get { return id; }
        set { id = value; }
    }

    [SerializeField]
    private Slider healthSlider;

    // this being public is kinda gross
    [HideInInspector]
    public AgentMovement movementScript;

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

        movementScript = gameObject.GetComponent<AgentMovement>();
        movementScript.Init();
    }
}
