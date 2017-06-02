using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

    private GameManager gameManager;

    private Vector3 initialposition;
    private Vector2 walkamount = new Vector2();
    private float speed = 1.6f;
    private bool goRight = true;

	// Use this for initialization
	void Start () {
        gameManager = FindObjectOfType<GameManager>();
        initialposition = transform.position;
	}
	
	// Update is called once per frame
	void Update () {
        float distance = speed * Time.deltaTime;
        if (goRight)
        {
            if(transform.position.x <= initialposition.x + 2f)
            {
                transform.Translate(distance, 0, 0);
            }
            else
            {
                goRight = false;
                transform.Translate(-distance, 0, 0);
            }
        }
        else
        {
            if(transform.position.x >= initialposition.x - 2f)
            {
                transform.Translate(-distance, 0, 0);
            }
            else
            {
                goRight = true;
                transform.Translate(distance, 0, 0);
            }
        }
		
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            gameManager.AddScore(-10);
            Destroy(gameObject);
        }
    }
}
