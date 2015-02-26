using UnityEngine;
using System.Collections;

public class Arrow : MonoBehaviour {

	public int path;

	void OnTriggerEnter (Collider other) {
		Enemy intruder = other.GetComponentInParent<Enemy> ();
		if (intruder != null && path == intruder.path)
			intruder.newDirection (transform.TransformDirection (Vector3.forward));
		
		Healer heal = other.GetComponentInParent<Healer> ();
		if (heal != null)
			heal.newDirection (transform.TransformDirection (Vector3.forward));
	}
}
