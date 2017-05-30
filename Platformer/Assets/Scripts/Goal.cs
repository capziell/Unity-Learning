using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Goal : MonoBehaviour
{

	// Use this for initialization
	void Start ()
	{
	}
	
	// Update is called once per frame
	void Update () {
	    if (Physics2D.IsTouchingLayers(GetComponent<BoxCollider2D>(), 1 << LayerMask.NameToLayer("Objects")))
	    {
	        SceneManager.LoadScene(0);
	    }
	}
}
