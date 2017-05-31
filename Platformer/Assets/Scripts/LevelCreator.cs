using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelCreator : MonoBehaviour
{

    public GameObject goal;

    public GameObject platform;
    public GameObject collectable;
    public GameObject life;
    public int numberOfPlatforms = 10;
    public float verticalMin = -4f;
    public float verticalMax = 2.5f;
    public float horizontalMin = 7.5f;
    public float horizontalMax = 14.5f;

    private Vector2 originPosition;

	// Use this for initialization
	void Awake ()
	{
	    originPosition = transform.position;
	    for (int i = 0; i < numberOfPlatforms; i++)
	    {
	        Vector2 nextPosition = originPosition +
	                               new Vector2(Random.Range(horizontalMin, horizontalMax),
	                                   Random.Range(verticalMin, verticalMax));
	        Instantiate(platform, nextPosition, Quaternion.identity);
	        for (int j = 0; j < Random.Range(0, 4); j++)
	        {
	            Instantiate(collectable, nextPosition + new Vector2(-1.5f + 1.5f * j, 1f), Quaternion.identity);
	        }

            int a = Random.Range(1, 101);
            if(a >= 95)
            {
                Instantiate(life, nextPosition + new Vector2(0, 3f), Quaternion.identity);
            }

	        originPosition = nextPosition;
	    }
	    Instantiate(goal, originPosition + new Vector2(1.8f, 1f), Quaternion.identity);

	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
