using UnityEngine;
using System.Collections;

public class Blizzard : MonoBehaviour {
	
	public Transform target;
	public float lifeTime;
	public float timeSlow;
	public float slowAmount;

	private float initialTime;

	void Start(){
		initialTime = Time.time;
		timeSlow = 1.0f;
		slowAmount = 0.5f;
	}
	
	void Update(){
		if (Time.time - initialTime > lifeTime)
			Destroy (gameObject);
		transform.position = new Vector3 (target.transform.position.x, transform.position.y, target.transform.position.z);
	}
	
	void OnTriggerStay (Collider other) {
		Enemy intruder = other.GetComponent<Enemy> ();
		if (intruder == null)
			return;
		
		intruder.slow(slowAmount, timeSlow);
	}

	void OnTriggerEnter (Collider other) {
		Enemy intruder = other.GetComponent<Enemy> ();
		if (intruder == null)
			return;
		
		intruder.slow(slowAmount, timeSlow);
	}
}
