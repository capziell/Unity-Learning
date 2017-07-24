using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    public PC player;
    public Enemy enemy;

    public Text HealthText;

    public Image Attack1Panel;
    public Image Attack2Panel;
    public Image Attack3Panel;
    public Image Attack4Panel;

    public Text Attack1Text;
    public Text Attack2Text;
    public Text Attack3Text;
    public Text Attack4Text;

    public Sprite redpanel;
    public Sprite bluepanel;
    public Sprite greenpanel;
    public Sprite yellowpanel;

    public Button RepeatButton;
    public Button ConfirmButton;

    private bool repeatPressed;
    private bool confirmPressed;

    private List<Image> attackPanels = new List<Image>();
    private List<Text> attackTexts = new List<Text>();

    public List<PC> attackOrder = new List<PC>();
    private List<PC> lastAttackOrder = new List<PC>();

    private float attackDelay = 0.3f;

    private bool enemyAttacking;

    private List<PC> playerCharacters = new List<PC>();

    public bool turnInProgress;

    private Enemy enemyInstance;

    // Use this for initialization
    void Start()
    {
        attackPanels.Add(Attack1Panel);
        attackPanels.Add(Attack2Panel);
        attackPanels.Add(Attack3Panel);
        attackPanels.Add(Attack4Panel);

        attackTexts.Add(Attack1Text);
        attackTexts.Add(Attack2Text);
        attackTexts.Add(Attack3Text);
        attackTexts.Add(Attack4Text);

        playerCharacters.Add(Instantiate(player, new Vector3(1f, 0f), Quaternion.identity));
        playerCharacters[0].keyCode = KeyCode.LeftArrow;
        playerCharacters[0].gameManager = this;
        playerCharacters[0].GetComponent<SpriteRenderer>().color = Color.blue;
        playerCharacters[0].GetComponentInChildren<Image>().sprite = bluepanel;
        playerCharacters.Add(Instantiate(player, new Vector3(4f, 0f), Quaternion.identity));
        playerCharacters[1].keyCode = KeyCode.RightArrow;
        playerCharacters[1].gameManager = this;
        playerCharacters[1].GetComponent<SpriteRenderer>().color = Color.red;
        playerCharacters[1].GetComponentInChildren<Image>().sprite = redpanel;
        playerCharacters.Add(Instantiate(player, new Vector3(2.5f, 2f), Quaternion.identity));
        playerCharacters[2].keyCode = KeyCode.UpArrow;
        playerCharacters[2].gameManager = this;
        playerCharacters[2].GetComponent<SpriteRenderer>().color = Color.yellow;
        playerCharacters[2].GetComponentInChildren<Image>().sprite = yellowpanel;
        playerCharacters.Add(Instantiate(player, new Vector3(2.5f, -2f), Quaternion.identity));
        playerCharacters[3].keyCode = KeyCode.DownArrow;
        playerCharacters[3].gameManager = this;
        playerCharacters[3].GetComponent<SpriteRenderer>().color = Color.green;
        playerCharacters[3].GetComponentInChildren<Image>().sprite = greenpanel;

        enemyInstance = Instantiate(enemy, new Vector3(-1f, 0f), Quaternion.identity);
        enemyInstance.GameManager = this;

        RepeatButton.onClick.AddListener(RepeatClicked);
        ConfirmButton.onClick.AddListener(ConfirmClicked);

        UpdateText();
    }

    private void RepeatClicked()
    {
        repeatPressed = true;
    }

    private void ConfirmClicked()
    {
        confirmPressed = true;
    }

    private void UpdateText()
    {
        HealthText.text = "Enemy: " + enemyInstance.HealthState();
        foreach (PC c in playerCharacters)
        {
            HealthText.text += " | " + (playerCharacters.IndexOf(c) + 1) + ": " + c.HealthState();
        }

        foreach (Image panel in attackPanels)
        {
            panel.enabled = false;
        }

        foreach (Text t in attackTexts)
        {
            t.text = "";
        }

        int currentIndex = 0;

        foreach (PC c in playerCharacters)
        {
            if (attackOrder.Contains(c))
            {
                int index = attackOrder.IndexOf(c);
                attackPanels[index].enabled = true;
                attackTexts[index].color = c.GetComponent<SpriteRenderer>().color;
                attackTexts[index].text = c.SelectedAttack().Name;
                currentIndex++;
            }
        }

        if (enemyInstance.health <= 0)
        {
            HealthText.text = "Battle Won";
            StartCoroutine(StartNewBattleDelayed(2f));
        }


        foreach (PC c in playerCharacters)
        {
            if (c.health > 0) return;
        }

        HealthText.text = "Game Over";
        StartCoroutine(StartNewBattleDelayed(2f));
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

        int charactersalive = 0;
        foreach (PC c in playerCharacters)
        {
            if (c.health > 0) charactersalive++;
        }

        bool everyoneAlive = true;

        foreach (PC c in lastAttackOrder)
        {
            if (c.health <= 0)
            {
                everyoneAlive = false;
            }
        }

        RepeatButton.interactable = everyoneAlive;


        if (enemyInstance.health > 0 && !HealthText.text.Equals("Game Over"))
        {

            if (Input.GetKeyDown(KeyCode.Return) || confirmPressed || attackOrder.Count == charactersalive)
            {
                if (!turnInProgress)
                {
                    enemyAttacking = Random.Range(1, 101) > 25;
                    StartCoroutine(InitiateAttack());
                    turnInProgress = true;
                }
            }
            else if (Input.GetKeyDown(KeyCode.Space) || repeatPressed)
            {
                foreach (PC c in lastAttackOrder)
                {
                    if (c.health <= 0) return;
                }
                attackOrder = lastAttackOrder;
            }
        }
        confirmPressed = false;
        repeatPressed = false;
    }

    IEnumerator InitiateAttack()
    {
        if (attackOrder.Count > 0)
            lastAttackOrder = new List<PC>();
        foreach (PC c in attackOrder)
        {
            lastAttackOrder.Add(c);
        }

        if (!enemyAttacking)
        {
            enemyInstance.Defend();
            if (attackOrder.Count == 0)
            {
                yield return new WaitForSeconds(0.6f);
            }
        }

        foreach (PC c in attackOrder)
        {
            Vector3 currentPosition = c.transform.position;
            StartCoroutine(Move(c.gameObject, enemyInstance.transform.position + new Vector3(0.8f, 0)));
            c.AttackTarget(enemyInstance);
            if (enemyInstance.health <= 0)
            {
                yield return new WaitForSeconds(attackDelay);
                StartCoroutine(Move(c.gameObject, currentPosition));
                yield break;
            }
            yield return new WaitForSeconds(attackDelay);
            StartCoroutine(Move(c.gameObject, currentPosition));
        }

        yield return new WaitForSeconds(0.3f);


        if (enemyAttacking)
        {
            int playerHealth = -1;
            PC attackTarget = null;
            while (playerHealth <= 0)
            {
                attackTarget = playerCharacters[Random.Range(0, playerCharacters.Count)];
                playerHealth = attackTarget.health;
            }
            if (attackTarget == null) yield break;
            Vector3 currentPosition = enemyInstance.transform.position;
            StartCoroutine(Move(enemyInstance.gameObject,
                attackTarget.transform.position + new Vector3(-0.8f, 0)));
            enemyInstance.AttackTarget(attackTarget);
            yield return new WaitForSeconds(attackDelay);
            StartCoroutine(Move(enemyInstance.gameObject, currentPosition));
            yield return new WaitForSeconds(0.3f);
        }

        if (!enemyAttacking) enemyInstance.StopDefending();

        enemyAttacking = false;

        attackOrder.Clear();

        turnInProgress = false;

    }

    private IEnumerator Move(GameObject objectToMove, Vector3 destination)
    {
        float remainingDistance = (objectToMove.transform.position - destination).sqrMagnitude;

        while (remainingDistance > Single.Epsilon)
        {
            Vector3 newPosition = Vector3.MoveTowards(objectToMove.transform.position, destination,
                15 * Time.deltaTime);
            objectToMove.transform.position = newPosition;
            remainingDistance = (objectToMove.transform.position - destination).sqrMagnitude;
            yield return null;
        }

    }

    public bool EnemyAttacking()
    {
        return enemyAttacking;
    }

}
