using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {

	public GameObject blobPrefab;
	public GameObject player;

	private static int level = 1;
	private int numberOfBlobsToSpawn;
	private int blobsSpawned = 0;

	private Vector2 camMin;
	private Vector2 camMax;

	private List<GameObject> blobs;

	// Use this for initialization
	void Start () {
		blobs = new List<GameObject>();		
		numberOfBlobsToSpawn = (int)((level / 1.5f) + (0.01f * Mathf.Pow(level, 2)));

		Vector2 camMinPointInPixels = new Vector2(0f, 0f);
		Vector2 camMaxPointInPixels = new Vector2(Screen.width, Screen.height);

		Vector2 camMin = Camera.main.ScreenToWorldPoint(camMinPointInPixels);
		Vector2 camMax = Camera.main.ScreenToWorldPoint(camMaxPointInPixels);

		for(int i = 1; i < numberOfBlobsToSpawn; i++){
			float xPoint = Random.Range(camMin.x, camMax.x);
			float yPoint = Random.Range(camMin.y, camMax.y);

			blobs.Add(Instantiate(blobPrefab, new Vector2(xPoint, yPoint), Quaternion.identity));
		}
	}

	void Update(){
		if(isLevelComplete()){
			level++;
			Application.LoadLevel("main");
		}
	}

	bool isLevelComplete(){
		bool levelComplete = true;

		for(int i = 0; i < blobs.Count; i++){
			if(blobs[i] != null){
				levelComplete = false;
				break;
			}
		}

		return levelComplete;
	}
}
