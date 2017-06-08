using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public PC playerOne;
    public PC playerTwo;
    public Enemy enemy;

    public Text text;

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
        text.text = "Enemy: " + enemyInstance.healthState() + " 1: " + playerOneInstance.healthState() + " 2: " +
                    playerTwoInstance.healthState();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            playerOneAttacking = true;
        }

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            playerTwoAttacking = true;
        }

        if (Input.GetKeyDown(KeyCode.Return))
        {
            enemyAttacking = Random.Range(1, 101) > 25;
            StartCoroutine(InitiateAttack());
        }

    }

    IEnumerator InitiateAttack()
    {
        if (playerOneAttacking)
        {
            playerOneInstance.transform.Translate(Vector3.left);
            if (Random.Range(1, 101) > 5)
            {
                if (enemyAttacking)
                    enemyInstance.addHealth(-playerOneInstance.attack);
                else enemyInstance.addHealth(-playerOneInstance.attack / 2);
            }
            UpdateText();
            yield return new WaitForSeconds(attackDelay);
            playerOneInstance.transform.Translate(Vector3.right);

        }
        if (playerTwoAttacking)
        {
            playerTwoInstance.transform.Translate(Vector3.left);
            if (Random.Range(1, 101) > 5)
            {
                if (enemyAttacking)
                    enemyInstance.addHealth(-playerTwoInstance.attack);
                else enemyInstance.addHealth(-playerTwoInstance.attack / 2);
            }
            UpdateText();
            yield return new WaitForSeconds(attackDelay);
            playerTwoInstance.transform.Translate(Vector3.right);
        }
        if (enemyAttacking)
        {
            enemyInstance.transform.Translate(Vector3.right);
            if (Random.Range(1, 101) > 5)
            {
                if (Random.Range(1, 3) == 1)
                {
                    playerOneInstance.addHealth(-enemyInstance.attack);
                }
                else playerTwoInstance.addHealth(-enemyInstance.attack);
            }
            UpdateText();
            yield return new WaitForSeconds(attackDelay);
            enemyInstance.transform.Translate(Vector3.left);
        }
        playerOneAttacking = false;
        playerTwoAttacking = false;
        enemyAttacking = false;

    }
}
