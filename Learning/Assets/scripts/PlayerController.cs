using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour {

    public float speed;

    public Text scoreText;

    public Text winText;

    private int score;

    private Rigidbody playerRigidbody;
    
    void Start() //first frame script is active
    {
        playerRigidbody = GetComponent<Rigidbody>();
        score = 0;
    }

    void Update() //before rendering frames
    {
        
    }

    void FixedUpdate() //before rendering physics
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        playerRigidbody.AddForce(moveHorizontal * speed, 0, moveVertical * speed);


    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("pickup"))
        {
            other.gameObject.SetActive(false);
            score++;
            scoreText.text = "SCORE: " + score.ToString();
            if (score >= 12)
            {
                winText.text = "YOU WIN!";
            }
        }
    }

}
