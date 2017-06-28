using System.Collections;
using System.Collections.Generic;
using Assets.Scripts;
using UnityEngine;
using UnityEngine.UI;

public class PC : MonoBehaviour
{
    public int health = 5;

    private List<Attack> attackSet = new List<Attack>();

    public KeyCode keyCode;

    private Text damageText;
    private Text defendText;
    private Text attackText;

    private Image attackPanel;


    public GameManager gameManager;

    // Use this for initialization
    void Start()
    {
        var texts = GetComponentsInChildren<Text>();
        damageText = texts[0];
        defendText = texts[1];
        attackText = texts[2];

        attackPanel = GetComponentInChildren<Image>();

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
        if (Defending())
        {
            health += i / 2;
            damageText.text = Mathf.Abs(i / 2).ToString();
        }
        else
        {
            health += i;
            damageText.text = Mathf.Abs(i).ToString();
        }
        StartCoroutine(ClearTextAfterDelay());
    }

    private bool Defending()
    {
        if (gameManager.attackOrder.Contains(this))
        {
            return false;
        }
        return true;
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

    public void AttackTarget(Enemy enemy)
    {
        enemy.AddHealth(-SelectedAttack().Damage);
    }

    void OnMouseDown()
    {
        if (!gameManager.turnInProgress)
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
    }

    // Update is called once per frame
    void Update()
    {
        if (gameManager.attackOrder.Contains(this))
        {
            defendText.text = "";
            attackPanel.enabled = false;
            attackText.text = "";
        }
        else
        {
            defendText.text = "D";
            attackPanel.enabled = true;
            attackText.text = attackSet[gameManager.attackOrder.Count].Damage + " Damage";
        }

        if (Input.GetKeyDown(keyCode))
        {
            if (!gameManager.turnInProgress)
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
        }

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


