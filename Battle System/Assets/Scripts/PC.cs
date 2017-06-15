using System.Collections;
using System.Collections.Generic;
using Assets.Scripts;
using UnityEngine;
using UnityEngine.UI;

public class PC : MonoBehaviour
{
    public int health = 5;
    public int attack = 2;

    private List<Attack> attackSet = new List<Attack>();

    public KeyCode keyCode;

    private Text damageText;

    public GameManager gameManager;

    // Use this for initialization
    void Start()
    {
        damageText = GetComponentInChildren<Text>();
        ClearText();
        PopulateAttackSets();
    }

    private void PopulateAttackSets()
    {
        for (int i = 0; i < 4; i++)
        {
            int n = Random.Range(1, 5);
            switch (n)
            {
                case 1:
                    attackSet.Add(new Attack("3 Damage Attack", 3));
                    break;
                case 2:
                    attackSet.Add(new Attack("5 Damage Attack", 5));

                    break;
                case 3:
                    attackSet.Add(new Attack("4 Damage Attack", 4));

                    break;
                case 4:
                    attackSet.Add(new Attack("2 Damage Attack", 2));

                    break;
            }
        }
    }

    public string GetAttackName(int i)
    {
        return attackSet[i].Name;
    }

    public void AddHealth(int i)
    {
        health += i;
        damageText.text = Mathf.Abs(i).ToString();
        StartCoroutine(ClearTextAfterDelay());
    }

    public Attack SelectedAttack()
    {
        return attackSet[gameManager.attackOrder.FindIndex(c => c == this)];
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
        if (Input.GetKeyDown(keyCode))
        {
            if (gameManager.attackOrder.Contains(this))
            {
                gameManager.attackOrder.Remove(this);
            }
            else
            {
                gameManager.attackOrder.Add(this);
            }
        }

        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }
}
