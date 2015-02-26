using UnityEngine;
using System.Collections;

public class Rotator : MonoBehaviour {

	public float rotation;
	
	void Update () {
		transform.Rotate(new Vector3(0, rotation*Time.deltaTime, 0));
	}
}
