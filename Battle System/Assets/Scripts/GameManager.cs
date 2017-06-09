using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public PC playerOne;
    public PC playerTwo;
    public Enemy enemy;

    public Text HealthText;
    public Text AttackText;
    
    private float attackDelay = 0.3f;

    private bool playerOneAttacking;
    private bool playerTwoAttacking;
    private bool enemyAttacking;

    private PC playerOneInstance;
    private PC playerTwoInstance;
    private Enemy enemyInstance;

    // Use this for initialization
    void Start()
    {
        playerOneInstance = Instantiate(playerOne, new Vector3(1f, 1f), Quaternion.identity);
        playerTwoInstance = Instantiate(playerTwo, new Vector3(1f, -1f), Quaternion.identity);
        enemyInstance = Instantiate(enemy, new Vector3(-1f, 0f), Quaternion.identity);

        UpdateText();
    }

    private void UpdateText()
    {
        HealthText.text = "Enemy: " + enemyInstance.HealthState() + " 1: " + playerOneInstance.HealthState() +
                          " 2: " +
                          playerTwoInstance.HealthState();
        AttackText.text = "1:";
        if (playerOneAttacking) AttackText.text += " Attack ";
        else if (playerOneInstance.health > 0) AttackText.text += " Defend ";
        else AttackText.text += " Dead ";
        AttackText.text += "2:";
        if (playerTwoAttacking) AttackText.text += " Attack ";
        else if (playerTwoInstance.health > 0) AttackText.text += " Defend ";
        else AttackText.text += " Dead ";

        if (enemyInstance.health <= 0)
        {
            AttackText.text = "";
            HealthText.text = "Battle Won";
            StartCoroutine(StartNewBattleDelayed(2f));
        }
        if (playerOneInstance.health <= 0 && playerTwoInstance.health <= 0)
        {
            AttackText.text = "";
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
        if (enemyInstance.health > 0 && (playerOneInstance.health > 0 || playerTwoInstance.health > 0))
        {
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                playerOneAttacking = !playerOneAttacking;
                UpdateText();
            }

            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                playerTwoAttacking = !playerTwoAttacking;
                UpdateText();
            }

            if (Input.GetKeyDown(KeyCode.Return))
            {
                enemyAttacking = Random.Range(1, 101) > 25;
                StartCoroutine(InitiateAttack());
            }
        }
    }

    IEnumerator InitiateAttack()
    {
        if (playerOneAttacking)
        {
            playerOneInstance.transform.Translate(Vector3.left);
            if (enemyAttacking)
                enemyInstance.AddHealth(-playerOneInstance.attack);
            else enemyInstance.AddHealth(-playerOneInstance.attack / 2);
            UpdateText();
            yield return new WaitForSeconds(attackDelay);
            playerOneInstance.transform.Translate(Vector3.right);

        }
        if (playerTwoAttacking)
        {
            playerTwoInstance.transform.Translate(Vector3.left);
            if (enemyAttacking)
                enemyInstance.AddHealth(-playerTwoInstance.attack);
            else enemyInstance.AddHealth(-playerTwoInstance.attack / 2);
            UpdateText();
            yield return new WaitForSeconds(attackDelay);
            playerTwoInstance.transform.Translate(Vector3.right);
        }
        if (enemyAttacking)
        {
            enemyInstance.transform.Translate(Vector3.right);
            if (playerOneInstance.health <= 0)
            {
                if (playerTwoAttacking) playerTwoInstance.AddHealth(-enemyInstance.attack);
                else playerTwoInstance.AddHealth(-enemyInstance.attack / 2);
                UpdateText();
                yield return new WaitForSeconds(attackDelay);
                enemyInstance.transform.Translate(Vector3.left);
                yield break;
            }
            if (playerTwoInstance.health <= 0)
            {
                if (playerOneAttacking) playerOneInstance.AddHealth(-enemyInstance.attack);
                else playerOneInstance.AddHealth(-enemyInstance.attack / 2);
                UpdateText();
                yield return new WaitForSeconds(attackDelay);
                enemyInstance.transform.Translate(Vector3.left);
                yield break;
            }
            if (Random.Range(1, 3) == 1)
            {
                if (playerOneAttacking) playerOneInstance.AddHealth(-enemyInstance.attack);
                else playerOneInstance.AddHealth(-enemyInstance.attack / 2);
            }
            else if (playerTwoAttacking) playerTwoInstance.AddHealth(-enemyInstance.attack);
            else playerTwoInstance.AddHealth(-enemyInstance.attack / 2);
            UpdateText();
            yield return new WaitForSeconds(attackDelay);
            enemyInstance.transform.Translate(Vector3.left);
        }
        playerOneAttacking = false;
        playerTwoAttacking = false;
        enemyAttacking = false;
        UpdateText();

    }
}
