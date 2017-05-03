using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolController : MonoBehaviour {
	private static List<GameObject> objectPool;

	public static GameObject InstantiateFromPool(GameObject obj, Vector2 pos, Quaternion rot){
		GameObject res;

		string objTag = obj.tag;

		if(objectPool == null){
			objectPool = new List<GameObject>();
		}

		// optimize this search
		int objIdx = -1;
		for(int i = 0; i < objectPool.Count; i++){
			if(objectPool[i] == null){
				objectPool.RemoveAt(i);
				i--;
				continue;
			}
			if(objectPool[i].tag.Equals(objTag) && !objectPool[i].activeSelf){
				objIdx = i;
				break;
			}
		}

		// if we found it, use the pooled object instead of making a new one
		if(objIdx != -1){
			res = objectPool[objIdx];
			res.SetActive(true);
		} else {
			// otherwise, instantiate it and store it in the pool
			res = Instantiate(obj, pos, rot);
			objectPool.Add(res);
		}

		res.transform.position = pos;
		res.transform.rotation = rot;
		return res;
	}
}
