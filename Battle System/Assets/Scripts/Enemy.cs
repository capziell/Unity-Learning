using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    public GameManager GameManager;

    public int health = 8;
    public int attack = 3;

    private Text damageText;
    private Text defendText;
    
    private bool defendedLastTurn;

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

    public void AttackTarget(PC character)
    {
        if (defendedLastTurn)
        {
            character.AddHealth(-attack * 2);
        }
        else character.AddHealth(-attack);

        defendedLastTurn = false;
    }

    public void StopDefending()
    {
        defendText.text = "";
        defendedLastTurn = true;
    }

    public void AddHealth(int i)
    {
        if (!GameManager.EnemyAttacking())
        {
            damageText.text = Mathf.Abs(i / 2).ToString();
            health += i / 2;
        }
        else
        {
            health += i;
            damageText.text = Mathf.Abs(i).ToString();
        }
        StartCoroutine(ClearTextAfterDelay());
    }
    // Update is called once per frame
    void Update()
    {
        if (health <= 0)
        {
            StartCoroutine(Die());
        }
    }

    private IEnumerator Die()
    {
        defendText.text = "Dead";
        yield return new WaitForSeconds(1f);
        gameObject.SetActive(false);
    }
}
