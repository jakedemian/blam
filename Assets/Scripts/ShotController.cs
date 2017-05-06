using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotController : MonoBehaviour {

	private float scaleMax;
	private float scaleChangeMax = 0.1f;
	private float scaleStep = 0.01f;
	private bool shrinking = false;

	void Start(){
		scaleMax = transform.localScale.y;
	}
	
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


	void FixedUpdate(){
		float scMax = scaleMax;
		float scMin = scaleMax - scaleChangeMax;

		if(shrinking){
			transform.localScale = new Vector2(scMax, transform.localScale.y - scaleStep);
			if(transform.localScale.y < scMin){
				transform.localScale = new Vector2(scMax, scMin);
				shrinking = false;
			}
		} else {
			transform.localScale = new Vector2(scMax, transform.localScale.y + scaleStep);
			if(transform.localScale.y > scMax){
				transform.localScale = new Vector2(scMax, scMax);
				shrinking = true;
			}
		}

	}

	private bool isIn(float value, float min, float max){
		return value < max && value > min;
	}

	void OnCollisionEnter2D(Collision2D c){
		gameObject.SetActive(false);
	}
}
