using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public PC player;
    public Enemy enemy;

    public Text HealthText;

    public Text Attack1Text;
    public Text Attack2Text;
    public Text Attack3Text;
    public Text Attack4Text;

    private List<Text> attackTexts = new List<Text>();

    public List<PC> attackOrder = new List<PC>();

    private float attackDelay = 0.3f;

    private bool enemyAttacking;

    private List<PC> playerCharacters = new List<PC>();

    private Enemy enemyInstance;

    private int playersAlive;

    // Use this for initialization
    void Start()
    {
        attackTexts.Add(Attack1Text);
        attackTexts.Add(Attack2Text);
        attackTexts.Add(Attack3Text);
        attackTexts.Add(Attack4Text);

        playerCharacters.Add(Instantiate(player, new Vector3(1f, 0f), Quaternion.identity));
        playerCharacters[0].keyCode = KeyCode.LeftArrow;
        playerCharacters[0].gameManager = this;
        playerCharacters[0].GetComponent<SpriteRenderer>().color = Color.blue;
        playerCharacters.Add(Instantiate(player, new Vector3(3f, 0f), Quaternion.identity));
        playerCharacters[1].keyCode = KeyCode.RightArrow;
        playerCharacters[1].gameManager = this;
        playerCharacters[1].GetComponent<SpriteRenderer>().color = Color.red;
        playerCharacters.Add(Instantiate(player, new Vector3(2f, 1f), Quaternion.identity));
        playerCharacters[2].keyCode = KeyCode.UpArrow;
        playerCharacters[2].gameManager = this;
        playerCharacters[2].GetComponent<SpriteRenderer>().color = Color.yellow;
        playerCharacters.Add(Instantiate(player, new Vector3(2f, -1f), Quaternion.identity));
        playerCharacters[3].keyCode = KeyCode.DownArrow;
        playerCharacters[3].gameManager = this;
        playerCharacters[3].GetComponent<SpriteRenderer>().color = Color.green;
        enemyInstance = Instantiate(enemy, new Vector3(-1f, 0f), Quaternion.identity);

        UpdateText();
    }

    private void UpdateText()
    {
        HealthText.text = "Enemy: " + enemyInstance.HealthState();
        foreach (PC c in playerCharacters)
        {
            HealthText.text += " " + (playerCharacters.IndexOf(c) + 1) + ": " + c.HealthState();
        }

        int currentIndex = 0;

        foreach (PC c in playerCharacters)
        {
            if (attackOrder.Contains(c))
            {
                int index = attackOrder.IndexOf(c);
                attackTexts[index].color = c.GetComponent<SpriteRenderer>().color;
                attackTexts[index].text = c.SelectedAttack().Name;
                currentIndex++;
            }
        }

        foreach (PC c in playerCharacters)
        {
            if (!attackOrder.Contains(c))
            {
                attackTexts[currentIndex].color = c.GetComponent<SpriteRenderer>().color;
                attackTexts[currentIndex].text = c.health > 0 ? "Defending" : "Dead";
                currentIndex++;
            }
        }

        if (enemyInstance.health <= 0)
        {
            HealthText.text = "Battle Won";
            StartCoroutine(StartNewBattleDelayed(2f));
        }

        playersAlive = 0;

        foreach (PC c in playerCharacters)
        {
            if (c.health > 0) playersAlive++;
        }

        if (playersAlive == 0)
        {
            HealthText.text = "Game Over";
            StartCoroutine(StartNewBattleDelayed(2f));
        }
    }

    IEnumerator StartNewBattleDelayed(float s)
    {
        yield return new WaitForSeconds(s);
        SceneManager.LoadScene(0);
    }

    // Update is called once per frame
    void Update()
    {
        UpdateText();
        if (enemyInstance.health > 0 && playersAlive > 0)
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                enemyAttacking = Random.Range(1, 101) > 25;
                StartCoroutine(InitiateAttack());
            }
        }
    }

    IEnumerator InitiateAttack()
    {
        foreach (PC c in attackOrder)
        {
            c.transform.Translate(Vector3.left);
            if (enemyAttacking)
            {
                enemyInstance.AddHealth(-c.SelectedAttack().Damage);
            }
            else enemyInstance.AddHealth(-c.SelectedAttack().Damage / 2);
            if (enemyInstance.health <= 0)
            {
                c.transform.Translate(Vector3.right);
                yield break;
            }
            yield return new WaitForSeconds(attackDelay);
            c.transform.Translate(Vector3.right);
        }
        if (enemyAttacking)
        {
            enemyInstance.transform.Translate(Vector3.right);
            int playerHealth = -1;
            PC attackTarget = null;
            while (playerHealth <= 0)
            {
                attackTarget = playerCharacters[Random.Range(0, 4)];
                playerHealth = attackTarget.health;
            }
            if (attackTarget == null) yield break;
            if (attackOrder.Contains(attackTarget)) attackTarget.AddHealth(-enemyInstance.attack);
            else attackTarget.AddHealth(-enemyInstance.attack / 2);
            yield return new WaitForSeconds(attackDelay);
            enemyInstance.transform.Translate(Vector3.left);
        }
        enemyAttacking = false;
        attackOrder.Clear();

    }
}
