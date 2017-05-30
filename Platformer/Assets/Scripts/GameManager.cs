using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;

    public Text ScoreText;

    private int score;

	// Use this for initialization
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
    }

    public void AddScore(int i)
    {
        score += i;
        if (score < 0) score = 0;
        ScoreText.text = "Score: " + score;
    }

    // Update is called once per frame
	void Update ()
	{
	}
}
