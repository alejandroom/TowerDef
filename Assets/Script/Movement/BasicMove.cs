using UnityEngine;
using System.Collections;

public class BasicMove : MonoBehaviour {
	public int speed=10;
	
	// Update is called once per frame
	void Update () {
		rigidbody.AddForce (new Vector3 (Input.GetAxis ("Horizontal"), 0.0f, Input.GetAxis ("Vertical"))*speed*Time.deltaTime);
	}
}
