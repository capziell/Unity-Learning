using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public abstract class MovingObject : MonoBehaviour
{

    public float moveTime = 0.1f;
    public LayerMask BlockingLayerMask;

    private BoxCollider2D boxCollider2D;
    private Rigidbody2D rb2D;
    private float inverseMoveTime;


	// Use this for initialization
	protected virtual void Start ()
	{
	    boxCollider2D = GetComponent<BoxCollider2D>();
	    rb2D = GetComponent<Rigidbody2D>();
	    inverseMoveTime = 1f / moveTime;

	}

    protected bool Move(int xDirection, int yDirection, out RaycastHit2D hit)
    {
        Vector2 start = transform.position;
        Vector2 end = start + new Vector2(xDirection, yDirection);
        boxCollider2D.enabled = false;
        hit = Physics2D.Linecast(start, end, BlockingLayerMask);
        boxCollider2D.enabled = true;

        if (hit.transform == null)
        {
            StartCoroutine(SmoothMovement(end));
            return true;
        }
        return false;
    }

    protected IEnumerator SmoothMovement(Vector3 end)
    {
        float squareRemainingDistance = (transform.position - end).sqrMagnitude;

        while (squareRemainingDistance > float.Epsilon)
        {
            Vector3 newPosition = Vector3.MoveTowards(rb2D.position, end, inverseMoveTime * Time.deltaTime);
            rb2D.MovePosition(newPosition);
            squareRemainingDistance = (transform.position - end).sqrMagnitude;
            yield return null;
        }
    }

    protected virtual void AttemptMove<T>(int xDirection, int yDirection) where T : Component
    {
        RaycastHit2D hit;
        bool canMove = Move(xDirection, yDirection, out hit);

        if (hit.transform == null)
        {
            return;
        }
        T hitComponent = hit.transform.GetComponent<T>();

        if (!canMove && hitComponent != null)
        {
            OnCantMove(hitComponent);
        }
    }

    protected abstract void OnCantMove<T>(T component) where T : Component;


}
