using UnityEngine;
using System.Collections;

public class Turret : MonoBehaviour {
	
	public Transform target;

	private Quaternion initial;
	
	void Start(){
		initial = transform.rotation;
	}
	
	void Update(){
		if (target != null)
			transform.LookAt (target.position);
		else
			transform.rotation = initial;
	}
}
