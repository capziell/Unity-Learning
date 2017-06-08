using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    public int health = 8;
    public int attack = 3;

    // Use this for initialization
    void Start()
    {

    }

    public string healthState()
    {
        if (health > 0)
        {
            return health.ToString();
        }
        return "Dead";
    }

    public void addHealth(int i)
    {
        health += i;
    }
    // Update is called once per frame
    void Update()
    {
        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }
}
