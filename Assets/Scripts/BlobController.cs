using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlobController : MonoBehaviour {
	private Rigidbody2D rb;
	public GameObject blobFragmentPrefab;
	private GameObject player;

	private List<GameObject> blobFragments;
	private int blobFragmentLayer = 8;

	// Use this for initialization
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

		player = GameObject.FindGameObjectsWithTag("Player")[0];

		Vector2 myPos = transform.position;
		Vector2 playerPos = player.transform.position;

		Vector2 moveToPlayerVector = new Vector2(playerPos.x - myPos.x, playerPos.y - myPos.y);

		rb.velocity = moveToPlayerVector * 0.05f;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
