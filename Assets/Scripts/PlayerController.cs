using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
	private Rigidbody2D rb;
	private BoxCollider2D collider;
	private float shotCooldownTimer = 0f;

	public GameObject shotPrefab;
	public GameObject grabberPrefab;

	private int currentHealth = 5;


	// CONSTANTS
	public float MOVEMENT_SPEED_FACTOR = 10f;
	public float TURN_SPEED_FACTOR = 2f;
	public float MAX_TURN_SPEED = 150f;
	public float MAX_MOVE_SPEED = 5f;
	public float SHOT_COOLDOWN = 0.2f;
	private float inactiveLinearDrag = 5f;
	private float inactiveAngularDrag = 10f;

	/**
	 * START
	 */
	void Start () {
		rb = GetComponent<Rigidbody2D>();
		collider = GetComponent<BoxCollider2D>();
	}

	/**
	 * UPDATE
	 */
	void Update(){
		handlePlayerShoot();
		updatePlayerDrag();
		if(currentHealth <= 0){
			Application.LoadLevel("main");
		}

		if(Input.GetButtonDown("Grabber") && GameObject.FindGameObjectsWithTag("Grabber").Length == 0){
			GameObject grabber = Instantiate(grabberPrefab, transform.position, transform.rotation);
			grabber.GetComponent<GrabberController>().launch(transform.up, gameObject);
		}
	}
	
	/**
	 * FIXED UPDATE
	 */
	void FixedUpdate () {
		Vector2 inputDir = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

		if(inputDir.x != 0f || inputDir.y != 0f){
			// float angle = Vector2.Angle(transform.up, inputDir);
			// angle = inputDir.y < 0 ? -angle : angle;
			// angle = angle + 180f;
			// Debug.Log(angle);
			// if(Mathf.Abs(angle) > 1f){
			// 	transform.Rotate(Vector3.forward, 5f * Mathf.Sign(angle));
			// }

			rb.AddForce(inputDir * MOVEMENT_SPEED_FACTOR);
			capMoveSpeed();
		}

		float spin = Input.GetAxisRaw("SpinCCW") - Input.GetAxisRaw("SpinCW");
		rb.AddTorque(spin * TURN_SPEED_FACTOR);
		capTurnSpeed();
	}

	/**
	 * Keep the magnitude of the player's move speed under the MAX_MOVE_SPEED limit.
	 */
	void capMoveSpeed(){
		float currentMagnitude = getVelocityMagnitude();
		if(currentMagnitude > MAX_MOVE_SPEED){
			float currentX = rb.velocity.x;
			float currentY = rb.velocity.y;

			float ratio = MAX_MOVE_SPEED / currentMagnitude;
			float newX = currentX * ratio;
			float newY = currentY * ratio;

			rb.velocity = new Vector2(newX, newY);
		}
	}

	/**
	 * Keep the turn speed under MAX_TURN_SPEED
	 */
	void capTurnSpeed(){
		if(rb.angularVelocity > MAX_TURN_SPEED){
			rb.angularVelocity = MAX_TURN_SPEED;
		} else if(rb.angularVelocity < -MAX_TURN_SPEED){
			rb.angularVelocity = -MAX_TURN_SPEED;
		}
	}

	/**
	 * Calculate the magnitude of the player's current velocity.
	 * @return The magnitude of the player's current velocity.
	 */
	float getVelocityMagnitude(){
		float x = rb.velocity.x;
		float y = rb.velocity.y;

		return Mathf.Sqrt((x*x) + (y*y));
	}

	/**
	 * Handle the player shoot event.
	 */
	void handlePlayerShoot(){
		if(Input.GetButtonDown("Shoot") && shotCooldownTimer <= 0f){
			shotCooldownTimer = SHOT_COOLDOWN;
			Vector3[] directions = {transform.up, transform.right, -transform.up, -transform.right};

			for(int i = 0; i < directions.Length; i++){
				Vector2 spawnLocation = transform.position + directions[i] * (0.5f + shotPrefab.transform.localScale.x * 0.5f);
				Debug.DrawRay(spawnLocation, directions[i] * 2f, Color.red);
			
				GameObject go = ObjectPoolController.InstantiateFromPool(shotPrefab, spawnLocation, transform.rotation);
				go.GetComponent<Rigidbody2D>().velocity = /*new Vector3(rb.velocity.x, rb.velocity.y, 0f) +*/ (directions[i] * 1f);

				if(directions[i] == transform.right || directions[i] == -transform.right){
					go.transform.Rotate(new Vector3(0f, 0f, 90f));
				}
			}

			shrinkPlayer();	
		}
		if(shotCooldownTimer > 0f){
			shotCooldownTimer -= Time.deltaTime;
		}
	}

	/**
	 * Update the player's angular and linear drag based on current user input.
	 */
	void updatePlayerDrag(){
		rb.angularDrag = Input.GetAxisRaw("SpinCCW") == 0 && Input.GetAxisRaw("SpinCW") == 0 ? inactiveAngularDrag : 0f;
		rb.drag = Input.GetAxisRaw("Horizontal") == 0 && Input.GetAxisRaw("Vertical") == 0 ? inactiveLinearDrag : 0f;
	}

	/**
	 * Shrink the player's size and decrement player health.
	 */
	public void shrinkPlayer(){
		currentHealth--;

		// TODO FIXME make shrink amount a constant
		transform.localScale = transform.localScale * 0.85f;
	}

	public void growPlayer(){
		currentHealth++;
		transform.localScale = transform.localScale * 1.15f;
	}
}
