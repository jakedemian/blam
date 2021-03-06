﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabberController : MonoBehaviour {
	private Rigidbody2D rb;
	private Collider2D collider;
	private GameObject player;
	private GameObject line;

	private Vector2 launchDirection;
	private Vector2 launchOppositeDir;

	private bool isLaunched = false;
	private float launchSpeed = 12f;
	private float decelerationSpeed = 0.8f;

	private bool isRebounding = false;
	private float currentReboundSpeed = 0f;
	private float MAX_REBOUND_SPEED = 15f;
	private float reboundAcceleration = 0.6f;

	private const float GRABBER_FINISHED_DIST = 0.3f;

	private bool isHoldingObject = false;
	private GameObject holdingObject;
	
	/**
	 * FIXED UPDATE
	 */
	void FixedUpdate () {
		if(isLaunched){
			if(isRebounding){
				Vector2 playerPos = player.transform.position;
				Vector2 myPos = transform.position;
				Vector2 diffs = playerPos - myPos;		

				float vectorToPlayer = Mathf.Sqrt((diffs.x * diffs.x) + (diffs.y * diffs.y));

				// vector with magnitude 1 pointing to player
				Vector2 unitVectorToPlayer = diffs / vectorToPlayer;

				currentReboundSpeed = currentReboundSpeed + reboundAcceleration > MAX_REBOUND_SPEED ? 
					MAX_REBOUND_SPEED : 
					currentReboundSpeed + reboundAcceleration;

				rb.velocity = unitVectorToPlayer * currentReboundSpeed;
			}else {
				rb.velocity -= (decelerationSpeed * launchDirection);

				if(Vector2.Dot(launchOppositeDir, rb.velocity) > 0){
					isRebounding = true;
					rb.velocity = Vector2.zero;
				}
			}
		}
	}

	/**
	 * UPDATE
	 */
	void Update(){
		if(isLaunched){
			Vector2 playerPos = player.transform.position;
			Vector2 myPos = transform.position;
			Vector2 diffs = playerPos - myPos;

			// keep grabber rotation in line with player position
			float angleRads = Mathf.Atan2(diffs.x, diffs.y);
			transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, 180f - (angleRads * Mathf.Rad2Deg));

			drawGrabberToPlayerLine();

			if(isRebounding 
			&& getDistanceBetweenTwoPoints(transform.position, player.transform.position) <= GRABBER_FINISHED_DIST){
				grabberFinished();
			}
		}
	}

	/**
	 * Handles work to be done when the grabber gets back to the player.
	 */
	void grabberFinished(){
		if(line != null) {
			Destroy(line);
		}

		if(isHoldingObject){
			Destroy(holdingObject);
			player.GetComponent<PlayerController>().growPlayer();
		}

		Destroy(gameObject);
	}

	/**
	 * Draws the connecting line between the player and the grabber.
	 */
	void drawGrabberToPlayerLine(){
		if(line != null) {
			Destroy(line);
		}

		line = new GameObject();
		line.transform.position = new Vector3(0f, 0f, 0f);
		line.AddComponent<LineRenderer>();

		LineRenderer lr = line.GetComponent<LineRenderer>();
		lr.material = new Material(Shader.Find("Particles/Alpha Blended Premultiply"));
		lr.startColor = Color.red;
		lr.endColor = Color.red;
		lr.startWidth = 0.1f;
		lr.endWidth = 0.1f;
		lr.SetPosition(0, transform.position);
		lr.SetPosition(1, player.transform.position);
	}

	/**
		Launch the grabber in the direction dir.

		@param dir 			The direction to launch the grabber.
		@param playerObj 	The player gameObject.
	*/
	public void launch(Vector2 dir, GameObject playerObj){
		player = playerObj;
		launchDirection = dir;
		launchOppositeDir = -dir;

		getRigidbody2D().AddForce((dir * launchSpeed) + player.GetComponent<Rigidbody2D>().velocity, ForceMode2D.Impulse);
		isLaunched = true;
	}

	/**
	 * Method that is called when a blob fragment detects that it has been grabbed.
	 * @param  objGrabbed	The gameObject that was grabbed.
	 */
	public void triggerGrabEvent(GameObject objGrabbed){
		isHoldingObject = true;
		getCollider2D().enabled = false;
		holdingObject = objGrabbed;
	}

	/**
	 * Obtain the distance between two points.
	 * 
	 * @param  a The first point.
	 * @param  b The second point.
	 * @return The distance between a and b.
	 */
	float getDistanceBetweenTwoPoints(Vector2 a, Vector2 b){
		return Mathf.Sqrt(Mathf.Pow(a.x-b.x, 2) + Mathf.Pow(a.y-b.y, 2));
	}

	/**
	 * Get this object's rigidbody, lazy load if needed.
	 * @return The rigidbody attached to the grabber.
	 */
	public Rigidbody2D getRigidbody2D(){
		if(rb == null){
			rb = GetComponent<Rigidbody2D>();
		}
		return rb;
	}

	/**
	 * Get this object's rigidbody, lazy load if needed.
	 * @return The collider attached to this object
	 */
	public Collider2D getCollider2D(){
		if(collider == null){
			collider = GetComponent<Collider2D>();
		}
		return collider;
	}
}
