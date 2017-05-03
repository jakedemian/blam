using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotController : MonoBehaviour {
	
	// Update is called once per frame
	void Update () {
		Vector2 camMinPoint = new Vector2(0f, 0f);
		Vector2 camMaxPoint = new Vector2(Screen.width, Screen.height);

		Vector2 min = Camera.main.ScreenToWorldPoint(camMinPoint);
		Vector2 max = Camera.main.ScreenToWorldPoint(camMaxPoint);

		if(!isIn(transform.position.x, min.x, max.x) || !isIn(transform.position.y, min.y, max.y)){
			gameObject.SetActive(false);
		}
	}

	private bool isIn(float value, float min, float max){
		return value < max && value > min;
	}

	void OnCollisionEnter2D(Collision2D c){
		gameObject.SetActive(false);
	}
}
