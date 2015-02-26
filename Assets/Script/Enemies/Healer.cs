using UnityEngine;
using System.Collections;

public class Healer : MonoBehaviour {
	
	public int healing=100;
	public float speed=1.0f;

	public Vector3 direction;
	
	void Start () {
		direction = transform.TransformDirection(Vector3.forward);
	}
	
	void Update(){
		this.rigidbody.velocity = direction * speed;
	}

	public void newDirection(Vector3 newD){
		direction = newD;
	}
}
