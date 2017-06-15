﻿using System.Collections;
using System.Collections.Generic;
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

    public List<PC> attackOrder = new List<PC>();

    private float attackDelay = 0.3f;

    private bool enemyAttacking;

    private PC playerOneInstance;
    private PC playerTwoInstance;
    private Enemy enemyInstance;

    // Use this for initialization
    void Start()
    {
        playerOneInstance = Instantiate(playerOne, new Vector3(1f, 0f), Quaternion.identity);
        playerOneInstance.keyCode = KeyCode.LeftArrow;
        playerOneInstance.gameManager = this;
        playerTwoInstance = Instantiate(playerTwo, new Vector3(2f, 0f), Quaternion.identity);
        playerTwoInstance.keyCode = KeyCode.RightArrow;
        playerTwoInstance.gameManager = this;
        enemyInstance = Instantiate(enemy, new Vector3(-1f, 0f), Quaternion.identity);

        UpdateText();
    }

    private void UpdateText()
    {
        HealthText.text = "Enemy: " + enemyInstance.HealthState() + " 1: " + playerOneInstance.HealthState() +
                          " 2: " +
                          playerTwoInstance.HealthState();
        AttackText.text = "1:";
        if (attackOrder.Contains(playerOneInstance))
        {
            AttackText.text = (attackOrder.FindIndex(c => c == playerOneInstance) + 1).ToString() + ":";
            AttackText.text += playerOneInstance.GetAttackName(attackOrder.FindIndex(c => c == playerOneInstance));
        }
        else if (playerOneInstance.health > 0) AttackText.text += " Defend ";
        else AttackText.text += " Dead ";
        if (!attackOrder.Contains(playerTwoInstance))
        {
            AttackText.text += "2:";
        }
        if (attackOrder.Contains(playerTwoInstance))
        {
            AttackText.text += (attackOrder.FindIndex(c => c == playerTwoInstance) + 1).ToString() + ":";
            AttackText.text += playerTwoInstance.GetAttackName(attackOrder.FindIndex(c => c == playerTwoInstance));
        }
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
        UpdateText();
        if (enemyInstance.health > 0 && (playerOneInstance.health > 0 || playerTwoInstance.health > 0))
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
            if (enemyAttacking)
            {
                enemyInstance.AddHealth(-c.SelectedAttack().Damage);
            }
            else enemyInstance.AddHealth(-c.SelectedAttack().Damage / 2);
            c.transform.Translate(Vector3.left);
            yield return new WaitForSeconds(attackDelay);
            c.transform.Translate(Vector3.right);
        }
        if (enemyAttacking)
        {
            enemyInstance.transform.Translate(Vector3.right);
            if (playerOneInstance.health <= 0)
            {
                if (attackOrder.Contains(playerTwoInstance)) playerTwoInstance.AddHealth(-enemyInstance.attack);
                else playerTwoInstance.AddHealth(-enemyInstance.attack / 2);
                UpdateText();
                yield return new WaitForSeconds(attackDelay);
                enemyInstance.transform.Translate(Vector3.left);
            }
            if (playerTwoInstance.health <= 0)
            {
                if (attackOrder.Contains(playerOneInstance)) playerOneInstance.AddHealth(-enemyInstance.attack);
                else playerOneInstance.AddHealth(-enemyInstance.attack / 2);
                UpdateText();
                yield return new WaitForSeconds(attackDelay);
                enemyInstance.transform.Translate(Vector3.left);
            }
            if (Random.Range(1, 3) == 1)
            {
                if (attackOrder.Contains(playerOneInstance)) playerOneInstance.AddHealth(-enemyInstance.attack);
                else playerOneInstance.AddHealth(-enemyInstance.attack / 2);
            }
            else if (attackOrder.Contains(playerTwoInstance)) playerTwoInstance.AddHealth(-enemyInstance.attack);
            else playerTwoInstance.AddHealth(-enemyInstance.attack / 2);
            UpdateText();
            yield return new WaitForSeconds(attackDelay);
            enemyInstance.transform.Translate(Vector3.left);
        }
        enemyAttacking = false;
        attackOrder.Clear();
        UpdateText();

    }
}
