using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PC : MonoBehaviour
{
    public int health = 5;
    public int attack = 2;

    // Use this for initialization
    void Start()
    {

    }

    public void addHealth(int i)
    {
        health += i;
    }

    public string healthState()
    {
        if (health > 0)
        {
            return health.ToString();
        }
        return "Dead";
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
