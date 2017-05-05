using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlobController : MonoBehaviour {
	private Rigidbody2D rb;
	private GameObject player;

	public GameObject blobFragmentPrefab;

	private List<GameObject> blobFragments;
	private int blobFragmentLayer = 8;
	private float moveSpeed = 1f;
	private float boundsOffsetFactor = 0.05f;

	/**
	 * START
	 */
	void Start () {
		rb = GetComponent<Rigidbody2D>();
		blobFragments = new List<GameObject>();

		Physics2D.IgnoreLayerCollision(blobFragmentLayer, blobFragmentLayer);
		int numberOfFragments = Random.Range(3, 7);
		for(int i = 0; i < numberOfFragments; i++){
			blobFragments.Add(
				ObjectPoolController.InstantiateFromPool(blobFragmentPrefab, transform.position, transform.rotation)
			);
			blobFragments[i].transform.parent = gameObject.transform;
		}

		rb.velocity = Random.insideUnitCircle * moveSpeed;
	}

	void Update(){
		boundBlobToCameraView();

		updateChildFragmentState();
	}

	/**
		Make blob stay within camera view.
	*/
	void boundBlobToCameraView(){
		float x = transform.position.x;
		float y = transform.position.y;

		float xVel = rb.velocity.x;
		float yVel = rb.velocity.y;

		Vector2 camMinPoint = new Vector2(0f + (Screen.width * boundsOffsetFactor), 0f + (Screen.height * boundsOffsetFactor));
		Vector2 camMaxPoint = new Vector2(Screen.width - (Screen.width * boundsOffsetFactor), Screen.height - (Screen.height * boundsOffsetFactor));

		Vector2 min = Camera.main.ScreenToWorldPoint(camMinPoint);
		Vector2 max = Camera.main.ScreenToWorldPoint(camMaxPoint);

		if(x > max.x || x < min.x){
			xVel = -xVel;
			reverseChildFragmentHorizVelocity();
		}

		if(y > max.y || y < min.y){
			yVel = -yVel;
			reverseChildFragmentVerticalVelocity();
		}

		rb.velocity = new Vector2(xVel, yVel);
	}

	void reverseChildFragmentHorizVelocity(){
		for(int i = 0; i < blobFragments.Count; i++){
			if(blobFragments[i] != null){
				Rigidbody2D childRb = blobFragments[i].GetComponent<Rigidbody2D>();
				childRb.velocity = new Vector2(-childRb.velocity.x, childRb.velocity.y);
			}
		}
	}

	void reverseChildFragmentVerticalVelocity(){
		for(int i = 0; i < blobFragments.Count; i++){
			if(blobFragments[i] != null){
				Rigidbody2D childRb = blobFragments[i].GetComponent<Rigidbody2D>();
				childRb.velocity = new Vector2(childRb.velocity.x, -childRb.velocity.y);
			}
		}
	}

	void updateChildFragmentState(){
		for(int i = 0; i < blobFragments.Count; i++){
			if(blobFragments[i] == null){
				blobFragments.RemoveAt(i);
				i--;
			}
		}

		if(blobFragments.Count == 0){
			Destroy(gameObject);
		}
	}
}
