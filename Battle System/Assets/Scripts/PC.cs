using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PC : MonoBehaviour
{
    public int health = 5;
    public int attack = 2;

    private Text damageText;

    // Use this for initialization
    void Start()
    {
        damageText = GetComponentInChildren<Text>();
        ClearText();
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
