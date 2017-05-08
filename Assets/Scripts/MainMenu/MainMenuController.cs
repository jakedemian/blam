using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour {
	public GameObject cursor;
	public List<GameObject> menuItems;

	private int cursorIdx = 0;

	// Use this for initialization
	void Start () {
		for(int i = 0; i < menuItems.Count; i++){
			Debug.Log(i + ": " + menuItems[i].GetComponent<Text>().transform.position);
		}
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.S)){
			cursorIdx++;
		} else if(Input.GetKeyDown(KeyCode.W)){
			cursorIdx--;
		}

		Vector2 menuItemPos = menuItems[cursorIdx].GetComponent<RectTransform>().position;

		cursor.transform.position = new Vector2(menuItemPos.x - 15f, menuItemPos.y);
	}
}
