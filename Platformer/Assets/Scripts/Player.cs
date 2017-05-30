using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    private Rigidbody2D rb2d;

    private GameManager gameManager;

    public float speed;
    public float jumpSpeed;

    private bool jump;
    private bool right;
    private bool left;
    private bool moving;
    private bool died;

    // Use this for initialization
    void Start ()
    { 
	    rb2d = GetComponent<Rigidbody2D>();
	    rb2d.freezeRotation = true;
        gameManager = FindObjectOfType<GameManager>();
    }
	
	// Update is called once per frame
	void Update () {

	    if (Input.GetKeyDown(KeyCode.UpArrow))
	    {
	        if (Physics2D.IsTouchingLayers(GetComponent<BoxCollider2D>(), 1 << LayerMask.NameToLayer("Platforms")) &&
	            Mathf.Abs(rb2d.velocity.y) < 0.5f)
	            jump = true;
	            
	    }

        if (Input.GetKey(KeyCode.RightArrow))
        {
            if (rb2d.velocity.x <= 10) right = true;

        }
        else if (Input.GetKey(KeyCode.LeftArrow))
        {
            if (rb2d.velocity.x >= -10) left = true;

        }
	    else if (Mathf.Abs(rb2d.velocity.x) > Single.Epsilon)
        {
            moving = true;
        }

        if (rb2d.position.y <= -50f)
        {
            died = true;
	    }

	}

    void FixedUpdate()
    {
        if (jump)
        {
            rb2d.AddForce(new Vector2(0, jumpSpeed), ForceMode2D.Impulse);
            jump = false;
        }

        if (right)
        {
            rb2d.AddForce(new Vector2(speed, 0));
            right = false;
        }
        else if (left)
        {
            rb2d.AddForce(new Vector2(-speed, 0));
            left = false;
        }
        else if (moving)
        {
            rb2d.AddForce(new Vector2(-15 * rb2d.velocity.x, 0), 0);
            moving = false;
        }

        if (died)
        {
            rb2d.MovePosition(new Vector2(0, 0));
            rb2d.velocity = Vector2.zero;
            rb2d.angularVelocity = 0;
            gameManager.AddScore(-3);
            died = false;
        }

    }

}


