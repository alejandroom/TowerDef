using UnityEngine;
using System.Collections;

public class AutoCannon : MonoBehaviour {
	
	private float initialPos;
	private float retroceso=0.04f;
	
	void Start () {
		initialPos = transform.localPosition.z;
	}
	
	void Update () {
		if(transform.localPosition.z<initialPos)
			transform.localPosition = new Vector3 (transform.localPosition.x, transform.localPosition.y , transform.localPosition.z+(retroceso*Time.deltaTime*10));
		if(transform.localPosition.z>initialPos)		
			transform.localPosition = new Vector3 (transform.localPosition.x, transform.localPosition.y, initialPos);
	}
	
	public void bang(){
		transform.localPosition = new Vector3 (transform.localPosition.x, transform.localPosition.y, initialPos - retroceso);
	}
}
