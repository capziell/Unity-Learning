using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{

    public int health = 8;
    public int attack = 3;

    private Text damageText;
    private Text defendText;

    // Use this for initialization
    void Start()
    {
        damageText = GetComponentsInChildren<Text>()[0];
        defendText = GetComponentsInChildren<Text>()[1];
        ClearText();
    }

    public string HealthState()
    {
        if (health > 0)
        {
            return health + " HP";
        }
        return "Dead";
    }

    private IEnumerator ClearTextAfterDelay()
    {
        yield return new WaitForSeconds(0.6f);
        ClearText();
    }

    private void ClearText()
    {
        damageText.text = "";
    }

    public void Defend()
    {
        defendText.text = "D";
    }

    public void StopDefending()
    {
        defendText.text = "";
    }

    public void AddHealth(int i)
    {
        health += i;
        damageText.text = Mathf.Abs(i).ToString();
        StartCoroutine(ClearTextAfterDelay());
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
