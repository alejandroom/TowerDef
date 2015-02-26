using UnityEngine;
using System.Collections;

public class Proyectil : MonoBehaviour {

	public Transform target;
	
	public int speed;
	public int damage;
	public bool slow = false;
	public bool area = false;

	private bool moving = false;

	public void Go () {
		moving = true;
	}

	void Update(){
		if (!moving)
			return;
		transform.LookAt (target.position);
		this.rigidbody.velocity = transform.TransformDirection(Vector3.forward * speed);
	}

	void OnTriggerEnter (Collider other) {
		Enemy intruder = other.GetComponentInParent<Enemy> ();
		if (intruder == null)
			return;

		if (other.transform != target && !area)
			return;
		
		intruder.AdjustCurHealth(-damage);
		if(slow)
			intruder.slow(0.5f, 3.0f);
		Destroy (gameObject);
	}
}
