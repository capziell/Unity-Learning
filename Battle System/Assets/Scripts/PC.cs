using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PC : MonoBehaviour
{
    public int health = 5;
    public int attack = 2;

    private List<string> attackSet = new List<string>();

    private Text damageText;

    // Use this for initialization
    void Start()
    {
        damageText = GetComponentInChildren<Text>();
        ClearText();
        PopulateAttackSets();
    }

    private void PopulateAttackSets()
    {
            for(int i = 0; i < 2; i++)
            {
                int n = Random.Range(1, 5);
                switch (n)
                {
                    case 1:
                        attackSet.Add(" Normal Attack ");
                        break;
                    case 2:
                        attackSet.Add(" Power Attack ");
                        break;
                    case 3:
                        attackSet.Add(" Double Attack ");
                        break;
                    case 4:
                        attackSet.Add(" Triple Attack ");
                        break;                           
                }
            }
    }

    public string GetAttackName(int i)
    {
        return attackSet[i];
    }

    public void AddHealth(int i)
    {
        health += i;
        damageText.text = Mathf.Abs(i).ToString();
        StartCoroutine(ClearTextAfterDelay());
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

    public string HealthState()
    {
        if (health > 0)
        {
            return health + " HP";
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
