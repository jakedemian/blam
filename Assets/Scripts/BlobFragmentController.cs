using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlobFragmentController : MonoBehaviour {
	private Rigidbody2D rb;
	private Rigidbody2D parentRb;
	private CircleCollider2D collider;
	private GameObject parentBlob;
	private GameObject grabber;

	private float scale;
	private float MAX_DISTANCE_FROM_PARENT_CENTER = 0.3f;

	public bool drawDebugInfo = false;
	private bool isEatable = false;
	private bool isGrabbed = false;

	public Sprite blackSprite;
	public Sprite redSprite;

	/**
		START
	*/
	void Start () {
		if(collider == null){
			collider = GetComponent<CircleCollider2D>();
		}

		rb = GetComponent<Rigidbody2D>();
		parentBlob = transform.parent.gameObject;
		parentRb = parentBlob.GetComponent<Rigidbody2D>();

		scale = Random.Range(0.7f, 0.9f);
		transform.localScale = new Vector2(scale, scale);
		collider.radius = scale / 2f;

		float xVel = parentRb.velocity.x + Random.Range(-0.3f, 0.3f);
		float yVel = parentRb.velocity.y + Random.Range(-0.3f, 0.3f);
		rb.velocity = new Vector2(xVel, yVel);
	}
	
	/**
		UPDATE
	*/
	void Update () {
		updateBlobFragmentColor();

		if(!isGrabbed){
			boundBlobFragmentsToParent();
		} else {
			transform.position = grabber.transform.position;
		}

		if(drawDebugInfo){
			doDrawDebugInfo();
		}
	}

	/**
		ON COLLISION ENTER
	*/
	void OnCollisionEnter2D (Collision2D c) { 
	    if (c.gameObject.tag == "PlayerShot") {
	    	isEatable = true;
	    } else if(c.gameObject.tag == "Grabber" && isEatable){
	    	isGrabbed = true;
	    	grabber = c.gameObject;
	    	c.gameObject.GetComponent<GrabberController>().triggerGrabEvent(gameObject);
	    }
	}

	/**
		Make blob fragments stay within a certain distance of their parent.
	*/
	void boundBlobFragmentsToParent(){
		float x = transform.position.x;
		float y = transform.position.y;

		float px = parentBlob.transform.position.x;
		float py = parentBlob.transform.position.y;

		float xVel = rb.velocity.x;
		float yVel = rb.velocity.y;

		if((x > px && x - px > MAX_DISTANCE_FROM_PARENT_CENTER) || (x < px && px - x > MAX_DISTANCE_FROM_PARENT_CENTER)){
			float relativeXVelocityToParent = rb.velocity.x - parentRb.velocity.x;
			xVel = (relativeXVelocityToParent * -1f) + parentRb.velocity.x;
		}

		if((y > py && y - py > MAX_DISTANCE_FROM_PARENT_CENTER) || (y < py && py - y > MAX_DISTANCE_FROM_PARENT_CENTER)){
			float relativeYVelocityToParent = rb.velocity.y - parentRb.velocity.y;
			yVel = (relativeYVelocityToParent * -1f) + parentRb.velocity.y;
		}

		rb.velocity = new Vector2(xVel, yVel);
	}

	/**
		Keep the fragment's color in line with it's isEatable state.
	*/
	void updateBlobFragmentColor(){
		if(isEatable && GetComponent<SpriteRenderer>().sprite != redSprite){
			GetComponent<SpriteRenderer>().sprite = redSprite;
		} else if(!isEatable && GetComponent<SpriteRenderer>().sprite != blackSprite){
			GetComponent<SpriteRenderer>().sprite = blackSprite;
		}
	}

	/**
		Draw debug lines in the scene editor, is debugging is enabled for this object.
	*/
	void doDrawDebugInfo(){
		Vector2 center = parentBlob.transform.position;

		Debug.DrawRay(center + (Vector2.up 		* MAX_DISTANCE_FROM_PARENT_CENTER), Vector2.left 	* MAX_DISTANCE_FROM_PARENT_CENTER, Color.blue);
		Debug.DrawRay(center + (Vector2.up 		* MAX_DISTANCE_FROM_PARENT_CENTER), Vector2.right 	* MAX_DISTANCE_FROM_PARENT_CENTER, Color.blue);
		Debug.DrawRay(center + (Vector2.left 	* MAX_DISTANCE_FROM_PARENT_CENTER), Vector2.up 		* MAX_DISTANCE_FROM_PARENT_CENTER, Color.blue);
		Debug.DrawRay(center + (Vector2.left 	* MAX_DISTANCE_FROM_PARENT_CENTER), Vector2.down 	* MAX_DISTANCE_FROM_PARENT_CENTER, Color.blue);
		Debug.DrawRay(center + (Vector2.down 	* MAX_DISTANCE_FROM_PARENT_CENTER), Vector2.left 	* MAX_DISTANCE_FROM_PARENT_CENTER, Color.blue);
		Debug.DrawRay(center + (Vector2.down 	* MAX_DISTANCE_FROM_PARENT_CENTER), Vector2.right 	* MAX_DISTANCE_FROM_PARENT_CENTER, Color.blue);
		Debug.DrawRay(center + (Vector2.right 	* MAX_DISTANCE_FROM_PARENT_CENTER), Vector2.up 		* MAX_DISTANCE_FROM_PARENT_CENTER, Color.blue);
		Debug.DrawRay(center + (Vector2.right 	* MAX_DISTANCE_FROM_PARENT_CENTER), Vector2.down 	* MAX_DISTANCE_FROM_PARENT_CENTER, Color.blue);

		Debug.DrawRay(transform.position, (Vector2.up * 0.1f), Color.yellow);
		Debug.DrawRay(transform.position, (Vector2.down * 0.1f), Color.yellow);
		Debug.DrawRay(transform.position, (Vector2.left * 0.1f), Color.yellow);
		Debug.DrawRay(transform.position, (Vector2.right * 0.1f), Color.yellow);
	}
}
