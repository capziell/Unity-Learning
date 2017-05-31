using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;

    public Text ScoreText;
    public Text LivesText;

    private int score;
    private int lives = 3;

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
        if (lives > 0)
        {
            score += i;
            ScoreText.text = "Score: " + score;
        }
    }

    public void AddLife(int i)
    {
        lives += i;
        if(lives <= 0)
        {
            LivesText.text = "Game Over. Final Score: " + score;
            ScoreText.text = "";
            Destroy(FindObjectOfType<Player>());
        }
        else
        {
            LivesText.text = "Lives: " + lives;
        }
    }

    // Update is called once per frame
	void Update ()
	{
	}
}
